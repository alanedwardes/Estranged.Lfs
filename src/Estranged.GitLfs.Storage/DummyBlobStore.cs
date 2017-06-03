using System;
using System.Threading.Tasks;

namespace Estranged.GitLfs.Storage
{
    public class DummyBlobStore : IBlobStore
    {
        public Task<SignedBlob> UriForDownload(string Oid)
        {
            return Task.FromResult(new SignedBlob());
        }

        public Task<SignedBlob> UriForUpload(string Oid, long size)
        {
            return Task.FromResult(new SignedBlob());
        }
    }
}
