using System.Collections.Generic;
using System.Drawing;

namespace StegoF5
{
    internal class F5Embedding : BaseF5, IEmbeddable
    {
        public Bitmap Embed(Bitmap image, int wordLength, int significantBitsLength,
            Dictionary<string, bool[]>[] areaEmdedding, string embeddedBits)
        {
            return new Bitmap(0,0);
        }
    }
}
