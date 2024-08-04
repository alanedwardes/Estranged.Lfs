using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.S3;
using Estranged.Lfs.Adapter.Azure.Blob;
using Estranged.Lfs.Adapter.S3;
using Estranged.Lfs.Api;
using Estranged.Lfs.Authenticator.BitBucket;
using Estranged.Lfs.Authenticator.GitHub;
using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Estranged.Lfs.Hosting.Lambda
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            const string LfsBucketVariable = "LFS_BUCKET";
            const string LfsUsernameVariable = "LFS_USERNAME";
            const string LfsPasswordVariable = "LFS_PASSWORD";
            const string GitHubOrganisationVariable = "GITHUB_ORGANISATION";
            const string GitHubRepositoryVariable = "GITHUB_REPOSITORY";
            const string BitBucketWorkspaceVariable = "BITBUCKET_WORKSPACE";
            const string BitBucketRepositoryVariable = "BITBUCKET_REPOSITORY";
            const string S3AccelerationVariable = "S3_ACCELERATION";
            const string LfsAzureStorageConnectionStringVariable = "LFS_AZUREBLOB_CONNECTIONSTRING";
            const string LfsAzureStorageContainerNameVariable = "LFS_AZUREBLOB_CONTAINERNAME";
            const string S3ServiceURL = "S3_SERVICE_URL";
            const string S3Region = "S3_REGION";
            const string S3AccessKey = "S3_ACCESS_KEY";
            const string S3AccessSecret = "S3_ACCESS_SECRET";

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            string lfsBucket = config[LfsBucketVariable];
            string lfsAzureStorageConnectionString = config[LfsAzureStorageConnectionStringVariable];
            string lfsUsername = config[LfsUsernameVariable];
            string lfsPassword = config[LfsPasswordVariable];
            string gitHubOrganisation = config[GitHubOrganisationVariable];
            string gitHubRepository = config[GitHubRepositoryVariable];
            string bitBucketWorkspace = config[BitBucketWorkspaceVariable];
            string bitBucketRepository = config[BitBucketRepositoryVariable];
            bool s3Acceleration = bool.Parse(config[S3AccelerationVariable] ?? "false");
            string s3ServiceURL = config[S3ServiceURL];
            string s3Region = config[S3Region];
            string s3AccessKey = config[S3AccessKey];
            string s3AccessSecret = config[S3AccessSecret];

            bool isS3Storage = !string.IsNullOrWhiteSpace(lfsBucket);
            bool isAzureStorage = !string.IsNullOrWhiteSpace(lfsAzureStorageConnectionString);
            bool isDictionaryAuthentication = !string.IsNullOrWhiteSpace(lfsUsername) && !string.IsNullOrWhiteSpace(lfsPassword);
            bool isGitHubAuthentication = !string.IsNullOrWhiteSpace(gitHubOrganisation) && !string.IsNullOrWhiteSpace(gitHubRepository);
            bool isBitBucketAuthentication = !string.IsNullOrWhiteSpace(bitBucketWorkspace) && !string.IsNullOrWhiteSpace(bitBucketRepository);

            // If all authentication mechanims are set, or none are set throw an error
            if (new[] {isDictionaryAuthentication, isGitHubAuthentication, isBitBucketAuthentication}.Count(x => x) != 1)
            {
                throw new InvalidOperationException($"Unable to detect authentication mechanism. Please set {LfsUsernameVariable} and {LfsPasswordVariable} for simple user/password auth" +
                                                    $" or {GitHubOrganisationVariable} and {GitHubRepositoryVariable} for authentication against that repository on GitHub");
            }

            if (isDictionaryAuthentication)
            {
                services.AddLfsDictionaryAuthenticator(new Dictionary<string, string> { { lfsUsername, lfsPassword } });
            }

            if (isGitHubAuthentication)
            {
                services.AddLfsGitHubAuthenticator(new GitHubAuthenticatorConfig { Organisation = gitHubOrganisation, Repository = gitHubRepository });
            }

            if (isBitBucketAuthentication)
            {
                services.AddLfsBitBucketAuthenticator(new BitBucketAuthenticatorConfig { Workspace = bitBucketWorkspace, Repository = bitBucketRepository });
            }

            if (isS3Storage)
            {
                if (!string.IsNullOrWhiteSpace(s3ServiceURL) && !string.IsNullOrWhiteSpace(s3Region) && !string.IsNullOrWhiteSpace(s3AccessKey) && !string.IsNullOrWhiteSpace(s3AccessSecret))
                {
                    services.AddLfsS3Adapter(new S3BlobAdapterConfig { Bucket = lfsBucket }, new AmazonS3Client(s3AccessKey, s3AccessSecret, new AmazonS3Config { UseAccelerateEndpoint = s3Acceleration, ServiceURL = s3ServiceURL, AuthenticationRegion = s3Region, SignatureVersion = "V4" }));

                }
                else
                {
                    services.AddLfsS3Adapter(new S3BlobAdapterConfig { Bucket = lfsBucket }, new AmazonS3Client(new AmazonS3Config { UseAccelerateEndpoint = s3Acceleration }));
                }
            }
            else if (isAzureStorage)
            {
                services.AddLfsAzureBlobAdapter(new AzureBlobAdapterConfig
                {
                    ConnectionString = lfsAzureStorageConnectionString,
                    ContainerName = config[LfsAzureStorageContainerNameVariable]
                });
            }
            else
            {
                throw new InvalidOperationException($"Missing environment variable {LfsBucketVariable} or {LfsAzureStorageConnectionStringVariable}.");
            }

            services.AddLfsApi();

            services.AddLogging(x => x.AddLambdaLogger());
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
