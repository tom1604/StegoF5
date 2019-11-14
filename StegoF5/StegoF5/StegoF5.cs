using System;
using System.Collections.Generic;
using System.Drawing;
using StegoF5.Extensions;

namespace StegoF5
{
    public static class StegoF5
    {
        private static IModifiable Modifier { get; }

        static StegoF5()
        {
            Modifier = Activator.CreateInstance<IModifiable>();
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, int bitsLength)
        {
            var bitsSequance = CommonExtensions.GenerateBitsSequence(bitsLength).ToCompleteStringEmptyBits(significantBitsLength);
            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmdedding, bitsSequance);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, string text)
        {
            var bitsString = text.ToBitsString().ToCompleteStringEmptyBits(significantBitsLength);
            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmdedding, bitsString);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, Bitmap embeddableImage)
        {
            var bitsString = embeddableImage.ToBitsString().ToCompleteStringEmptyBits(significantBitsLength);
            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmdedding, bitsString);
        }

        public static Bitmap ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, int widthImage, int heightImage)
        {
            string binImage = string.Empty; //битовая строка изображения
            //извлечение изображения из стегоконтейнера
            //binImage = Extract(areaEmdedding, width * height * 3 * 8);

            return binImage.ToBitmap(widthImage, heightImage);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, int textLength)
        {
            var binText = string.Empty;//битовая строка текста
           // binText = Extract(areaEmdedding, textLength * 16);//извлечение текста из стегоконтейнера

            return binText.ToTextString(textLength);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding)
        {
            var binText = string.Empty;//битовая строка текста
            // binText = Extract(areaEmdedding, textLength * 16);//извлечение текста из стегоконтейнера

            return binText.ToTextString();
        }
    }
}
