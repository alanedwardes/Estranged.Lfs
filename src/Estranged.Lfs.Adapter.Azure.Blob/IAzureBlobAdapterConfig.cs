using System;

namespace Estranged.Lfs.Adapter.Azure.Blob
{
    public interface IAzureBlobAdapterConfig
    {
        string ConnectionString { get;  }
        string ContainerName { get; }
        string KeyPrefix { get; }
        TimeSpan Expiry { get; }
    }
}