using System;
using System.Drawing;
using NLog;
using StegoF5.Extensions;
using StegoF5.Interfaces;
using StegoF5.Models;

namespace StegoF5
{
    public static class StegoF5
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        private static IModifiable Modifier { get; }

        static StegoF5()
        {
            Modifier = Activator.CreateInstance<IModifiable>();
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, int bitsLength)
        {
            var bitsSequance = ConvertExtensions.GenerateBitsSequence(bitsLength).ToCompleteStringEmptyBits(significantBitsLength);
            if (string.IsNullOrEmpty(bitsSequance))
            {
                Logger.Fatal("Embedding information failed! Embedding information is null or empty!");
                return null;
            }

            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmbedding, matrix, bitsSequance);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, string text)
        {
            var bitsString = text.ToBitsString()?.ToCompleteStringEmptyBits(significantBitsLength);
            if (string.IsNullOrEmpty(bitsString))
            {
                Logger.Fatal("Embedding information failed! Embedding information is null or empty");
                return null;
            }

            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmbedding, matrix, bitsString);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, Bitmap embeddableImage)
        {
            var bitsString = embeddableImage?.ToBitsString()?.ToCompleteStringEmptyBits(significantBitsLength);
            if (string.IsNullOrEmpty(bitsString))
            {
                Logger.Fatal("Embedding information failed! Embedding information is null or empty");
                return null;
            }

            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmbedding, matrix, bitsString);
        }

        public static Bitmap ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, int widthImage, int heightImage)
        {
            var countBits = widthImage * heightImage * 3 * 8;
            var binImage = Modifier.Extract(image, wordLength, significantBitsLength, areaEmbedding, matrix, countBits);
            return binImage.ToBitmap(widthImage, heightImage);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, int textLength)
        {
            var countBits = textLength * 16;
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmbedding, matrix, countBits);
            return binText.ToTextString(textLength);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix)
        {
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmbedding, matrix, null);
            return binText.ToTextString();
        }
    }
}
