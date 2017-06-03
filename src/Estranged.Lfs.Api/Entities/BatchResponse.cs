using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Estranged.Lfs.Api.Entities
{
    public class BatchResponse
    {
        [JsonProperty("transfer")]
        public string Transfer { get; set; } = "basic";
        [JsonProperty("objects")]
        public IEnumerable<ResponseObject> Objects { get; set; } = Enumerable.Empty<ResponseObject>();
    }
}
