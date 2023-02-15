using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tau.CodeGenerator.Abstractions.IR;

public sealed class IRString : IRNode
{
    private static readonly HashSet<string> _cacheableKeys
        = new HashSet<string>
        {
            "",
            "y", "Y", "yes", "Yes", "YES",
            "t", "T", "true", "True", "TRUE",
            "n", "N", "no", "No", "NO",
            "f", "F", "false", "False", "FALSE",
            "value", "Value", "key", "Key",
            "name", "Name", "type", "Type",
            "kind", "Kind",
        };

    private static readonly ConcurrentDictionary<string, IRString> _cache
        = new ConcurrentDictionary<string, IRString>();

    public static IRString Create(string value)
    {
        if (_cacheableKeys.Contains(value))
        {
            return _cache.GetOrAdd(value, static (value) => new IRString(value));
        }
        return new IRString(value);
    }

    public string Value { get; }

    private IRString(string value)
        : base(IRNodeKind.String)
    {
        Value = value;
    }

    internal override void AcceptVisitor(IRVisitor visitor)
    {
        visitor.VisitString(this);
    }
}
