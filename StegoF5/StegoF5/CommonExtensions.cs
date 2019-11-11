using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StegoF5
{
    internal static class CommonExtensions
    {
        internal static string GenerateBitsSequence(int bitsLength)
        {
            var random = new Random();
            var resultString = new StringBuilder();

            //генерация псевдослучайной битовой последовательности
            for (var i = 0; i < bitsLength; i++)
            {
                resultString.Append(random.Next(0, 2));
            }

            return resultString.ToString();
        }

        internal static string ToBitsString(this string value)
        {
            return value.Select(x => Convert.ToString(x, 2).PadLeft(16, '0')).ToString();
        }

        internal static string ToBitsString(this Bitmap value)
        {
            var resultString = new StringBuilder();
            for (var x = 0; x < value.Height; x++)
            {
                for (var y = 0; y < value.Width; y++)
                {
                    Color pixel = value.GetPixel(x, y);
                    resultString.Append(Convert.ToString(pixel.R, 2).PadLeft(8, '0'));
                    resultString.Append(Convert.ToString(pixel.G, 2).PadLeft(8, '0'));
                    resultString.Append(Convert.ToString(pixel.B, 2).PadLeft(8, '0'));
                }
            }

            return resultString.ToString();
        }
    }
}
