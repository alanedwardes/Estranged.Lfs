using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class User
    {
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }

        [DataMember(Name = "links")]
        public Links Links { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "account_id")]
        public string AccountId { get; set; }
    }
}
