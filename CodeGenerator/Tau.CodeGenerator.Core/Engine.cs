using System.Threading;
using System.Threading.Tasks;
using Tau.CodeGenerator.Abstractions;

namespace Tau.CodeGenerator.Core;

internal sealed class Engine : IEngine
{
    private readonly IInputParser _inputParser;
    private readonly ICodeGenerator _codeGenerator;
    private readonly ITauProjectConverter _tauProjectConverter;

    public Engine(
        IInputParser inputParser,
        ICodeGenerator codeGenerator,
        ITauProjectConverter tauProjectConverter)
    {
        _inputParser = inputParser;
        _codeGenerator = codeGenerator;
        _tauProjectConverter = tauProjectConverter;
    }

    public async ValueTask Run(EngineRunOptions runOptions, CancellationToken ct)
    {
        var input = await _inputParser.ParseProject(runOptions.ProjectFileName, runOptions.ProjectDirectory, ct);
        var project = _tauProjectConverter.Convert(input);
        await _codeGenerator.Generate(runOptions.OutputDirectory, project, ct);
    }
}
