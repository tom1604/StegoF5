using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using StegoF5.Extensions;
using StegoF5.Interfaces;
using StegoF5.Models;

namespace StegoF5.Services
{
    internal class F5EmbeddingService : BaseF5Service, IEmbeddable
    {
        public Bitmap Embed(Bitmap image, int wordLength, int significantBitsLength,
             AreaEmbeddingModel areaEmbedding, Matrix matrix, string binInformation)
        {
            var information = binInformation.ToCompleteStringEmptyBits(significantBitsLength).ToByteArray();
            var imagePixels = image.ToPixelsArray();
            var insignificantBitsLength = wordLength - significantBitsLength;

            return ValidateParams(imagePixels, wordLength, significantBitsLength, areaEmbedding, matrix, information)
                ? EmbedCore(imagePixels, insignificantBitsLength, significantBitsLength, areaEmbedding, matrix,
                    information)
                : null;
        }

        private static bool ValidateParams(Color[,] imagePixels, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, IEnumerable information)
        {
            if (information == null)
            {
                Logger.Fatal("Embedding information failed! Embedding information is null or empty!");
                return false;
            }

            if (imagePixels == null || imagePixels.Length == 0)
            {
                Logger.Fatal("Embedding information failed! Image container is null or empty!");
                return false;
            }

            if (wordLength == 0)
            {
                Logger.Fatal("Embedding information failed! Word Length is null!");
                return false;
            }

            if (significantBitsLength == 0)
            {
                Logger.Fatal("Embedding information failed! Significant Bits Length is null!");
                return false;
            }

            if (matrix == null || matrix.Rows == 0 || matrix.Columns == 0)
            {
                Logger.Fatal("Embedding information failed! Matrix is null or empty!");
                return false;
            }

            if (areaEmbedding.SignificantBits == null && areaEmbedding.InSignificantBits == null)
            {
                /*TODO Check dictionary on valid*/
                Logger.Fatal("Embedding information failed! Area Embedding is null or empty!");
                return false;
            }

            return true;
        }

        private Bitmap EmbedCore(Color[,] imagePixels, int insignificantBitsLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, IReadOnlyList<byte> binInformation)
        {
            var countWords = 0;
            var count = 0;
            var changedWorkspace = new List<byte[]>();

            Logger.Info("Embed information: Input values are valid");
            //формирование рабочей области
            var workSpace = FormWorkspace(imagePixels, areaEmbedding);
            Logger.Info("Embed information: The working area of the image extracted");
            //Встраивание битовой строки в рабочую область контейнера
            while ((countWords < binInformation.Count / significantBitsLength) 
                   && (countWords * significantBitsLength <= workSpace.Significantbits.Length - significantBitsLength) 
                   && (countWords * insignificantBitsLength <= workSpace.Insignificantbits.Length - insignificantBitsLength))
            {
                //получение слова из рабочей области изображения (значимые и незначимые биты)
                var word = GetWord(workSpace, insignificantBitsLength, significantBitsLength, countWords);
                countWords++;
                //получение синдрома от слова
                var syndrom = GetSyndrom(matrix, word);
                // сложение синдрома и встраиваемой битовой строкой по модулю два
                for (var i = 0; i < significantBitsLength - 1; i++)
                {
                    syndrom[i] = (byte)((syndrom[i] + binInformation[count + i]) % 2);
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
            Logger.Info("Embed information: Embedding a workspace in a container");
            var pixels = EmbedWorkspace(imagePixels, areaEmbedding, changedWorkspace, significantBitsLength);
            Logger.Info("Embed information: Information is embedded");
            return pixels.ToBitmapImage();
        }

        private static byte[] FindErrorVector(Matrix matrix, IReadOnlyList<byte> syndrom)
        {
            var vector = new byte[matrix.Rows];
            for (var i = 0; i < matrix.Rows; i++)
            {
                var count = 0;
                for (var j = 0; j < matrix.Columns; j++)
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

        protected byte[] FindWeightErrorVector(Matrix matrix, byte[] syndrom, int significantBitsLength)
        {
            var vector = FindErrorVector(matrix, syndrom);
            if (!vector.IsNullVector())
            {
                return vector;
            }

            var numberColumn = FindNumberColumn(matrix, syndrom, significantBitsLength);
            if (numberColumn == null)
            {
                return vector;
            }

            vector = new byte[matrix.Rows];
            for (var i = 0; i < vector.Length; i++)
            {
                vector[i] = (byte) (i == numberColumn[0] || i == numberColumn[1] ? 1 : 0);
            }

            return vector;
        }

        private static List<int> FindNumberColumn(Matrix matrix, byte[] syndrom, int significantBitsLength)
        {
            List<int> numberColumn = null;
            var lowestWeightColumn = int.MaxValue;
            var length = matrix.Rows;
            for (var i = 0; i < length - 1; i++)
            {
                for (var j = i + 1; j < length; j++)
                {
                    var vector = matrix.SumColomn(i, j);
                    if (syndrom.EqualVector(vector))
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

        private static Color[,] EmbedWorkspace(Color[,] container, AreaEmbeddingModel areaEmbedding, IEnumerable<byte[]> workspace, int signbitsLength)
        {
            var significantbits = new List<byte>();
            var insignificantbits = new List<byte>();
            var width = container.GetLength(0);
            var height = container.GetLength(1);

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
                        if (areaEmbedding.SignificantBits["R"][i] && signCount < significantbits.Count)
                        {
                            red = red.SetBit(i, significantbits[signCount] == 1);
                            signCount++;
                        }
                        if (areaEmbedding.SignificantBits["G"][i] && signCount < significantbits.Count)
                        {
                            green = green.SetBit(i, significantbits[signCount] == 1);
                            signCount++;
                        }
                        if (areaEmbedding.SignificantBits["B"][i] && signCount < significantbits.Count)
                        {
                            blue = blue.SetBit(i, significantbits[signCount] == 1);
                            signCount++;
                        }
                        if (areaEmbedding.SignificantBits["R"][i] && insignCount < insignificantbits.Count)
                        {
                            red = red.SetBit(i, insignificantbits[insignCount] == 1);
                            insignCount++;
                        }
                        if (areaEmbedding.SignificantBits["G"][i] && insignCount < insignificantbits.Count)
                        {
                            green = green.SetBit(i, insignificantbits[insignCount] == 1);
                            insignCount++;
                        }
                        if (areaEmbedding.SignificantBits["B"][i] && insignCount < insignificantbits.Count)
                        {
                            blue = blue.SetBit(i, insignificantbits[insignCount] == 1);
                            insignCount++;
                        }
                    }
                    container[x, y] = Color.FromArgb(container[x, y].A, red, green, blue);
                    if (insignCount >= insignificantbits.Count && signCount >= significantbits.Count)
                    {
                        return container;
                    }
                }
            }

            return container;
        }
    }
}
