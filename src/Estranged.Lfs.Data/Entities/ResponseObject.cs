using System.Runtime.Serialization;

namespace Estranged.Lfs.Data.Entities
{
    [DataContract]
    public class ResponseObject
    {
        [DataMember(Name = "oid")]
        public string Oid { get; set; }
        [DataMember(Name = "size")]
        public long Size { get; set; }
        [DataMember(Name = "authenticated")]
        public bool? Authenticated { get; set; }
        [DataMember(Name = "actions")]
        public Actions Actions { get; set; } = new Actions();
    }
}
