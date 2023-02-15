using Tau.CodeGenerator.Abstractions.IR;

namespace Tau.CodeGenerator.Abstractions.Models;

public abstract class TauType
{
    public TauIdentifier Name { get; }
    public TauIdentifierPath Namespace { get; }
    public TauIdentifierPath FullName { get; }
    public IRObject AdditionalSettings { get; }

    protected TauType(
        TauIdentifier name,
        TauIdentifierPath @namespace,
        IRObject? additionalSettings)
    {
        Name = name;
        Namespace = @namespace;
        var path = @namespace.AsArray().AsUnsafeArray();
        var fullPath = new TauIdentifier[path.Length + 1];
        path.CopyTo(fullPath, 0);
        fullPath[fullPath.Length - 1] = name;
        FullName = new TauIdentifierPath(fullPath);
        AdditionalSettings = additionalSettings ?? IRObject.Create(null);
    }
}
