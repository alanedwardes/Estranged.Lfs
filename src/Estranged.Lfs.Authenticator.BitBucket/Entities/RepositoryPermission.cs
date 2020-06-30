using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public sealed class RepositoryPermission
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        [DataMember(Name = "repository")]
        public RepositorySummary Repository { get; set; }

        [DataMember(Name = "permission")]
        public RepositoryPermissionType Permission { get; set; }
    }
}
