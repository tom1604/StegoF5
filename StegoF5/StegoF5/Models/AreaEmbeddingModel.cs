using System.Collections;
using System.Collections.Generic;

namespace StegoF5.Models
{
    public struct AreaEmbeddingModel
    {
        public Dictionary<string, BitArray> SignificantBits { get; set; }

        public Dictionary<string, BitArray> InSignificantBits { get; set; }
    }
}
