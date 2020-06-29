using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class Links
    {
        [DataMember(Name = "self")]
        public Link Self { get; set; }

        [DataMember(Name = "html")]
        public Link Html { get; set; }

        [DataMember(Name = "avatar")]
        public Link Avatar { get; set; }
    }
}
