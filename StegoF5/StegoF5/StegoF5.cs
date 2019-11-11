using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < bitmap.Height; x++)
            {
                for (int y = 0; y < bitmap.Width; y++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    sb.Append(Convert.ToString(pixel.R, 2).PadLeft(8, '0'));
                    sb.Append(Convert.ToString(pixel.G, 2).PadLeft(8, '0'));
                    sb.Append(Convert.ToString(pixel.B, 2).PadLeft(8, '0'));
                }
            }
            //встраивание битовой строки в контейнер
            //Embed(areaEmdedding, sb.ToString());
            return new Bitmap(20, 20);
        }
    }
}
