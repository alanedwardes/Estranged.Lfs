using System.Runtime.Serialization;

namespace Estranged.Lfs.Api.Entities
{
    public enum LfsOperation
    {
        [EnumMember]
        Verify,
        [EnumMember]
        Upload,
        [EnumMember]
        Download
    }
}
