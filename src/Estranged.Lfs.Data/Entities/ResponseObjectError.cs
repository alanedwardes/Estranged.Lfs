using System.Runtime.Serialization;

namespace Estranged.Lfs.Data.Entities
{
    [DataContract]
    public sealed class ResponseObjectError
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
