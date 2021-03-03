using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Estranged.Lfs.Adapter.Azure.Blob;
using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Estranged.Lfs.Tests.Adapter.AzureBlob
{
    public class AzureBlobAdapterTests
    {
        private IBlobAdapter CreateAdapter()
        {
            var connectionString = ConfigurationManager.GetAzureStorageAccountConnectionString();
            var blobServiceClient = new BlobServiceClient(connectionString);
            var config = new AzureBlobAdapterConfig
            {
                ContainerName = nameof(AzureBlobAdapterTests).ToLowerInvariant(),
            };

            return new ServiceCollection().AddLfsAzureBlobAdapter(config, blobServiceClient)
                .BuildServiceProvider()
                .GetRequiredService<IBlobAdapter>();
        }

        [Fact]
        public async Task TestDownloadBlobNotFound()
        {
            var adapter = CreateAdapter();
            var blob = await adapter.UriForDownload("wibble", CancellationToken.None);

            Assert.Equal(404, blob.ErrorCode);
            Assert.Contains("Status: 404 (The specified blob does not exist.)", blob.ErrorMessage);
        }

        [Fact]
        public async Task TestUploadBlob()
        {
            var adapter = CreateAdapter();
            var blob = await adapter.UriForUpload("wibble", 10, CancellationToken.None);

            Assert.Null(blob.ErrorCode);
            Assert.Null(blob.ErrorMessage);
            Assert.NotNull(blob.Uri);
        }
    }
}