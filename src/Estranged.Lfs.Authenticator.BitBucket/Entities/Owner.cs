using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class Owner
    {
        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "display_name")]
        public string Display_name { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }

        [DataMember(Name = "links")]
        public Links Links { get; set; }
    }
}