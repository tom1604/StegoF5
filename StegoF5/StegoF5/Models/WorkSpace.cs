using System;
using System.Text;

namespace StegoF5.Models
{
    internal class WorkSpace
    {
        internal string Significantbits { get; set; }

        internal string Insignificantbits { get; set; }

        internal WorkSpace()
        {
            Significantbits = string.Empty;
            Insignificantbits = string.Empty;
        }
    }
}
