using Narochno.Primitives.Parsing;

namespace Estranged.Lfs.Api.Entities
{
    public enum LfsOperation
    {
        [EnumString("verify")]
        Verify,
        [EnumString("upload")]
        Upload,
        [EnumString("download")]
        Download
    }
}
