using Microsoft.Net.Http.Headers;

namespace Estranged.Lfs.Api
{
    public static class LfsConstants
    {
        public static MediaTypeHeaderValue LfsMediaType => new MediaTypeHeaderValue("application/vnd.git-lfs+json");
    }
}
