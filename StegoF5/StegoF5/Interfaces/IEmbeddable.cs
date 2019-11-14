using System.Collections.Generic;
using System.Drawing;

namespace StegoF5.Interfaces
{
    internal interface IEmbeddable
    {
        Bitmap Embed(Bitmap image, int wordLength, int significantBitsLength, Dictionary<string, bool[]>[] areaEmdedding, byte[,] matrix, string binInformation);
    }
}
