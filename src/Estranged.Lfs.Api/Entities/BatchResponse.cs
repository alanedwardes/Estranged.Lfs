using Estranged.Lfs.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Estranged.Lfs.Api.Entities
{
    [DataContract]
    public class BatchResponse
    {
        [DataMember(Name = "transfer")]
        public string Transfer { get; set; } = "basic";
        [DataMember(Name = "objects")]
        public IEnumerable<ResponseObject> Objects { get; set; } = Enumerable.Empty<ResponseObject>();
    }
}
