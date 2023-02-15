namespace Tau.CodeGenerator.Abstractions.IR;

public sealed class IRNull : IRNode
{
    public static readonly IRNull Value = new IRNull();

    public static IRNull Create() => Value;

    private IRNull()
        : base(IRNodeKind.Null)
    { }

    internal override void AcceptVisitor(IRVisitor visitor)
    {
        visitor.VisitNull(this);
    }
}
