using System.Runtime.Serialization;

namespace Estranged.Lfs.Data.Entities
{
    [DataContract]
    public class RequestObject
    {
        [DataMember(Name = "oid")]
        public string Oid { get; set; }
        [DataMember(Name = "size")]
        public long Size { get; set; }
    }
}
