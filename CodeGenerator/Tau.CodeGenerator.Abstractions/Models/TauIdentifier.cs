using System;

namespace Tau.CodeGenerator.Abstractions.Models;

public readonly struct TauIdentifier : IEquatable<TauIdentifier>
{
    private readonly string _value;
    private readonly int _hash;

    public TauIdentifier(string? value)
    {
        _value = value ?? "";
        _hash = _value.GetHashCode();
    }

    public override string ToString() => _value;

    public override int GetHashCode() => _hash;

    public bool Equals(TauIdentifier other)
        => other._hash == _hash && other._value == _value;

    public override bool Equals(object obj)
        => obj is TauIdentifier id && Equals(id);

    public static bool operator==(TauIdentifier left, TauIdentifier right)
        => left.Equals(right);

    public static bool operator!=(TauIdentifier left, TauIdentifier right)
        => !left.Equals(right);
}
