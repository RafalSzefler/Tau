using System;
using Tau.CodeGenerator.Abstractions;

namespace Tau.CodeGenerator.Core;

public sealed class EngineBuilder
{
    private static readonly ITauProjectConverter _tauProjectConverter
        = new TauProjectConverter();

    private IInputParser? _inputParser;
    private ICodeGenerator? _codeGenerator;

    public EngineBuilder SetInputParser(IInputParser parser)
    {
        _inputParser = parser;
        return this;
    }

    public EngineBuilder SetCodeGenerator(ICodeGenerator codeGenerator)
    {
        _codeGenerator = codeGenerator;
        return this;
    }

    public IEngine Build()
    {
        if (_inputParser == null)
        {
            throw new ArgumentException($"[{nameof(IInputParser)}] has to be set through [{nameof(SetInputParser)}] method.");
        }

        if (_codeGenerator == null)
        {
            throw new ArgumentException($"[{nameof(ICodeGenerator)}] has to be set through [{nameof(SetCodeGenerator)}] method.");
        }

        return new Engine(_inputParser, _codeGenerator, _tauProjectConverter);
    }
}
