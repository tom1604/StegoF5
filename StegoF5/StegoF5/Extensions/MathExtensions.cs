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

        internal static int SetBit(this int value, int numberBit, bool flag)
        {
            return flag ? value | (1 << numberBit) : value & ~(1 << numberBit);
        }

        internal static byte[] SumColomn(this byte[,] matrix, int first, int second)
        {
            var result = new byte[matrix.GetLength(0)];
            for (var i = 0; i < matrix.GetLength(1); i++)
            {
                result[i] = (byte)((matrix[first, i] + matrix[second, i]) % 2);
            }

            return result;
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
