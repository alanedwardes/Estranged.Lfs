using System;
using System.Collections.Generic;

namespace Estranged.Lfs.Data
{
    public class SignedBlob
    {
        public Uri Uri { get; set; }
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public TimeSpan Expiry { get; set; }
    }
}
