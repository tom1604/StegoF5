using System.Drawing;
using StegoF5.Extensions;
using StegoF5.Models;

namespace StegoF5.Interfaces
{
    internal interface IEmbeddable
    {
        Bitmap Embed(Bitmap image, int wordLength, int significantBitsLength, AreaEmbeddingModel areaEmbedding, Matrix matrix, string binInformation);
    }
}
