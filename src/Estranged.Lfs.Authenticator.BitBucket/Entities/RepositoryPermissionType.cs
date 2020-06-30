using System.Runtime.Serialization;

namespace Estranged.Lfs.Authenticator.BitBucket.Entities
{
    [DataContract]
    public enum RepositoryPermissionType
    {
        [EnumMember(Value = "admin")]
        Admin,
        [EnumMember(Value = "write")]
        Write,
        [EnumMember(Value = "read")]
        Read
    }
}
