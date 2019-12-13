﻿using System;
using System.Drawing;
using System.Linq;
using NLog;
using StegoF5.Extensions;
using StegoF5.Models;

namespace StegoF5.Services
{
    internal class BaseF5Service
    {
        protected static Logger Logger = LogManager.GetCurrentClassLogger();

        protected WorkSpace FormWorkspace(Color[,] container, AreaEmbeddingModel areaEmbedding)
        {
            var rows = container.GetLength(0);
            var columns = container.GetLength(1);
            var workSpace = new WorkSpace();
            for (var x = 0; x < rows; x++)
            {
                for (var y = 0; y < columns; y++)
                {
                    for (var i = 0; i < 8; i++)
                    {
                        try
{
                        if (areaEmbedding.SignificantBits["R"][i])
                        {
                            workSpace.Significantbits += container[x, y].R.GetBit(i);
                        }

                        if (areaEmbedding.SignificantBits["G"][i])
                        {
                            workSpace.Significantbits += container[x, y].G.GetBit(i);
                        }

                        if (areaEmbedding.SignificantBits["B"][i])
                        {
                            workSpace.Significantbits += container[x, y].B.GetBit(i);
                        }

                        if (areaEmbedding.InSignificantBits["R"][i])
                        {
                            workSpace.Insignificantbits += container[x, y].R.GetBit(i);
                        }

                        if (areaEmbedding.InSignificantBits["G"][i])
                        {
                            workSpace.Insignificantbits += container[x, y].G.GetBit(i);
                        }

                        if (areaEmbedding.InSignificantBits["B"][i])
                        {
                            workSpace.Insignificantbits += container[x, y].B.GetBit(i);
                        }
}
                        catch (Exception ex)
                        {
                            Logger.Error("Forming Workspace failed!", ex.Message, ex.StackTrace);
                            return null;
                        }
                    }
                }
            }

            return workSpace;
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

        protected byte[] GetWord(WorkSpace workSpace, int insignificantBitsLength, int significantBitsLength, int countWords)
        {
            var insignificantBits = workSpace.Insignificantbits.Substring(countWords * insignificantBitsLength, insignificantBitsLength);
            var significantBits = workSpace.Significantbits.Substring(countWords * significantBitsLength, significantBitsLength);

            return string.Concat(insignificantBits, significantBits).Select(x => Convert.ToByte(x.ToString())).ToArray();
        }
    }
}
