using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace Estranged.GitLfs.Storage
{
    public class S3BlobStore : IBlobStore
    {
        private readonly IAmazonS3 client;

        public S3BlobStore(IAmazonS3 client)
        {
            this.client = client;
        }

        public Task<Uri> UriForDownload(string Oid)
        {
            throw new NotImplementedException();
        }

        public Task<Uri> UriForUpload(string Oid, long size)
        {
            client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                
            });
            throw new NotImplementedException();
        }
    }
}
