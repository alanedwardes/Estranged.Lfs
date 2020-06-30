using Estranged.Lfs.Authenticator.BitBucket.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    internal sealed class BitBucketClient : IBitBucketClient
    {
        private readonly HttpClient httpClient;

        public BitBucketClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<RepositoryPermissions> GetRepositoryPermissions(string workspace, string repository, CancellationToken token)
        {
            var response = await httpClient.GetAsync($"/2.0/user/permissions/repositories?q=repository.full_name=\"{HttpUtility.UrlEncode(workspace)}/{HttpUtility.UrlEncode(repository)}\"", token);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<RepositoryPermissions>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Repository> GetRepository(string workspace, string repository, CancellationToken token)
        {
            var response = await httpClient.GetAsync($"/2.0/repositories/{HttpUtility.UrlEncode(workspace)}/{HttpUtility.UrlEncode(repository)}", token);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Repository>(await response.Content.ReadAsStringAsync());
        }
    }
}
