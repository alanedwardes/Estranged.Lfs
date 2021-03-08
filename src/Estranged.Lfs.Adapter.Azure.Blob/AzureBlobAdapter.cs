using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Estranged.Lfs.Data;

namespace Estranged.Lfs.Adapter.Azure.Blob
{
    internal sealed class AzureBlobAdapter : IBlobAdapter
    {
        private readonly BlobContainerClient _client;
        private readonly IAzureBlobAdapterConfig _config;

        public AzureBlobAdapter(BlobContainerClient client, IAzureBlobAdapterConfig config)
        {
            _client = client;
            _config = config;
        }

        private Uri MakePreAuthenticatedUrl(string oid, BlobSasPermissions permissions)
        {
            var blob = _client.GetBlobClient(GetBlobName(oid));
            return blob.GenerateSasUri(permissions, GetExpiry());
        }

        private string GetBlobName(string oid)
          => $"{_config.KeyPrefix}{oid}";

        private DateTimeOffset GetExpiry()
          => DateTimeOffset.UtcNow.Add(_config.Expiry);

        public Task<SignedBlob> UriForUpload(string Oid, long size, CancellationToken token)
        {
            var blob = new SignedBlob
            {
                Uri = MakePreAuthenticatedUrl(Oid, BlobSasPermissions.Write),
                Expiry = _config.Expiry,
                Headers = new Dictionary<string, string>
                {
                    ["Content-Type"] = BlobConstants.UploadMimeType,
                    ["x-ms-blob-type"] = "BlockBlob"
                }
            };

            return Task.FromResult(blob);
        }

        public async Task<SignedBlob> UriForDownload(string Oid, CancellationToken token)
        {
            try
            {
                var blob = _client.GetBlobClient(GetBlobName(Oid));

                return new SignedBlob
                {
                    Uri = MakePreAuthenticatedUrl(Oid, BlobSasPermissions.Read),
                    Expiry = _config.Expiry,
                    Size = (await blob.GetPropertiesAsync(null, token)).Value.ContentLength
                };
            }
            catch (RequestFailedException ex)
            {
                return new SignedBlob
                {
                    ErrorCode = ex.Status,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
