using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tau.CodeGenerator.Abstractions.FS;

public interface IFile : IAsyncDisposable
{
    public FileDescription Description { get; }
    public FileMode FileMode { get; }
    public OpenMode OpenMode { get; }
    public ValueTask<int> GetSize();
    public ValueTask Write(ReadOnlyMemory<byte> input, CancellationToken ct);
    public ValueTask Read(Memory<byte> output, CancellationToken ct);
    public ValueTask<IReadOnlyList<FileDescription>> ListFiles(CancellationToken ct);
    public ValueTask Delete(CancellationToken ct);
}
