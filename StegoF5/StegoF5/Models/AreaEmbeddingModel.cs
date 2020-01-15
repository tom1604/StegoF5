using System.Collections;
using System.Collections.Generic;

namespace StegoF5.Models
{
    public class AreaEmbeddingModel
    {
        public Dictionary<string, BitArray> SignificantBits { get; }

        public Dictionary<string, BitArray> InSignificantBits { get; }

        public AreaEmbeddingModel(byte redSignificantBits, byte greenSignificantBits, byte blueSignificantBits, byte redInSignificantBits, byte greenInSignificantBits, byte blueInSignificantBits)
        {
            SignificantBits = new Dictionary<string, BitArray>();
            InSignificantBits = new Dictionary<string, BitArray>();

            SignificantBits.Add("R", new BitArray(redSignificantBits));
            SignificantBits.Add("G", new BitArray(greenSignificantBits));
            SignificantBits.Add("B", new BitArray(blueSignificantBits));

            InSignificantBits.Add("R", new BitArray(redInSignificantBits));
            InSignificantBits.Add("G", new BitArray(greenInSignificantBits));
            InSignificantBits.Add("B", new BitArray(blueInSignificantBits));
        }
    }
}
