using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace Estranged.GitLfs.Storage
{
    public interface IS3BlobStoreConfig
    {
        string Bucket { get; }
        string KeyPrefix { get; }
        Protocol Protocol { get; }
        TimeSpan Expiry { get; }
    }

    public class S3BlobStoreConfig : IS3BlobStoreConfig
    {
        public string Bucket { get; set; }
        public string KeyPrefix { get; set; }
        public Protocol Protocol { get; set; } = Protocol.HTTPS;
        public TimeSpan Expiry { get; set; } = TimeSpan.FromHours(1);
    }

    public class S3BlobStore : IBlobStore
    {
        private readonly IAmazonS3 client;
        private readonly IS3BlobStoreConfig config;

        public S3BlobStore(IAmazonS3 client, IS3BlobStoreConfig config)
        {
            this.client = client;
            this.config = config;
        }

        public GetPreSignedUrlRequest GenerateRequest(string Oid, HttpVerb verb) => new GetPreSignedUrlRequest
        {
            Verb = verb,
            BucketName = config.Bucket,
            Key = config.KeyPrefix + Oid,
            Protocol = config.Protocol,
            ContentType = "application/octet-stream",
            Expires = DateTime.UtcNow + config.Expiry
        };

        public Task<SignedBlob> UriForDownload(string Oid)
        {
            GetPreSignedUrlRequest request = GenerateRequest(Oid, HttpVerb.GET);
            string signed = client.GetPreSignedURL(request);

            return Task.FromResult(new SignedBlob
            {
                Uri = new Uri(signed),
                Expiry = config.Expiry
            });
        }

        public Task<SignedBlob> UriForUpload(string Oid, long size)
        {
            GetPreSignedUrlRequest request = GenerateRequest(Oid, HttpVerb.PUT);
            string signed = client.GetPreSignedURL(request);

            return Task.FromResult(new SignedBlob
            {
                Uri = new Uri(signed),
                Expiry = config.Expiry
            });
        }
    }
}
