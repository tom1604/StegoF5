using System.Collections.Generic;
using System.Drawing;

namespace StegoF5
{
    public static class StegoF5
    {
        static StegoF5()
        {

        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, int bitsLength)
        {
            var bitsSequance = CommonExtensions.GenerateBitsSequence(bitsLength);
            //встраивания сгенерированной битовой строки в контейнре
            //Embed(areaEmdedding, sb.ToString());
            return new Bitmap(20,20);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, string text)
        {
            var bitsString = text.ToBitsString();
            //встраивания битовой строки в контейнер
            //Embed(areaEmdedding, sb.ToString());
            return new Bitmap(20, 20);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, Bitmap embeddableImage)
        {
            var bitsString = embeddableImage.ToBitsString();
            //встраивание битовой строки в контейнер
            //Embed(areaEmdedding, sb.ToString());
            return new Bitmap(20, 20);
        }

        public static Bitmap ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, int widthImage, int heightImage)
        {
            string binImage = string.Empty; //битовая строка изображения
            //извлечение изображения из стегоконтейнера
            //binImage = Extract(areaEmdedding, width * height * 3 * 8);

            return binImage.ToBitmap(widthImage, heightImage);
        }
    }
}
