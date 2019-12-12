using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using StegoF5.Extensions;
using StegoF5.Interfaces;
using StegoF5.Models;

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
            AreaEmbeddingModel areaEmdedding, byte[,] matrix, int bitsLength)
        {
            var bitsSequance = ConvertExtensions.GenerateBitsSequence(bitsLength).ToCompleteStringEmptyBits(significantBitsLength);
            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmdedding, matrix, bitsSequance);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmdedding, byte[,] matrix, string text)
        {
            var bitsString = text.ToBitsString().ToCompleteStringEmptyBits(significantBitsLength);
            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmdedding, matrix, bitsString);
        }

        public static Bitmap EmbedInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmdedding, byte[,] matrix, Bitmap embeddableImage)
        {
            var bitsString = embeddableImage.ToBitsString().ToCompleteStringEmptyBits(significantBitsLength);
            return Modifier.Embed(image, wordLength, significantBitsLength, areaEmdedding, matrix, bitsString);
        }

        public static Bitmap ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmdedding, byte[,] matrix, int widthImage, int heightImage)
        {
            var countBits = widthImage * heightImage * 3 * 8;
            var binImage = Modifier.Extract(image, wordLength, significantBitsLength, areaEmdedding, matrix, countBits);
            return binImage.ToBitmap(widthImage, heightImage);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmdedding, byte[,] matrix, int textLength)
        {
            var countBits = textLength * 16;
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmdedding, matrix, countBits);
            return binText.ToTextString(textLength);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmdedding, byte[,] matrix)
        {
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmdedding, matrix, null);
            return binText.ToTextString();
        }
    }
}
