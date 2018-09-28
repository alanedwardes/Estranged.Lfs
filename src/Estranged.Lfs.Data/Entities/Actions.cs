using System.Runtime.Serialization;

namespace Estranged.Lfs.Data.Entities
{
    [DataContract]
    public class Actions
    {
        [DataMember(Name = "upload", EmitDefaultValue = false)]
        public Action Upload { get; set; }
        [DataMember(Name = "download", EmitDefaultValue = false)]
        public Action Download { get; set; }
    }
}
