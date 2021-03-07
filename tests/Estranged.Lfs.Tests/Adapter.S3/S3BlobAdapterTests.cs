using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Estranged.Lfs.Adapter.S3;
using Moq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Estranged.Lfs.Tests.Adapter.S3
{
    public class S3BlobAdapterTests : IDisposable
    {
        private readonly MockRepository mockRepository = new MockRepository(MockBehavior.Strict);

        public void Dispose() => mockRepository.VerifyAll();

        [Fact]
        public async Task TestDownloadBlobFound()
        {
            var mockClient = mockRepository.Create<IAmazonS3>();

            mockClient.Setup(x => x.GetObjectMetadataAsync("my-bucket", "prefix/d53de494a038b6a8ede0aea08c38bde00244b155924bf4c463d1de208faecee8", CancellationToken.None))
                      .ReturnsAsync(new GetObjectMetadataResponse{ContentLength = 19961});

            mockClient.Setup(x => x.GetPreSignedURL(It.IsAny<GetPreSignedUrlRequest>()))
                      .Callback<GetPreSignedUrlRequest>(request =>
                      {
                          Assert.Equal("prefix/d53de494a038b6a8ede0aea08c38bde00244b155924bf4c463d1de208faecee8", request.Key);
                          Assert.Equal("my-bucket", request.BucketName);
                          Assert.Equal(HttpVerb.GET, request.Verb);
                          Assert.Equal(Protocol.HTTPS, request.Protocol);
                      })
                      .Returns("https://www.example.com/");

            var adapter = new S3BlobAdapter(mockClient.Object, new S3BlobAdapterConfig { Bucket = "my-bucket", KeyPrefix = "prefix/" });

            var signedBlob = await adapter.UriForDownload("d53de494a038b6a8ede0aea08c38bde00244b155924bf4c463d1de208faecee8", CancellationToken.None);

            Assert.Null(signedBlob.ErrorCode);
            Assert.Null(signedBlob.ErrorMessage);
            Assert.Equal(19961, signedBlob.Size);
            Assert.Equal("https://www.example.com/", signedBlob.Uri.ToString());
        }

        [Fact]
        public async Task TestDownloadBlobNotFound()
        {
            var mockClient = mockRepository.Create<IAmazonS3>();

            mockClient.Setup(x => x.GetObjectMetadataAsync("my-bucket", "prefix/wibble", CancellationToken.None))
                      .ThrowsAsync(new AmazonS3Exception("Error making request with Error Code NotFound and Http Status Code NotFound. No further error information was returned by the service.", ErrorType.Sender, "NotFound", "requestId", HttpStatusCode.NotFound));

            var adapter = new S3BlobAdapter(mockClient.Object, new S3BlobAdapterConfig { Bucket = "my-bucket", KeyPrefix = "prefix/" });

            var signedBlob = await adapter.UriForDownload("wibble", CancellationToken.None);

            Assert.Equal(404, signedBlob.ErrorCode);
            Assert.Equal("Error making request with Error Code NotFound and Http Status Code NotFound. No further error information was returned by the service.", signedBlob.ErrorMessage);
        }

        [Fact]
        public async Task TestUploadBlob()
        {
            var mockClient = mockRepository.Create<IAmazonS3>();

            mockClient.Setup(x => x.GetPreSignedURL(It.IsAny<GetPreSignedUrlRequest>()))
                      .Callback<GetPreSignedUrlRequest>(request =>
                      {
                          Assert.Equal("prefix/wibble", request.Key);
                          Assert.Equal("my-bucket", request.BucketName);
                          Assert.Equal(HttpVerb.PUT, request.Verb);
                          Assert.Equal(Protocol.HTTPS, request.Protocol);
                      })
                      .Returns("https://www.example.com/");

            var adapter = new S3BlobAdapter(mockClient.Object, new S3BlobAdapterConfig { Bucket = "my-bucket", KeyPrefix = "prefix/" });

            var signedBlob = await adapter.UriForUpload("wibble", 10, CancellationToken.None);

            Assert.Null(signedBlob.ErrorCode);
            Assert.Null(signedBlob.ErrorMessage);
            Assert.Equal("https://www.example.com/", signedBlob.Uri.ToString());
        }
    }
}
