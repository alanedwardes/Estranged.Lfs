using Amazon.S3;
using Amazon.S3.Model;
using Estranged.Lfs.Data;
using System;
using System.Threading.Tasks;

namespace Estranged.Lfs.Adapter.S3
{
    public class S3BlobStore : IBlobStore
    {
        private readonly IAmazonS3 client;
        private readonly IS3BlobStoreConfig config;

        public S3BlobStore(IAmazonS3 client, IS3BlobStoreConfig config)
        {
            this.client = client;
            this.config = config;
        }

        public GetPreSignedUrlRequest GenerateRequest(string Oid, HttpVerb verb, string mimeType) => new GetPreSignedUrlRequest
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
            GetPreSignedUrlRequest request = GenerateRequest(Oid, HttpVerb.GET, null);
            string signed = client.GetPreSignedURL(request);

            return Task.FromResult(new SignedBlob
            {
                Uri = new Uri(signed),
                Expiry = config.Expiry
            });
        }

        public Task<SignedBlob> UriForUpload(string Oid, long size)
        {
            GetPreSignedUrlRequest request = GenerateRequest(Oid, HttpVerb.PUT, StorageConstants.UploadMimeType);
            string signed = client.GetPreSignedURL(request);

            return Task.FromResult(new SignedBlob
            {
                Uri = new Uri(signed),
                Expiry = config.Expiry
            });
        }
    }
}
