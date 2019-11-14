using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StegoF5.Extensions
{
    internal static class CommonExtensions
    {
        internal static string GenerateBitsSequence(int bitsLength)
        {
            var random = new Random();
            var resultString = new StringBuilder();

            //генерация псевдослучайной битовой последовательности
            for (var i = 0; i < bitsLength; i++)
            {
                resultString.Append(random.Next(0, 2));
            }

            return resultString.ToString();
        }

        internal static string ToBitsString(this string value)
        {
            return value.Select(x => Convert.ToString(x, 2).PadLeft(16, '0')).ToString();
        }

        internal static string ToBitsString(this Bitmap value)
        {
            var resultString = new StringBuilder();
            for (var x = 0; x < value.Height; x++)
            {
                for (var y = 0; y < value.Width; y++)
                {
                    Color pixel = value.GetPixel(x, y);
                    resultString.Append(Convert.ToString(pixel.R, 2).PadLeft(8, '0'));
                    resultString.Append(Convert.ToString(pixel.G, 2).PadLeft(8, '0'));
                    resultString.Append(Convert.ToString(pixel.B, 2).PadLeft(8, '0'));
                }
            }

            return resultString.ToString();
        }

        internal static Bitmap ToBitmap(this string value, int widthImage, int heightImage)
        {
            var image = new Bitmap(widthImage, heightImage);
            //сохранение извлеченного изображения
            int length = 0;//счетчик длины бинарной строки
            for (int x = 0; x < heightImage; x++)
            {
                for (int y = 0; y < widthImage; y++)
                {
                    byte red, green, blue;
                    if (length < value.Length)
                    {
                        red = Convert.ToByte(value.Substring(length, 8), 2);
                        length += 8;
                    }
                    else red = 0;

                    if (length < value.Length)
                    {
                        green = Convert.ToByte(value.Substring(length, 8), 2);
                        length += 8;
                    }
                    else green = 0;

                    if (length < value.Length)
                    {
                        blue = Convert.ToByte(value.Substring(length, 8), 2);
                        length += 8;
                    }
                    else blue = 0;

                    image.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }

            return image;
        }

        internal static string ToTextString(this string value, int textLength)
        {
            var resultString = new StringBuilder();
            for (var i = 0; i < textLength; i++)
            {
                try
                {
                    var letter = Convert.ToInt32(value.Substring(i * 16, 16), 2);
                    resultString.Append(Convert.ToChar(letter));
                }
                catch (Exception)
                { break; }
            }

            return resultString.ToString();
        }

        internal static string ToTextString(this string value)
        {
            var resultString = new StringBuilder();
            var countChars = value.Length / 16;
            for (var i = 0; i < countChars; i++)
            {
                try
                {
                    var letter = Convert.ToInt32(value.Substring(i * 16, 16), 2);
                    resultString.Append(Convert.ToChar(letter));
                }
                catch (Exception)
                { break; }
            }

            return resultString.ToString();
        }

        internal static byte[] ToByteArray(this string value)
        {
            return value.Select(x => Convert.ToByte(x.ToString())).ToArray();
        }

        internal static string ToCompleteStringEmptyBits(this string value, int border)
        {
            if (border == 0)
            {
                return value;
            }

            while (value.Length % border != 0)
            {
                value += "0";
            }

            return value;
        }

        internal static Color[,] ToPixelsArray(this Bitmap image)
        {
            var pixelsArray = new Color[image.Height, image.Width];
            for (var x = 0; x < image.Height; x++)
            {
                for (var y = 0; y < image.Width; y++)
                {
                    pixelsArray[x, y] = image.GetPixel(x, y);
                }
            }

            return pixelsArray;
        }

        internal static Bitmap ToBitmapImage(this Color[,] pixels)
        {
            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);
            var image = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    image.SetPixel(x, y, pixels[x, y]);
                }
            }

            return image;
        }
    }
}
