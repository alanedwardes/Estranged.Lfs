using Newtonsoft.Json;

namespace Estranged.Lfs.Api.Entities
{
    public class ResponseObject
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
        [JsonProperty("authenticated")]
        public bool? Authenticated { get; set; }
        [JsonProperty("actions")]
        public Actions Actions { get; set; } = new Actions();
    }
}
