using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Estranged.Lfs.Data;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Estranged.Lfs.Adapter.Azure.Blob
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLfsAzureBlobAdapter(this IServiceCollection services, IAzureBlobAdapterConfig config, bool createContainerIfNotExists = true)
        {
            if (string.IsNullOrEmpty(config.ConnectionString))
            {
                throw new ArgumentNullException(nameof(config.ConnectionString), "Provide a ConnectionString variable");
            }

            var sa = new BlobServiceClient(config.ConnectionString);
            return AddLfsAzureBlobAdapter(services, config, sa, createContainerIfNotExists);
        }

        public static IServiceCollection AddLfsAzureBlobAdapter(this IServiceCollection services, IAzureBlobAdapterConfig config, BlobServiceClient blobClient, bool createContainerIfNotExists = true)
        {
            var containerClient = blobClient.GetBlobContainerClient(config.ContainerName);

            if (createContainerIfNotExists)
            {
                containerClient.CreateIfNotExistsAsync(PublicAccessType.None).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            return services
                .AddSingleton<IBlobAdapter, AzureBlobAdapter>()
                .AddSingleton(containerClient)
                .AddSingleton(config);
        }
    }
}