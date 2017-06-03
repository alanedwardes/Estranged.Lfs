using Newtonsoft.Json;
using System.Collections.Generic;

namespace Estranged.GitLfs.Api.Entities
{
    public enum BatchOperation
    {
        Unknown,
        Upload
    }

    public class BatchRequest
    {
        [JsonProperty("operation")]
        public BatchOperation Operation { get; set; }
        [JsonProperty("objects")]
        public IList<Object> Objects { get; set; }
    }

    public class Object
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
