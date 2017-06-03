using System;
using System.Threading.Tasks;

namespace Estranged.GitLfs.Storage
{
    public class DummyBlobStore : IBlobStore
    {
        public Task<Uri> UriForDownload(string Oid)
        {
            return Task.FromResult(new Uri("https://www.example.com/"));
        }

        public Task<Uri> UriForUpload(string Oid, long size)
        {
            return Task.FromResult(new Uri("https://www.example.com/"));
        }
    }
}
