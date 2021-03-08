using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Estranged.Lfs.Adapter.Azure.Blob;
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
            var blobProperties = mockRepository.Create<BlobProperties>();

            blobContainerClient.Setup(x => x.GetBlobClient("Prefix/wibble"))
                .Returns(blobClient.Object);

            blobClient.Setup(x => x.GenerateSasUri(BlobSasPermissions.Read, It.IsAny<DateTimeOffset>()))
                .Returns(new Uri("https://www.example.com/"));

            blobClient.Setup(x => x.GetPropertiesAsync(null, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(Response.FromValue(blobProperties.Object, null));

            var adapter = new AzureBlobAdapter(blobContainerClient.Object, new AzureBlobAdapterConfig { ContainerName = "gitlfs", KeyPrefix = "Prefix/" });

            var blob = await adapter.UriForDownload("wibble", CancellationToken.None);

            Assert.Null(blob.ErrorCode);
            Assert.Null(blob.ErrorMessage);
            Assert.Equal("https://www.example.com/", blob.Uri.ToString());
        }

        [Fact]
        public async Task TestDownloadBlobNotFound()
        {
            var blobContainerClient = mockRepository.Create<BlobContainerClient>();

            blobContainerClient.Setup(x => x.GetBlobClient("Prefix/non-existing"))
                .Throws(new RequestFailedException(404, "Status: 404 (The specified blob does not exist.)"));

            var adapter = new AzureBlobAdapter(blobContainerClient.Object, new AzureBlobAdapterConfig
            {
                ContainerName = "gitlfs",
                KeyPrefix = "Prefix/"
            });

            var blob = await adapter.UriForDownload("non-existing", CancellationToken.None);

            Assert.Equal(404, blob.ErrorCode);
            Assert.Contains("Status: 404 (The specified blob does not exist.)", blob.ErrorMessage);
        }

        [Fact]
        public async Task TestUploadBlob()
        {
            var blobContainerClient = mockRepository.Create<BlobContainerClient>();
            var blobClient = mockRepository.Create<BlobClient>();
            var blobProperties = mockRepository.Create<BlobProperties>();

            blobContainerClient.Setup(x => x.GetBlobClient("Prefix/wibble"))
                .Returns(blobClient.Object);

            blobClient.Setup(x => x.GenerateSasUri(BlobSasPermissions.Write, It.IsAny<DateTimeOffset>()))
                .Returns(new Uri("https://www.example.com/"));

            var adapter = new AzureBlobAdapter(blobContainerClient.Object, new AzureBlobAdapterConfig
            {
                ContainerName = "gitlfs",
                KeyPrefix = "Prefix/"
            });

            var blob = await adapter.UriForUpload("wibble", 10, CancellationToken.None);

            Assert.Null(blob.ErrorCode);
            Assert.Null(blob.ErrorMessage);
            Assert.Equal("https://www.example.com/", blob.Uri.ToString());
            Assert.Contains("x-ms-blob-type", blob.Headers);
        }
    }
}