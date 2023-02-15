namespace Tau.CodeGenerator.Abstractions.IR;

public sealed class IRBoolean : IRNode
{
    public static readonly IRBoolean TrueValue = new IRBoolean(true);
    public static readonly IRBoolean FalseValue = new IRBoolean(false);

    public static IRBoolean Create(bool value) => value ? TrueValue : FalseValue;

    public bool Value { get; }

    private IRBoolean(bool value)
        : base(IRNodeKind.Boolean)
    {
        Value = value;
    }

    internal override void AcceptVisitor(IRVisitor visitor)
    {
        visitor.VisitBoolean(this);
    }
}
