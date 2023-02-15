using Tau.CodeGenerator.Abstractions.Models;

namespace Tau.CodeGenerator.Abstractions;

public interface IPlugin
{
    public string Name { get; }
    public TauVersion Version { get; }
    public ICodeGenerator? CreateCodeGenerator();
    public IInputParser? CreateInputParser();
}
