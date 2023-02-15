using System.Threading;
using System.Threading.Tasks;
using Tau.CodeGenerator.Abstractions.FS;
using Tau.CodeGenerator.Abstractions.IR;
using Tau.CodeGenerator.Abstractions.Models;

namespace Tau.CodeGenerator.Abstractions;

public interface IInputParser
{
    public string Name { get; }
    public TauVersion Version { get; }
    public ValueTask<IRNode> ParseProject(
        string projectFileName,
        IFile inputDirectory,
        CancellationToken ct);
}
