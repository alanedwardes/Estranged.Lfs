using System;

namespace Estranged.Lfs.Data
{
    public class SignedBlob
    {
        public Uri Uri { get; set; }
        public TimeSpan Expiry { get; set; }
    }
}
