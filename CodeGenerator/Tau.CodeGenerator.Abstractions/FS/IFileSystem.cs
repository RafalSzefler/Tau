using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tau.CodeGenerator.Abstractions.FS;

public interface IFileSystem : IAsyncDisposable
{
    public ValueTask<IFile> Open(string path, FileMode fileMode, OpenMode openMode, CancellationToken ct);
}
