namespace Tau.CodeGenerator.Abstractions.Models;

public sealed class TauIdMessageType
{
    public int Id { get; }
    public TauType ReturnType { get; }

    public TauIdMessageType(int id, TauType returnType)
    {
        Id = id;
        ReturnType = returnType;
    }
}
