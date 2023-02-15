using System;
using System.Collections.Generic;
using System.Linq;

namespace Tau.CodeGenerator.Abstractions.Models;

public readonly struct TauIdentifierPath : IEquatable<TauIdentifierPath>
{
    private readonly TauIdentifier[] _ids;
    private readonly int _hash;

    public TauIdentifierPath(TauIdentifier[]? ids)
    {
        _ids = ids ?? Array.Empty<TauIdentifier>();

        const uint prime = 0x01000193;
        const uint basis = 0x811c9dc5;

        var hash = basis;
        for (var i = 0; i < _ids.Length; i++)
        {
            unchecked {
                hash = hash ^ (uint)_ids[i].GetHashCode();
                hash = hash * prime;
            }
        }

        _hash = (int)hash;
    }

    public IReadOnlyList<TauIdentifier> AsArray() => _ids;

    public override string ToString()
        => String.Join(Globals.IdentifierSeparator, _ids.Select(static id => id.ToString()));

    public override int GetHashCode() => _hash;

    public bool Equals(TauIdentifierPath other)
        => other._hash == _hash && _ids.SequenceEqual(other._ids);

    public override bool Equals(object obj)
        => obj is TauIdentifierPath id && Equals(id);

    public static bool operator==(TauIdentifierPath left, TauIdentifierPath right)
        => left.Equals(right);

    public static bool operator!=(TauIdentifierPath left, TauIdentifierPath right)
        => !left.Equals(right);
}
