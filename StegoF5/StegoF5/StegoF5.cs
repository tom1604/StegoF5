﻿using System;
using System.Collections.Generic;
using System.Drawing;
using StegoF5.Extensions;
using StegoF5.Interfaces;

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
            var countBits = widthImage * heightImage * 3 * 8;
            var binImage = Modifier.Extract(image, wordLength, significantBitsLength, areaEmdedding, countBits);
            return binImage.ToBitmap(widthImage, heightImage);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, int textLength)
        {
            var countBits = textLength * 16;
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmdedding, countBits);
            return binText.ToTextString(textLength);
        }

        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding)
        {
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmdedding, null);
            return binText.ToTextString();
        }
    }
}
