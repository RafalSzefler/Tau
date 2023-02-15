using System.Collections.Generic;
using Tau.CodeGenerator.Abstractions.IR;

namespace Tau.CodeGenerator.Abstractions.Models;

public sealed class TauService
{
    public TauIdentifier Name { get; }
    public TauIdentifierPath Namespace { get; }
    public TauIdentifierPath FullName { get; }
    public IReadOnlyList<TauIdMessageType> ReturnTypes { get; }
    public IReadOnlyList<TauIdMessageType> ParameterTypes { get; }
    public IRObject AdditionalSettings { get; }

    public TauService(
        TauIdentifier name,
        TauIdentifierPath @namespace,
        IReadOnlyList<TauIdMessageType> returnTypes,
        IReadOnlyList<TauIdMessageType> parameterTypes,
        IRObject? additionalSettings)
    {
        Name = name;
        Namespace = @namespace;
        ReturnTypes = returnTypes;
        ParameterTypes = parameterTypes;
        var path = @namespace.AsArray().AsUnsafeArray();
        var fullPath = new TauIdentifier[path.Length + 1];
        path.CopyTo(fullPath, 0);
        fullPath[fullPath.Length - 1] = name;
        FullName = new TauIdentifierPath(fullPath);
        AdditionalSettings = additionalSettings ?? IRObject.Create(null);
    }
}
