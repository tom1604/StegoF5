using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoF5
{
    internal interface IEmbeddable
    {
        Bitmap Embed(Bitmap image, int wordLength, int significantBitsLength, Dictionary<string, bool[]>[] areaEmdedding, string embeddedBits);
    }
}
