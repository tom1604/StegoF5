using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using StegoF5.Extensions;
using StegoF5.Interfaces;

namespace StegoF5.Services
{
    internal class F5EmbeddingService : BaseF5Service, IEmbeddable
    {
        public Bitmap Embed(Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, byte[,] matrix, string binInformation)
        {
            var countWords = 0;
            var count = 0;
            var changedWorkspace = new List<byte[]>();
            binInformation = binInformation.ToCompleteStringEmptyBits(significantBitsLength);
            var information = binInformation.ToByteArray();
            var imagePixels = image.ToPixelsArray();
            var insignificantBitsLength = wordLength - significantBitsLength;
            //формирование рабочей области
            FormWorkspace(imagePixels, areaEmdedding);
            //Встраивание битовой строки в рабочую область контейнера
            while ((countWords < information.Length / significantBitsLength) && (countWords * significantBitsLength <= Significantbits.Length - significantBitsLength) && (countWords * insignificantBitsLength <= Insignificantbits.Length - insignificantBitsLength))
            {
                //получение слова из рабочей области изображения (значимые и незначимые биты)
                var word = GetWord(insignificantBitsLength, significantBitsLength, countWords);
                countWords++;
                //получение синдрома от слова
                var syndrom = GetSyndrom(matrix, word);
                // сложение синдрома и встраиваемой битовой строкой по модулю два
                for (var i = 0; i < significantBitsLength - 1; i++)
                {
                    syndrom[i] = (byte)((syndrom[i] + information[count + i]) % 2);
                }
                count += significantBitsLength - 1;
                //получение вектора ошибки
                var vector = FindWeightErrorVector(matrix, syndrom, significantBitsLength);
                //сложение вектора ошибки со словом
                for (var i = 0; i < word.Length; i++)
                {
                    word[i] = (byte)((word[i] + vector[i]) % 2);
                }
                changedWorkspace.Add(word);
            }
            //встраивание рабочей области в контейнер
            var pixels = EmbedWorkspace(imagePixels, areaEmdedding, changedWorkspace, significantBitsLength);

            return pixels.ToBitmapImage();
        }

        private byte[] FindErrorVector(byte[,] matrix, IReadOnlyList<byte> syndrom)
        {
            var vector = new byte[matrix.GetLength(0)];
            var row = matrix.GetLength(0);
            var column = matrix.GetLength(1);
            for (var i = 0; i < row; i++)
            {
                var count = 0;
                for (var j = 0; j < column; j++)
                {
                    if (matrix[i, j] != syndrom[j])
                    {
                        break;
                    }

                    count++;
                }

                vector[i] = (byte) (count == syndrom.Count ? 1 : 0);
            }

            return vector;
        }

        protected byte[] FindWeightErrorVector(byte[,] matrix, byte[] syndrom, int significantBitsLength)
        {
            var vector = FindErrorVector(matrix, syndrom);
            if (!CheckNullVector(vector))
            {
                return vector;
            }

            var numberColumn = FindNumberColumn(matrix, syndrom, significantBitsLength);
            if (numberColumn == null)
            {
                return vector;
            }

            vector = new byte[matrix.GetLength(0)];
            for (var i = 0; i < vector.Length; i++)
            {
                vector[i] = (byte) (i == numberColumn[0] || i == numberColumn[1] ? 1 : 0);
            }

            return vector;
        }

        private List<int> FindNumberColumn(byte[,] matrix, byte[] syndrom, int significantBitsLength)
        {
            List<int> numberColumn = null;
            var lowestWeightColumn = int.MaxValue;
            var length = matrix.GetLength(0);
            for (var i = 0; i < length - 1; i++)
            {
                for (var j = i + 1; j < length; j++)
                {
                    var vector = SumColomn(matrix, i, j);
                    if (CheckVector(syndrom, vector))
                    {
                        var currentWeight = (i < significantBitsLength ? 1 : 2) + (j < significantBitsLength ? 1 : 2);
                        if (currentWeight < lowestWeightColumn)
                        {
                            numberColumn = new List<int> { i, j };
                            lowestWeightColumn = currentWeight;
                        }
                    }
                }
            }

            return numberColumn;
        }

        private static bool CheckVector(IEnumerable<byte> syndrom, IReadOnlyList<byte> vector)
        {
            return !syndrom.Where((t, i) => t != vector[i]).Any();
        }

        private static byte[] SumColomn(byte[,] matrix, int first, int second)
        {
            var result = new byte[matrix.GetLength(0)];
            for (var i = 0; i < matrix.GetLength(1); i++)
            {
                result[i] = (byte)((matrix[first, i] + matrix[second, i]) % 2);
            }

            return result;
        }

        private static bool CheckNullVector(IEnumerable<byte> vector)
        {
            return vector.All(t => t == 0);
        }

        private static Color[,] EmbedWorkspace(Color[,] container, IReadOnlyList<Dictionary<string, bool[]>> areaEmdedding, IEnumerable<byte[]> workspace, int signbitsLength)
        {
            var significantbits = new List<byte>();
            var insignificantbits = new List<byte>();
            var width = container.GetLength(0);
            var height = container.GetLength(1);
            var exit = false;//флаг выход из циклов
            foreach (var it in workspace)
            {
                for (var i = 0; i < it.Length; i++)
                {
                    if (i >= signbitsLength - 1)
                    {
                        significantbits.Add(it[i]);
                    }
                    else
                    {
                        insignificantbits.Add(it[i]);
                    }
                }
            }

            var signCount = 0;//счетчик значимых бит
            var insignCount = 0;//счетчик незначимых бит

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    int red = container[x, y].R;
                    int green = container[x, y].G;
                    int blue = container[x, y].B;
                    for (var i = 0; i < 8; i++)
                    {
                        if (areaEmdedding[0]["R"][i] && (signCount < significantbits.Count))
                        {
                            red = significantbits[signCount] == 1 ? red | (1 << i) : red & ~(1 << i);
                            signCount++;
                        }
                        if (areaEmdedding[0]["G"][i] && (signCount < significantbits.Count))
                        {
                            green = significantbits[signCount] == 1 ? green | (1 << i) : green & ~(1 << i);
                            signCount++;
                        }
                        if (areaEmdedding[0]["B"][i] && (signCount < significantbits.Count))
                        {
                            blue = significantbits[signCount] == 1 ? blue | (1 << i) : blue & ~(1 << i);
                            signCount++;
                        }
                        if (areaEmdedding[1]["R"][i] && (insignCount < insignificantbits.Count))
                        {
                            red = insignificantbits[insignCount] == 1 ? red | (1 << i) : red & ~(1 << i);
                            insignCount++;
                        }
                        if (areaEmdedding[1]["G"][i] && (insignCount < insignificantbits.Count))
                        {
                            green = insignificantbits[insignCount] == 1 ? green | (1 << i) : green & ~(1 << i);
                            insignCount++;
                        }
                        if (areaEmdedding[1]["B"][i] && (insignCount < insignificantbits.Count))
                        {
                            blue = insignificantbits[insignCount] == 1 ? blue | (1 << i) : blue & ~(1 << i);
                            insignCount++;
                        }
                    }
                    container[x, y] = Color.FromArgb(container[x, y].A, red, green, blue);
                    if (insignCount >= insignificantbits.Count && signCount >= significantbits.Count)
                    {
                        exit = true;
                        break;
                    }
                }

                if (exit)
                { break;}
            }

            return container;
        }
    }
}
