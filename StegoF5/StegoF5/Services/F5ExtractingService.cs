using System.Drawing;
using System.Text;
using StegoF5.Extensions;
using StegoF5.Interfaces;
using StegoF5.Models;

namespace StegoF5.Services
{
    internal class F5ExtractingService : BaseF5Service, IExtractable
    {
        public string Extract(Bitmap image, int wordLength, int significantBitsLength, AreaEmbeddingModel areaEmdedding, byte[,] matrix, int? countBits)
        {
            var binInformation = new StringBuilder();
            var countWords = 0;//счетчик кодовых слов
            var imagePixels = image.ToPixelsArray();
            var insignificantBitsLength = wordLength - significantBitsLength;
            if (countBits == null)
            {
                countBits = imagePixels.GetLength(0) * imagePixels.GetLength(1) * 3 * 8;
            }
            //получение рабочей области
            var workSpace = FormWorkspace(imagePixels, areaEmdedding);
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

    }
}