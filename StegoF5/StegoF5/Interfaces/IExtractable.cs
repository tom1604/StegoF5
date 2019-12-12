using System.Drawing;
using StegoF5.Models;

namespace StegoF5.Interfaces
{
    internal interface IExtractable
    {
        string Extract(Bitmap image, int wordLength, int significantBitsLength, AreaEmbeddingModel areaEmdedding, byte[,] matrix, int? countBits);
    }
}