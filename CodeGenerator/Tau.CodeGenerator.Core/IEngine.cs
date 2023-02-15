using System.Threading;
using System.Threading.Tasks;

namespace Tau.CodeGenerator.Core;

public interface IEngine
{
    public ValueTask Run(EngineRunOptions runOptions, CancellationToken ct);
}
