using Newtonsoft.Json;

namespace Estranged.Lfs.Data.Entities
{
    public class RequestObject
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
