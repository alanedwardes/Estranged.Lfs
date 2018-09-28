using Estranged.Lfs.Data.Entities;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Estranged.Lfs.Api.Entities
{
    [DataContract]
    public class BatchRequest
    {
        [DataMember(Name = "operation")]
        public LfsOperation Operation { get; set; }
        [DataMember(Name = "transfers")]
        public IList<string> Transfers { get; set; } = new List<string> { "basic" };
        [DataMember(Name = "objects")]
        public IList<RequestObject> Objects { get; set; }
    }
}
