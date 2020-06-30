using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class Workspace
    {
        [DataMember(Name = "slug")]
        public string Slug { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "links")]
        public Links Links { get; set; }

        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }
    }
}