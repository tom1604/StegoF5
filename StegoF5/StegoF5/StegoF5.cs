using System;
using System.Drawing;
using NLog;
using StegoF5.Extensions;
using StegoF5.Interfaces;
using StegoF5.Models;

namespace StegoF5
{
    /// <summary>
    /// StegoF5 static class is main in library
    /// </summary>
    public static class StegoF5
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        private static IModifiable Modifier { get; }

        static StegoF5()
        {
            Modifier = Activator.CreateInstance<IModifiable>();
        }

        /// <summary>
        /// Method for embedding a pseudo-random sequence in image
        /// </summary>
        /// <param name="image">Bitmap image to embed information in</param>
        /// <param name="wordLength">The length of the code words</param>
        /// <param name="significantBitsLength">Number of significant bits</param>
        /// <param name="areaEmbedding"></param>
        /// <param name="matrix">Binary matrix for embedding</param>
        /// <param name="bitsLength">Number of embedded bits</param>
        /// <returns>Bitmap image with embedded information</returns>
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

        /// <summary>
        /// Method for embedding text in image
        /// </summary>
        /// <param name="image">>Bitmap image to embed information in</param>
        /// <param name="wordLength">The length of the code words</param>
        /// <param name="significantBitsLength">Number of significant bits</param>
        /// <param name="areaEmbedding"></param>
        /// <param name="matrix">Binary matrix for embedding</param>
        /// <param name="text">Embedded text</param>
        /// <returns>Bitmap image with embedded information</returns>
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

        /// <summary>
        /// Method for embedding image in image
        /// </summary>
        /// <param name="image">Bitmap image to embed information in</param>
        /// <param name="wordLength">The length of the code words</param>
        /// <param name="significantBitsLength">Number of significant bits</param>
        /// <param name="areaEmbedding"></param>
        /// <param name="matrix">Binary matrix for embedding</param>
        /// <param name="embeddableImage">Embedded image</param>
        /// <returns>Bitmap image with embedded information</returns>
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

        /// <summary>
        /// Method for extracting image from image
        /// </summary>
        /// <param name="image">Bitmap image to embed information in</param>
        /// <param name="wordLength">The length of the code words</param>
        /// <param name="significantBitsLength">Number of significant bits</param>
        /// <param name="areaEmbedding"></param>
        /// <param name="matrix">Binary matrix for embedding</param>
        /// <param name="widthImage">Width of extracting image</param>
        /// <param name="heightImage">Height of extracting image</param>
        /// <returns>Embedded image(Bitmap)</returns>
        public static Bitmap ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, int widthImage, int heightImage)
        {
            var countBits = widthImage * heightImage * 3 * 8;
            var binImage = Modifier.Extract(image, wordLength, significantBitsLength, areaEmbedding, matrix, countBits);
            return binImage.ToBitmap(widthImage, heightImage);
        }

        /// <summary>
        /// Method for extracting text from image
        /// </summary>
        /// <param name="image">Bitmap image to embed information in</param>
        /// <param name="wordLength">The length of the code words</param>
        /// <param name="significantBitsLength">Number of significant bits</param>
        /// <param name="areaEmbedding"></param>
        /// <param name="matrix">Binary matrix for embedding</param>
        /// <param name="textLength">Number of characters in the extracted text</param>
        /// <returns>Embedded text(string)</returns>
        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix, int textLength)
        {
            var countBits = textLength * 16;
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmbedding, matrix, countBits);
            return binText.ToTextString(textLength);
        }

        /// <summary>
        /// Method for extracting all text from image
        /// </summary>
        /// <param name="image">Bitmap image to embed information in</param>
        /// <param name="wordLength">The length of the code words</param>
        /// <param name="significantBitsLength">Number of significant bits</param>
        /// <param name="areaEmbedding"></param>
        /// <param name="matrix">Binary matrix for embedding</param>
        /// <returns>Embedded text(string)</returns>
        public static string ExtractInformation(this Bitmap image, int wordLength, int significantBitsLength,
            AreaEmbeddingModel areaEmbedding, Matrix matrix)
        {
            var binText = Modifier.Extract(image, wordLength, significantBitsLength, areaEmbedding, matrix, null);
            return binText.ToTextString();
        }
    }
}
