namespace Tau.CodeGenerator.Abstractions.IR;

public enum IRNodeKind : byte
{
    UNKNOWN = 0,
    Null = 1,
    Boolean = 2,
    Integer = 3,
    Float = 4,
    String = 5,
    Array = 6,
    Object = 7,
}
