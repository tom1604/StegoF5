using System;
using System.Collections.Generic;
using System.Linq;

namespace StegoF5.Extensions
{
    internal static class MathExtensions
    {
        internal static int GetBit(this byte value, int numberBit)
        {
            return Convert.ToByte(value & (1 << numberBit)) >> numberBit;
        }

        internal static int SetBit(this int value, int numberBit, bool notNull)
        {
            return notNull ? value | (1 << numberBit) : value & ~(1 << numberBit);
        }

        internal static bool EqualVector(this IEnumerable<byte> syndrom, IReadOnlyList<byte> vector)
        {
            return !syndrom.Where((t, i) => t != vector[i]).Any();
        }

        internal static bool IsNullVector(this IEnumerable<byte> vector)
        {
            return vector.All(t => t == 0);
        }
    }
}
