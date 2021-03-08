using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Estranged.Lfs.Adapter.Azure.Blob;
using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Estranged.Lfs.Tests.Adapter.AzureBlob
{
    public class AzureBlobAdapterTests : IDisposable
    {
        private readonly MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

        public void Dispose() => mockRepository.VerifyAll();

        [Fact]
        public async Task TestDownloadBlobFound()
        {
            var blobContainerClient = mockRepository.Create<BlobContainerClient>();
            var blobClient = mockRepository.Create<BlobClient>();

            blobContainerClient.Setup(x => x.GetBlobClient("Prefix/wibble"))
                .Returns(blobClient.Object);

            blobClient.Setup(x => x.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, It.IsAny<DateTimeOffset>()))
                .Returns(new Uri("https://www.example.com/"));

            blobClient.Setup(x => x.GetPropertiesAsync(null, It.IsAny<CancellationToken>()))
                .ReturnsAsync();

            var adapter = new AzureBlobAdapter(blobContainerClient.Object, new AzureBlobAdapterConfig { ContainerName = "gitlfs", KeyPrefix = "Prefix/" });

            var blob = await adapter.UriForDownload("wibble", CancellationToken.None);

            Assert.Equal(404, blob.ErrorCode);
            Assert.Contains("Status: 404 (The specified blob does not exist.)", blob.ErrorMessage);
        }

        [Fact]
        public Task TestUploadBlob()
        {
            return Task.CompletedTask;
        }
    }
}