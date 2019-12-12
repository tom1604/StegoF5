using System;

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
    }
}
