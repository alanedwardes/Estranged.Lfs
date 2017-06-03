using Microsoft.Net.Http.Headers;

namespace Estranged.GitLfs.Api
{
    public static class GitLfsConstants
    {
        public static MediaTypeHeaderValue GitLfsMediaType => new MediaTypeHeaderValue("application/vnd.git-lfs+json");
    }
}
