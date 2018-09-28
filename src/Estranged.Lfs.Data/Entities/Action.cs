using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Estranged.Lfs.Data.Entities
{
    [DataContract]
    public class Action
    {
        [DataMember(Name = "href")]
        public Uri Href { get; set; }
        [DataMember(Name = "header")]
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        [DataMember(Name = "expires_in")]
        public long ExpiresIn { get; set; }
    }
}
