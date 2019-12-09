using System;
using System.Drawing;
using System.Linq;
using System.Text;
using NLog;

namespace StegoF5.Extensions
{
    internal static class CommonExtensions
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        internal static string GenerateBitsSequence(int bitsLength)
        {
            var random = new Random();
            var resultString = new StringBuilder();

            for (var i = 0; i < bitsLength; i++)
            {
                resultString.Append(random.Next(0, 2));
            }

            return resultString.ToString();
        }

        internal static string ToBitsString(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Warn("Convert to bits string failed! String for convert is null or Empty!");
                return null;
            }

            try
            {
                return value.Select(x => Convert.ToString(x, 2).PadLeft(16, '0')).ToString();
            }
            catch (Exception ex)
            {
                Logger.Error("Convert to bits string failed!", ex.Message, ex.StackTrace);
                return null;
            }
        }

        internal static string ToBitsString(this Bitmap value)
        {
            if (value == null)
            {
                Logger.Warn("Convert to bits string failed! Image for convert is null or Empty!");
                return null;
            }

            if (value.Height == 0 || value.Width == 0)
            {
                Logger.Warn("Convert to bits string failed! Width or Height of image is 0!");
                return null;
            }

            var resultString = new StringBuilder();
            for (var x = 0; x < value.Height; x++)
            {
                for (var y = 0; y < value.Width; y++)
                {
                    try
                    {
                        var pixel = value.GetPixel(x, y);
                        resultString.Append(Convert.ToString(pixel.R, 2).PadLeft(8, '0'));
                        resultString.Append(Convert.ToString(pixel.G, 2).PadLeft(8, '0'));
                        resultString.Append(Convert.ToString(pixel.B, 2).PadLeft(8, '0'));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Convert to bits string failed!", ex.Message, ex.StackTrace);
                        return null;
                    }
                }
            }

            return resultString.ToString();
        }

        internal static Bitmap ToBitmap(this string value, int widthImage, int heightImage)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Warn("Convert string to Bitmap failed! String is null or empty");
                return null;
            }

            if (widthImage == 0 || heightImage == 0)
            {
                Logger.Error("Width or Height of image is 0 when converting string to Image!");
                return null;
            }

            var image = new Bitmap(widthImage, heightImage);
            var length = 0;//binary string length counter

            for (var x = 0; x < heightImage; x++)
            {
                for (var y = 0; y < widthImage; y++)
                {
                    try
                    {
                        byte red, green, blue;
                        if (length < value.Length)
                        {
                            red = Convert.ToByte(value.Substring(length, 8), 2);
                            length += 8;
                        }
                        else
                        {
                            red = 0;
                        }

                        if (length < value.Length)
                        {
                            green = Convert.ToByte(value.Substring(length, 8), 2);
                            length += 8;
                        }
                        else
                        {
                            green = 0;
                        }

                        if (length < value.Length)
                        {
                            blue = Convert.ToByte(value.Substring(length, 8), 2);
                            length += 8;
                        }
                        else
                        {
                            blue = 0;
                        }

                        image.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Convert string to Bitmap failed!", ex.Message, ex.StackTrace);
                        return null;
                    }
                }
            }

            return image;
        }

        internal static string ToTextString(this string value, int textLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Error("Convert string to text string failed! Start string is null or empty!");
                return null;
            }

            var resultString = new StringBuilder();

            for (var i = 0; i < textLength; i++)
            {
                try
                {
                    var letter = Convert.ToInt32(value.Substring(i * 16, 16), 2);
                    resultString.Append(Convert.ToChar(letter));
                }
                catch (Exception ex)
                {
                    Logger.Error("Convert string to text string failed!", ex.Message, ex.StackTrace);
                    return null;
                }
            }

            return resultString.ToString();
        }

        internal static string ToTextString(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Error("Convert string to text string failed! Start string is null or empty!");
                return null;
            }

            var resultString = new StringBuilder();
            var countChars = value.Length / 16;

            for (var i = 0; i < countChars; i++)
            {
                try
                {
                    var letter = Convert.ToInt32(value.Substring(i * 16, 16), 2);
                    resultString.Append(Convert.ToChar(letter));
                }
                catch (Exception ex)
                {
                    Logger.Error("Convert string to text string failed!", ex.Message, ex.StackTrace);
                    return null;
                }
            }

            return resultString.ToString();
        }

        internal static byte[] ToByteArray(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Error("Convert string to byte array failed! String is null or empty!");
                return null;
            }

            try
            {
                return value.Select(x => Convert.ToByte(x.ToString())).ToArray();
            }
            catch (Exception ex)
            {
                Logger.Error("Convert string to byte array failed!", ex.Message, ex.StackTrace);
                return null;
            }
        }

        internal static string ToCompleteStringEmptyBits(this string value, int border)
        {
            if (string.IsNullOrEmpty(value))
            {
                Logger.Error("Completing string empty bits failed! String is null or empty!");
                return null;
            }

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
            if (image == null)
            {
                Logger.Error("Convert Bitmap to pixels array failed! Bitmap is null!");
                return null;
            }

            if (image.Width == 0 || image.Height == 0)
            {
                Logger.Error("Convert Bitmap to pixels array failed! Width or Height of bitmap is 0!");
                return null;
            }

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
            if (pixels == null || pixels.Length == 0)
            {
                Logger.Error("Convert pixels array to Bitmap failed! Pixels array is null or empty");
                return null;
            }

            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);
            var image = new Bitmap(width, height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    image.SetPixel(x, y, pixels[x, y]);
                }
            }

            return image;
        }
    }
}
