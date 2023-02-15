using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tau.CodeGenerator.Abstractions.FS;

namespace Tau.CodeGenerator.Core.FS;

internal sealed class LocalFileSystem : IFileSystem
{
    private readonly string _workingDirectory;

    public LocalFileSystem(string workingDirectory)
    {
        _workingDirectory = workingDirectory;
    }

    public ValueTask DisposeAsync() => new ValueTask();

    public ValueTask<IFile> Open(string path, Abstractions.FS.FileMode fileMode, OpenMode openMode, CancellationToken ct)
    {
        var fullPath = Path.Join(_workingDirectory, path);
        FileAttributes attr;
        try
        {
            attr = File.GetAttributes(fullPath);
        }
        catch (FileNotFoundException)
        {
            throw new NotFoundException(fullPath);
        }
        catch (DirectoryNotFoundException)
        {
            throw new NotFoundException(fullPath);
        }
        var isDirectory = attr.HasFlag(FileAttributes.Directory);
        var description = new FileDescription(isDirectory, path, fullPath);
        var file = new LocalDiskFile(description, fileMode, openMode);
        return new ValueTask<IFile>(file);
    }
}
