using Newtonsoft.Json;

namespace Estranged.Lfs.Api.Entities
{
    public class Actions
    {
        [JsonProperty("upload", NullValueHandling = NullValueHandling.Ignore)]
        public Action Upload { get; set; }
        [JsonProperty("download", NullValueHandling = NullValueHandling.Ignore)]
        public Action Download { get; set; }
    }
}
