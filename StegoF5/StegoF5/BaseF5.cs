using System;
using System.Collections.Generic;
using System.Drawing;
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

        protected Color[,] EmbedWorkspace(Color[,] container, Dictionary<string, bool[]>[] areaEmdedding, List<byte[]> workspace, int signbitsLength)
        {
            List<byte> significantbits = new List<byte>();
            List<byte> insignificantbits = new List<byte>();
            bool exit = false;//флаг выход из циклов
            foreach (var it in workspace)
            {
                for (int i = 0; i < it.Length; i++)
                {
                    if (i >= (signbitsLength - 1)) significantbits.Add(it[i]);
                    else insignificantbits.Add(it[i]);
                }
            }
            int signCount = 0;//счетчик значимых бит
            int insignCount = 0;//счетчик незначимых бит
            for (int x = 0; x < container.GetLength(0); x++)
            {
                for (int y = 0; y < container.GetLength(1); y++)
                {
                    int red = container[x, y].R;
                    int green = container[x, y].G;
                    int blue = container[x, y].B;
                    for (int i = 0; i < 8; i++)
                    {
                        if (areaEmdedding[0]["R"][i] && (signCount < significantbits.Count))
                        {
                            if (significantbits[signCount] == 1) red |= (1 << i);
                            else red &= ~(1 << i);
                            signCount++;
                        }
                        if (areaEmdedding[0]["G"][i] && (signCount < significantbits.Count))
                        {
                            if (significantbits[signCount] == 1) green |= (1 << i);
                            else green &= ~(1 << i);
                            signCount++;
                        }
                        if (areaEmdedding[0]["B"][i] && (signCount < significantbits.Count))
                        {
                            if (significantbits[signCount] == 1) blue |= (1 << i);
                            else blue &= ~(1 << i);
                            signCount++;
                        }
                        if (areaEmdedding[1]["R"][i] && (insignCount < insignificantbits.Count))
                        {
                            if (insignificantbits[insignCount] == 1) red |= (1 << i);
                            else red &= ~(1 << i);
                            insignCount++;
                        }
                        if (areaEmdedding[1]["G"][i] && (insignCount < insignificantbits.Count))
                        {
                            if (insignificantbits[insignCount] == 1) green |= (1 << i);
                            else green &= ~(1 << i);
                            insignCount++;
                        }
                        if (areaEmdedding[1]["B"][i] && (insignCount < insignificantbits.Count))
                        {
                            if (insignificantbits[insignCount] == 1) blue |= (1 << i);
                            else blue &= ~(1 << i);
                            insignCount++;
                        }
                    }
                    container[x, y] = Color.FromArgb(container[x, y].A, red, green, blue);
                    if ((insignCount >= insignificantbits.Count) && (signCount >= significantbits.Count))
                    {
                        exit = true;
                        break;
                    }
                }
                if (exit) break;
            }
            return container;
        }
    }
}
