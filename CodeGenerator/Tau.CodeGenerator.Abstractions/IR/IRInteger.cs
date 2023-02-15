using System.Collections.Concurrent;

namespace Tau.CodeGenerator.Abstractions.IR;

public sealed class IRInteger : IRNode
{
    private static readonly ConcurrentDictionary<int, IRInteger> _cache
        = new ConcurrentDictionary<int, IRInteger>();

    public static IRInteger Create(int value)
    {
        if (value >= -1 && value <= 32)
        {
            return _cache.GetOrAdd(value, static (value) => new IRInteger(value));
        }
        return new IRInteger(value);
    }

    public int Value { get; }

    private IRInteger(int value)
        : base(IRNodeKind.Integer)
    {
        Value = value;
    }

    internal override void AcceptVisitor(IRVisitor visitor)
    {
        visitor.VisitInteger(this);
    }
}
