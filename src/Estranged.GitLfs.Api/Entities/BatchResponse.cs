using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Estranged.GitLfs.Api.Entities
{
    public class BatchResponse
    {
        [JsonProperty("transfer")]
        public string Transfer { get; set; } = "basic";
        [JsonProperty("objects")]
        public IEnumerable<ResponseObject> Objects { get; set; } = Enumerable.Empty<ResponseObject>();
    }

    public class ResponseObject
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
        [JsonProperty("authenticated")]
        public bool? Authenticated { get; set; }
        [JsonProperty("actions")]
        public IDictionary<LfsOperation, Action> Actions { get; set; }
    }

    public class Action
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }
        [JsonProperty("header")]
        public IHeaderDictionary Headers { get; set; } = new HeaderDictionary();
        [JsonProperty("expires_in")]
        public long? ExpiresIn { get; set; }
        [JsonProperty("expires_at")]
        public DateTime? ExpiresAt { get; set; }
    }
}
