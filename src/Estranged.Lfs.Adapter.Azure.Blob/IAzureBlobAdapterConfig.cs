using System;

namespace Estranged.Lfs.Adapter.Azure.Blob
{
    public interface IAzureBlobAdapterConfig
    {
        string ContainerName { get; }
        string KeyPrefix { get; }
        TimeSpan Expiry { get; }
    }
}