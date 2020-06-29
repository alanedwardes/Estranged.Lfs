using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class Link
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }
    }
}
