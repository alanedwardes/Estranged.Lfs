using Amazon.S3;
using Amazon.S3.Model;
using Estranged.Lfs.Data;
using System;
using System.Collections.Generic;
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

        public GetPreSignedUrlRequest MakePreSignedUrl(string oid, HttpVerb verb, string mimeType) => new GetPreSignedUrlRequest
        {
            Verb = verb,
            BucketName = config.Bucket,
            Key = config.KeyPrefix + oid,
            Protocol = config.Protocol,
            ContentType = mimeType,
            Expires = DateTime.UtcNow + config.Expiry
        };

        public Task<SignedBlob> UriForDownload(string oid)
        {
            GetPreSignedUrlRequest request = MakePreSignedUrl(oid, HttpVerb.GET, null);
            string signed = client.GetPreSignedURL(request);

            return Task.FromResult(new SignedBlob
            {
                Uri = new Uri(signed),
                Expiry = config.Expiry
            });
        }

        public Task<SignedBlob> UriForUpload(string oid, long size)
        {
            GetPreSignedUrlRequest request = MakePreSignedUrl(oid, HttpVerb.PUT, BlobConstants.UploadMimeType);
            string signed = client.GetPreSignedURL(request);

            return Task.FromResult(new SignedBlob
            {
                Uri = new Uri(signed),
                Expiry = config.Expiry,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", BlobConstants.UploadMimeType}
                }
            });
        }
    }
}
