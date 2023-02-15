namespace Tau.CodeGenerator.Abstractions.IR;

public abstract class IRVisitor
{
    protected internal abstract void VisitObject(IRObject node);
    protected internal abstract void VisitArray(IRArray node);
    protected internal abstract void VisitBoolean(IRBoolean node);
    protected internal abstract void VisitInteger(IRInteger node);
    protected internal abstract void VisitFloat(IRFloat node);
    protected internal abstract void VisitString(IRString node);
    protected internal abstract void VisitNull(IRNull node);

    public void Visit(IRNode node)
    {
        node.AcceptVisitor(this);
    }
}
