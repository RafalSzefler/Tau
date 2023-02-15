namespace Tau.CodeGenerator.Abstractions.IR;

public abstract class IRNode
{
    public IRNodeKind Kind { get; }

    protected IRNode(IRNodeKind kind)
    {
        Kind = kind;
    }

    internal abstract void AcceptVisitor(IRVisitor visitor);
}
