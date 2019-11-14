using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using StegoF5.Extensions;

namespace StegoF5
{
    internal class F5Embedding : BaseF5, IEmbeddable
    {
        public Bitmap Embed(Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, string binInformation)
        {
            var countWords = 0;
            var count = 0;
            var changedWorkspace = new List<byte[]>();
            binInformation = binInformation.ToCompleteStringEmptyBits(significantBitsLength);
            var information = binInformation.ToByteArray();
            var imagePixels = image.ToPixelsArray();
            //формирование рабочей области
            FormWorkspace(imagePixels, areaEmdedding);
            //Встраивание битовой строки в рабочую область контейнера
            while ((countWords < (information.Length / significantBitsLength)) && ((countWords * significantBitsLength) <= (Significantbits.Length - significantBitsLength)) && ((countWords * (wordLength - significantBitsLength)) <= (Insignificantbits.Length - (wordLength - significantBitsLength))))
            {
                //получение слова из рабочей области изображения (значимые и незначимые биты)
                byte[] word = (Insignificantbits.Substring(countWords * (wordLength - significantBitsLength), (wordLength - significantBitsLength)) + Significantbits.Substring(countWords * significantBitsLength, significantBitsLength)).Select(x => Convert.ToByte(x.ToString())).ToArray();
                countWords++;
                //получение синдрома от слова
                byte[] syndrom;
                if (wordLength == 7) syndrom = CalculateSyndrom(Matrix3x7, word);
                else syndrom = CalculateSyndrom(Matrix4x9, word);
                // сложение синдрома и встраиваемой битовой строкой по модулю два
                for (int i = 0; i < SignificantBitsLength - 1; i++)
                {
                    syndrom[i] = (byte)((syndrom[i] + information[count + i]) % 2);
                }
                count += SignificantBitsLength - 1;
                //получение вектора ошибки
                byte[] vector;
                if (wordLength == 7) vector = FindWeightErrorVector(Matrix3x7, syndrom, significantBitsLength);
                else vector = FindWeightErrorVector(Matrix4x9, syndrom, significantBitsLength);
                //сложение вектора ошибки со словом
                for (int i = 0; i < word.Length; i++)
                {
                    word[i] = (byte)((word[i] + vector[i]) % 2);
                }
                changedWorkspace.Add(word);
            }
            //встраивание рабочей области в контейнер
            var pixels = EmbedWorkspace(imagePixels, areaEmdedding, changedWorkspace, significantBitsLength);

            return pixels.ToBitmapImage();
        }
    }
}
