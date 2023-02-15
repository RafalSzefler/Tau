using System;

namespace Tau.CodeGenerator.Abstractions.Models;

public readonly struct TauVersion : IEquatable<TauVersion>
{
    private readonly int _major;
    private readonly int _minor;
    private readonly int _hash;

    public TauVersion(int major, int minor)
    {
        _major = major;
        _minor = minor;

        const uint prime = 0x01000193;
        const uint basis = 0x811c9dc5;

        var hash = basis;
        unchecked {
            hash = hash ^ (uint)_major.GetHashCode();
            hash = hash * prime;
            hash = hash ^ (uint)_minor.GetHashCode();
            hash = hash * prime;
        }
        _hash = (int)hash;
    }

    public override string ToString() => $"{_major}.{_minor}";

    public override int GetHashCode() => _hash;

    public bool Equals(TauVersion other)
        => other._hash == _hash
        && other._major == _major
        && other._minor == _minor;

    public override bool Equals(object obj)
        => obj is TauVersion id && Equals(id);

    public static bool operator==(TauVersion left, TauVersion right)
        => left.Equals(right);

    public static bool operator!=(TauVersion left, TauVersion right)
        => !left.Equals(right);
}
