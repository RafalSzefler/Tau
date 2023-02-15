namespace Tau.CodeGenerator.Abstractions.FS;

public sealed class FileDescription
{
    public bool IsDirectory { get; }
    public string Name { get; }
    public string FullPath { get; }

    public FileDescription(bool isDirectory, string name, string fullPath)
    {
        IsDirectory = isDirectory;
        Name = name;
        FullPath = fullPath;
    }
}
