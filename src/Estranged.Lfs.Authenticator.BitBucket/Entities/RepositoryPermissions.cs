using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class RepositoryPermissions
    {
        [DataMember(Name = "pagelen")]
        public int Pagelen { get; set; }

        [DataMember(Name = "values")]
        public ICollection<RepositoryPermission> Values { get; set; }

        [DataMember(Name = "page")]
        public int Page { get; set; }
    }
}
