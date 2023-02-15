using System.Collections.Generic;
using Tau.CodeGenerator.Abstractions.IR;

namespace Tau.CodeGenerator.Abstractions.Models;

public sealed class TauMessageType : TauType
{
    public IReadOnlyCollection<TauMessageTypeField> Fields { get; }

    public TauMessageType(
        TauIdentifier name,
        TauIdentifierPath @namespace,
        IRObject? additionalSettings,
        IReadOnlyCollection<TauMessageTypeField> fields
    ) : base(name, @namespace, additionalSettings)
    {
        Fields = fields;
    }
}
