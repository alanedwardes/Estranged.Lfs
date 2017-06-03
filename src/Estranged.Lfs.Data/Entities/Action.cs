using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Estranged.Lfs.Data.Entities
{
    public class Action
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }
        [JsonProperty("header")]
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
    }
}
