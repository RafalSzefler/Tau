using System.Threading;
using System.Threading.Tasks;
using Tau.CodeGenerator.Abstractions.FS;
using Tau.CodeGenerator.Abstractions.Models;

namespace Tau.CodeGenerator.Abstractions;

public interface ICodeGenerator
{
    public string Name { get; }
    public TauVersion Version { get; }
    public ValueTask Generate(
        IFile outputDirectory,
        TauProject project,
        CancellationToken ct);
}
