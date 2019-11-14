using System.Collections.Generic;
using System.Drawing;

namespace StegoF5.Interfaces
{
    internal interface IExtractable
    {
        string Extract(Bitmap image, int wordLength, int significantBitsLength, Dictionary<string, bool[]>[] areaEmdedding, byte[,] matrix, int? countBits);
    }
}