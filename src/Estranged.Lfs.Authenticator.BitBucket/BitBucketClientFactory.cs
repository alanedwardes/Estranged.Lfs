using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    internal sealed class BitBucketClientFactory : IBitBucketClientFactory
    {
        public IBitBucketClient CreateClient(Uri baseAddress, string username, string password)
        {
            var client = new HttpClient { BaseAddress = baseAddress };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
            return new BitBucketClient(client);
        }
    }
}
