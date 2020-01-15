using System.Drawing;
using System.Text;
using StegoF5.Extensions;
using StegoF5.Interfaces;
using StegoF5.Models;

namespace StegoF5.Services
{
    internal class F5ExtractingService : BaseF5Service, IExtractable
    {
        public string Extract(Bitmap image, int wordLength, int significantBitsLength, AreaEmbeddingModel areaEmbedding,
            Matrix matrix, int? countBits)
        {
            var imagePixels = image.ToPixelsArray();
            var insignificantBitsLength = wordLength - significantBitsLength;

            return ValidateParams(imagePixels, wordLength, significantBitsLength, areaEmbedding, matrix, countBits)
                ? ExtractCore(imagePixels, insignificantBitsLength, significantBitsLength, areaEmbedding, matrix,
                    countBits)
                : null;
        }

        private string ExtractCore(Color[,] imagePixels, int insignificantBitsLength, int significantBitsLength, AreaEmbeddingModel areaEmbedding, 
            Matrix matrix, int? countBits)
        {
            var binInformation = new StringBuilder();
            var countWords = 0;//счетчик кодовых слов
            if (countBits == null)
            {
                countBits = imagePixels.GetLength(0) * imagePixels.GetLength(1) * 3 * 8;
            }
            //получение рабочей области
            var workSpace = FormWorkspace(imagePixels, areaEmbedding);
            //извлечение битов из кодовых слов рабочей области стегоконтейнера
            while (countWords < (countBits / significantBitsLength)
                   && (countWords * significantBitsLength) <= (workSpace.Significantbits.Length - significantBitsLength)
                   && (countWords * insignificantBitsLength) <= (workSpace.Insignificantbits.Length - insignificantBitsLength))
            {
                //формировани кодового слова
                var word = GetWord(workSpace, insignificantBitsLength, significantBitsLength, countWords);
                countWords++;
                var syndrom = GetSyndrom(matrix, word);
                foreach (var bit in syndrom)
                {
                    binInformation.Append(bit);
                }
            }

            return binInformation.ToString();
        }

        private static bool ValidateParams(Color[,] imagePixels, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, int? countBits)
        {
            if (countBits == 0)
            {
                Logger.Fatal("Extracting information failed! Input variable countBits is 0!");
                return false;
            }

            if (imagePixels == null || imagePixels.Length == 0)
            {
                Logger.Fatal("Extracting information failed! Image container is null or empty!");
                return false;
            }

            if (wordLength == 0)
            {
                Logger.Fatal("Extracting information failed! Word Length is null!");
                return false;
            }

            if (significantBitsLength == 0)
            {
                Logger.Fatal("Extracting information failed! Significant Bits Length is null!");
                return false;
            }

            if (matrix == null || matrix.Rows == 0 || matrix.Columns == 0)
            {
                Logger.Fatal("Extracting information failed! Matrix is null or empty!");
                return false;
            }

            if (areaEmbedding.SignificantBits == null && areaEmbedding.InSignificantBits == null)
            {
                /*TODO Check dictionary on valid*/
                Logger.Fatal("Extracting information failed! Area Embedding is null or empty!");
                return false;
            }

            return true;
        }

    }
}