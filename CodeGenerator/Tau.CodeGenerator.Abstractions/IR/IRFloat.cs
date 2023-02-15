using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tau.CodeGenerator.Abstractions.IR;

public sealed class IRFloat : IRNode
{
    private static readonly HashSet<float> _cacheableKeys
        = new HashSet<float>
        {
            -1, float.NaN, float.PositiveInfinity,
            float.NegativeInfinity, 0, 1, 2
        };

    private static readonly ConcurrentDictionary<float, IRFloat> _cache
        = new ConcurrentDictionary<float, IRFloat>();

    public static IRFloat Create(float value)
    {
        if (_cacheableKeys.Contains(value))
        {
            return _cache.GetOrAdd(value, static (value) => new IRFloat(value));
        }
        return new IRFloat(value);
    }

    public float Value { get; }

    private IRFloat(float value)
        : base(IRNodeKind.Float)
    {
        Value = value;
    }

    internal override void AcceptVisitor(IRVisitor visitor)
    {
        visitor.VisitFloat(this);
    }
}
