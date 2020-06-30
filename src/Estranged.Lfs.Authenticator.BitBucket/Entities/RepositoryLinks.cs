using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class RepositoryLinks
    {
        [DataMember(Name = "watchers")]
        public Link Watchers { get; set; }

        [DataMember(Name = "branches")]
        public Link Branches { get; set; }

        [DataMember(Name = "tags")]
        public Link Tags { get; set; }

        [DataMember(Name = "commits")]
        public Link Commits { get; set; }

        [DataMember(Name = "clone")]
        public ICollection<Clone> Clone { get; set; }

        [DataMember(Name = "self")]
        public Link Self { get; set; }

        [DataMember(Name = "source")]
        public Link Source { get; set; }

        [DataMember(Name = "html")]
        public Link Html { get; set; }

        [DataMember(Name = "avatar")]
        public Link Avatar { get; set; }

        [DataMember(Name = "hooks")]
        public Link Hooks { get; set; }

        [DataMember(Name = "forks")]
        public Link Forks { get; set; }

        [DataMember(Name = "downloads")]
        public Link Downloads { get; set; }

        [DataMember(Name = "pullrequests")]
        public Link Pullrequests { get; set; }
    }
}