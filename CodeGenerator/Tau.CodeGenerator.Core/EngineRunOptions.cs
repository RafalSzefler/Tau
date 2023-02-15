using Tau.CodeGenerator.Abstractions.FS;

namespace Tau.CodeGenerator.Core;

public readonly struct EngineRunOptions
{
    public string ProjectFileName { get; }
    public IFile ProjectDirectory { get; }
    public IFile OutputDirectory { get; }

    public EngineRunOptions(
        string projectFileName,
        IFile projectDirectory,
        IFile outputDirectory)
    {
        ProjectFileName = projectFileName;
        ProjectDirectory = projectDirectory;
        OutputDirectory = outputDirectory;
    }
}
