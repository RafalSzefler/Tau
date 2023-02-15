using Tau.CodeGenerator.Abstractions.IR;

namespace Tau.CodeGenerator.Abstractions.Models;

public sealed class TauMessageTypeField
{
    public int Id { get; }
    public TauIdentifier Name { get; }
    public TauType FieldType { get; }

    public TauMessageTypeField(int id, TauIdentifier name, TauType fieldType)
    {
        Id = id;
        Name = name;
        FieldType = fieldType;
    }
}
