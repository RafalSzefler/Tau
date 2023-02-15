using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tau.CodeGenerator.Abstractions.FS;

namespace Tau.CodeGenerator.Core.FS;


internal sealed class LocalDiskFile : IFile
{
    public FileDescription Description { get; }

    public Abstractions.FS.FileMode FileMode { get; }

    public OpenMode OpenMode { get; }

    private readonly Lazy<FileStream> _fileStream;

    public LocalDiskFile(
        FileDescription descriptor,
        Abstractions.FS.FileMode fileMode,
        OpenMode openMode)
    {
        Description = descriptor;
        FileMode = fileMode;
        OpenMode = openMode;
        _fileStream = new Lazy<FileStream>(() => {
            if (Description.IsDirectory)
            {
                throw new InvalidOperationException("Cannot open directory as stream");
            }
            return System.IO.File.Open(Description.FullPath, Map(OpenMode), Map(FileMode));
        });
    }

    public ValueTask Delete(CancellationToken ct)
    {
        if (Description.IsDirectory)
        {
            System.IO.Directory.Delete(Description.FullPath, true);
        }
        else
        {
            System.IO.File.Delete(Description.FullPath);
        }
        return new ValueTask();
    }

    public ValueTask DisposeAsync()
    {
        if (_fileStream.IsValueCreated)
        {
            return _fileStream.Value.DisposeAsync();
        }
        return new ValueTask();
    }

    public ValueTask<IReadOnlyList<FileDescription>> ListFiles(CancellationToken ct)
    {
        if (!Description.IsDirectory)
        {
            throw new InvalidOperationException($"[{nameof(IFile)}] instance is not a directory.");
        }
        var descriptions = new List<FileDescription>(8);
        foreach (var name in System.IO.Directory.EnumerateDirectories(Description.FullPath))
        {
            descriptions.Add(new FileDescription(true, name, Path.Join(Description.FullPath, name)));
        }
        foreach (var name in System.IO.Directory.EnumerateFiles(Description.FullPath))
        {
            descriptions.Add(new FileDescription(false, name, Path.Join(Description.FullPath, name)));
        }
        return new ValueTask<IReadOnlyList<FileDescription>>(descriptions);
    }

    public async ValueTask Read(Memory<byte> output, CancellationToken ct)
    {
        var stream = _fileStream.Value;

        while (true)
        {
            var read = await stream.ReadAsync(output, ct);
            if (read == 0)
            {
                return;
            }
            output = output.Slice(read);
        }
    }

    public ValueTask Write(ReadOnlyMemory<byte> input, CancellationToken ct)
    {
        var stream = _fileStream.Value;
        return stream.WriteAsync(input, ct);
    }

    public ValueTask<int> GetSize()
    {
        if (Description.IsDirectory)
        {
            throw new InvalidOperationException($"[{nameof(GetSize)}] can be called only on files, not directories.");
        }
        var size = new System.IO.FileInfo(Description.FullPath).Length;
        const int maxSize = int.MaxValue / 4;
        if (size >= maxSize)
        {
            throw new InvalidOperationException($"File [{Description.FullPath}] too big. Max size allowed is: {maxSize}.");
        }
        return new ValueTask<int>((int)size);
    }

    private static System.IO.FileMode Map(OpenMode mode)
    {
        switch (mode)
        {
            case OpenMode.Get:
                return System.IO.FileMode.Open;
            case OpenMode.Create:
                return System.IO.FileMode.Create;
            case OpenMode.GetOrCreate:
                return System.IO.FileMode.OpenOrCreate;
            default:
                throw new NotImplementedException();
        }
    }

    private static System.IO.FileAccess Map(Abstractions.FS.FileMode mode)
    {
        switch (mode)
        {
            case Abstractions.FS.FileMode.Read:
                return System.IO.FileAccess.Read;
            case Abstractions.FS.FileMode.Write:
                return System.IO.FileAccess.ReadWrite;
            default:
                throw new NotImplementedException();
        }
    }
}