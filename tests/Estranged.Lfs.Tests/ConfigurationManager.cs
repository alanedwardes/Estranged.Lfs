using Microsoft.Extensions.Configuration;

namespace Estranged.Lfs.Tests
{
    public static class ConfigurationManager
    {
        private static IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("credentials.json", true).AddEnvironmentVariables().Build();

        private static (string Username, string Password) ParseUsernamePasswordCredentials(string credentials)
        {
            var parts = credentials.Split(":");
            return (parts[0], parts[1]);
        }

        public static string GetS3BucketName() => Configuration["S3_BUCKET"];

        public static (string Username, string Password) GetBitBucketCredentials()
        {
            return ParseUsernamePasswordCredentials(Configuration["BITBUCKET_CREDENTIALS"]);
        }

        public static (string Username, string Password) GetGitHubCredentials()
        {
            return ParseUsernamePasswordCredentials(Configuration["GH_CREDENTIALS"]);
        }

        public static string GetAzureStorageAccountConnectionString()
        {
            return Configuration["AZURE_STORAGE_CONNECTIONSTRING"];
        }
    }
}
