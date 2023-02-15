using System.Collections.Generic;
using System.Linq;

namespace Tau.CodeGenerator.Abstractions;

internal static class Extensions
{
    public static T[] AsUnsafeArray<T>(this IReadOnlyList<T> array)
    {
        if (array is T[] unsafeResult)
        {
            return unsafeResult;
        }
        return array.ToArray();
    }
}
