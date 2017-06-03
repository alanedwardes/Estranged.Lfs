using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace Estranged.Lfs.Api.Entities
{
    public class Action
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }
        [JsonProperty("header")]
        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
    }
}
