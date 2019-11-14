using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StegoF5
{
    internal class BaseF5
    {
        protected string Significantbits { get; set; }

        protected string Insignificantbits { get; set; }

        protected void FormWorkspace(Color[,] container, Dictionary<string, bool[]>[] areaEmdedding)
        {
            var rows = container.GetLength(0);
            var columns = container.GetLength(1);
            var significantbits = new StringBuilder();
            var insignificantbits = new StringBuilder();
            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < columns; y++)
                {
                    for (var i = 0; i < 8; i++)
                    {
                        if (areaEmdedding[0]["R"][i])
                        {
                            significantbits.Append(Convert.ToByte(container[x, y].R & (1 << i)) >> i);
                        }

                        if (areaEmdedding[0]["G"][i])
                        {
                            significantbits.Append(Convert.ToByte(container[x, y].G & (1 << i)) >> i);
                        }

                        if (areaEmdedding[0]["B"][i])
                        {
                            significantbits.Append(Convert.ToByte(container[x, y].B & (1 << i)) >> i);
                        }

                        if (areaEmdedding[1]["R"][i])
                        {
                            insignificantbits.Append(Convert.ToByte(container[x, y].R & (1 << i)) >> i);
                        }

                        if (areaEmdedding[1]["G"][i])
                        {
                            insignificantbits.Append(Convert.ToByte(container[x, y].G & (1 << i)) >> i);
                        }

                        if (areaEmdedding[1]["B"][i])
                        {
                            insignificantbits.Append(Convert.ToByte(container[x, y].B & (1 << i)) >> i);
                        }
                    }
                }
            }
            Significantbits = significantbits.ToString();
            Insignificantbits = insignificantbits.ToString();
        }

        protected byte[] GetSyndrom(byte[,] matrix, byte[] word)
        {
            var syndrom = new byte[matrix.GetLength(1)];
            for (var i = 0; i < matrix.GetLength(1); i++)
            {
                for (var j = 0; j < matrix.GetLength(0); j++)
                {
                    syndrom[i] += (byte)(word[j] * matrix[j, i]);
                }
                syndrom[i] %= 2;
            }

            return syndrom;
        }

        protected byte[] GetWord(int insignificantBitsLength, int significantBitsLength, int countWords)
        {
            var insignificantBits = Insignificantbits.Substring(countWords * insignificantBitsLength, insignificantBitsLength);
            var significantBits = Significantbits.Substring(countWords * significantBitsLength, significantBitsLength);

            return string.Concat(insignificantBits, significantBits).Select(x => Convert.ToByte(x.ToString())).ToArray();
        }
    }
}
