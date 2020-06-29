using Estranged.Lfs.Authenticator.BitBucket.Entities;
using System.Net.Http;
using System.Runtime.Serialization.Json;
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

        public async Task<RepositoryPermissions> GetRepositoryPermissions(string repositoryFullName, CancellationToken token)
        {
            var response = await httpClient.GetAsync($"/api.bitbucket.org/2.0/user/permissions/repositories?q=repository.full_name=\"{HttpUtility.UrlEncode(repositoryFullName)}\"", token);
            response.EnsureSuccessStatusCode();

            var seraliser = new DataContractJsonSerializer(typeof(RepositoryPermissions));

            return (RepositoryPermissions)seraliser.ReadObject(await response.Content.ReadAsStreamAsync());
        }
    }
}
