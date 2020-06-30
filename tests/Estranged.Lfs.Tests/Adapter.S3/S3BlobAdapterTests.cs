using Amazon.S3;
using Estranged.Lfs.Adapter.S3;
using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Estranged.Lfs.Tests.Adapter.S3
{
    public class S3BlobAdapterTests
    {
        private IBlobAdapter CreateAdapter()
        {
            var config = new S3BlobAdapterConfig
            {
                Bucket = ConfigurationManager.GetS3BucketName()
            };
            return new ServiceCollection().AddLfsS3Adapter(config, new AmazonS3Client()).BuildServiceProvider().GetRequiredService<IBlobAdapter>();
        }

        [Fact]
        public async Task TestDownloadBlobFound()
        {
            var adapter = CreateAdapter();

            var signedBlob = await adapter.UriForDownload("d53de494a038b6a8ede0aea08c38bde00244b155924bf4c463d1de208faecee8", CancellationToken.None);

            Assert.Null(signedBlob.ErrorCode);
            Assert.Null(signedBlob.ErrorMessage);
            Assert.Equal(19961, signedBlob.Size);
            Assert.NotNull(signedBlob.Uri);
        }

        [Fact]
        public async Task TestDownloadBlobNotFound()
        {
            var adapter = CreateAdapter();

            var signedBlob = await adapter.UriForDownload("wibble", CancellationToken.None);

            Assert.Equal(404, signedBlob.ErrorCode);
            Assert.Equal("Error making request with Error Code NotFound and Http Status Code NotFound. No further error information was returned by the service.", signedBlob.ErrorMessage);
        }

        [Fact]
        public async Task TestUploadBlob()
        {
            var adapter = CreateAdapter();

            var signedBlob = await adapter.UriForUpload("wibble", 10, CancellationToken.None);

            Assert.Null(signedBlob.ErrorCode);
            Assert.Null(signedBlob.ErrorMessage);
            Assert.NotNull(signedBlob.Uri);
        }
    }
}
