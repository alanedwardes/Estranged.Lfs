using Estranged.Lfs.Data.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Estranged.Lfs.Api.Entities
{
    public class BatchRequest
    {
        [JsonProperty("operation")]
        public LfsOperation Operation { get; set; }
        [JsonProperty("transfers")]
        public IList<string> Transfers { get; set; } = new List<string> { "basic" };
        [JsonProperty("objects")]
        public IList<RequestObject> Objects { get; set; }
    }
}
