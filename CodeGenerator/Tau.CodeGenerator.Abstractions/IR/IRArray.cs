using System;
using System.Collections.Generic;

namespace Tau.CodeGenerator.Abstractions.IR;

public sealed class IRArray : IRNode
{
    private static readonly IRArray _empty = new IRArray(Array.Empty<IRNode>());
    private static IRArray Create(IReadOnlyList<IRNode>? elements)
    {
        if (elements == null || elements.Count == 0)
        {
            return _empty;
        }
        return new IRArray(elements);
    }

    public IReadOnlyList<IRNode> Elements { get; }

    private IRArray(IReadOnlyList<IRNode> elements)
        : base(IRNodeKind.Array)
    {
        Elements = elements;
    }

    internal override void AcceptVisitor(IRVisitor visitor)
    {
        visitor.VisitArray(this);
    }
}
