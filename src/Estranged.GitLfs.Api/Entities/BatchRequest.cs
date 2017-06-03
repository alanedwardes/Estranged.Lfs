using Narochno.Primitives.Parsing;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Estranged.GitLfs.Api.Entities
{
    public enum LfsOperation
    {
        [EnumString("verify")]
        Verify,
        [EnumString("upload")]
        Upload,
        [EnumString("download")]
        Download
    }

    public class BatchRequest
    {
        [JsonProperty("operation")]
        public LfsOperation Operation { get; set; }
        [JsonProperty("transfers")]
        public IList<string> Transfers { get; set; } = new List<string> { "basic" };
        [JsonProperty("objects")]
        public IList<RequestObject> Objects { get; set; }
    }

    public class RequestObject
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
