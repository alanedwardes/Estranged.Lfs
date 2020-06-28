using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Estranged.Lfs.Api;
using Estranged.Lfs.Adapter.S3;
using Amazon.S3;
using Estranged.Lfs.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Estranged.Lfs.Authenticator.GitHub;

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
            const string S3AccelerationVariable = "S3_ACCELERATION";

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            string lfsBucket = config[LfsBucketVariable] ?? throw new InvalidOperationException("Missing environment variable LFS_BUCKET");
            string lfsUsername = config[LfsUsernameVariable];
            string lfsPassword = config[LfsPasswordVariable];
            string gitHubOrganisation = config[GitHubOrganisationVariable];
            string gitHubRepository = config[GitHubRepositoryVariable];
            bool.TryParse(config[S3AccelerationVariable] ?? "false", out bool s3Acceleration);

            bool isDictionaryAuthentication = !string.IsNullOrWhiteSpace(lfsUsername) && !string.IsNullOrWhiteSpace(lfsPassword);
            bool isGitHubAuthentication = !string.IsNullOrWhiteSpace(gitHubOrganisation) && !string.IsNullOrWhiteSpace(gitHubRepository);

            // If all authentication mechanims are set, or none are set throw an error
            if (new[] {isDictionaryAuthentication, isGitHubAuthentication}.Count(x => x) != 1)
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

            services.AddSingleton<IAmazonS3>(x => new AmazonS3Client(new AmazonS3Config { UseAccelerateEndpoint = s3Acceleration }));

            services.AddLfsS3Adapter(new S3BlobAdapterConfig { Bucket = lfsBucket });
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
