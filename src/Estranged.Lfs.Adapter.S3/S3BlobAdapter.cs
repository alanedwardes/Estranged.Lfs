using Amazon.S3;
using Amazon.S3.Model;
using Estranged.Lfs.Data;
using System;
using System.Threading.Tasks;

namespace Estranged.Lfs.Adapter.S3
{
    public class S3BlobAdapter : IBlobAdapter
    {
        private readonly IAmazonS3 client;
        private readonly IS3BlobAdapterConfig config;

        public S3BlobAdapter(IAmazonS3 client, IS3BlobAdapterConfig config)
        {
            this.client = client;
            this.config = config;
        }

        public GetPreSignedUrlRequest MakePreSignedUrl(string Oid, HttpVerb verb, string mimeType) => new GetPreSignedUrlRequest
        {
            Verb = verb,
            BucketName = config.Bucket,
            Key = config.KeyPrefix + Oid,
            Protocol = config.Protocol,
            ContentType = mimeType,
            Expires = DateTime.UtcNow + config.Expiry
        };

        public Task<SignedBlob> UriForDownload(string Oid)
        {
            GetPreSignedUrlRequest request = MakePreSignedUrl(Oid, HttpVerb.GET, null);
            string signed = client.GetPreSignedURL(request);

            var builder = new UriBuilder(signed);
            if (!string.IsNullOrWhiteSpace(config.AccessHost))
            {
                builder.Host = config.AccessHost;
            }

            return Task.FromResult(new SignedBlob
            {
                Uri = builder.Uri,
                Expiry = config.Expiry
            });
        }

        public Task<SignedBlob> UriForUpload(string Oid, long size)
        {
            GetPreSignedUrlRequest request = MakePreSignedUrl(Oid, HttpVerb.PUT, BlobConstants.UploadMimeType);
            string signed = client.GetPreSignedURL(request);

            var builder = new UriBuilder(signed);
            if (!string.IsNullOrWhiteSpace(config.AccessHost))
            {
                builder.Host = config.AccessHost;
            }

            return Task.FromResult(new SignedBlob
            {
                Uri = builder.Uri,
                Expiry = config.Expiry
            });
        }
    }
}
