using System.Collections.Generic;

namespace Tau.CodeGenerator.Abstractions.IR;

public sealed class IRObject : IRNode
{
    private static readonly Dictionary<string, IRNode> _emptyDict
        = new Dictionary<string, IRNode>();
    private static readonly IRObject _empty
        = new IRObject(_emptyDict);

    public static IRObject Create(IReadOnlyDictionary<string, IRNode>? properties)
    {
        if (properties == null || properties.Count == 0)
        {
            return _empty;
        }
        return new IRObject(properties);
    }

    public IReadOnlyDictionary<string, IRNode> Properties { get; }

    private IRObject(IReadOnlyDictionary<string, IRNode> properties)
        : base(IRNodeKind.Object)
    {
        Properties = properties;
    }

    internal override void AcceptVisitor(IRVisitor visitor)
    {
        visitor.VisitObject(this);
    }
}
