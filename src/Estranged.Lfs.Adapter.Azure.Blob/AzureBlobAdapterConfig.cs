using System;

namespace Estranged.Lfs.Adapter.Azure.Blob
{
    public class AzureBlobAdapterConfig : IAzureBlobAdapterConfig
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }

        public string KeyPrefix { get; set; }

        public TimeSpan Expiry { get; set; } = TimeSpan.FromHours(1);
    }
}