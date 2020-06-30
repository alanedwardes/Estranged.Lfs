using System;

namespace Estranged.Lfs.Adapter.S3
{
    public interface IS3BlobAdapterConfig
    {
        string Bucket { get; }
        string KeyPrefix { get; }
        TimeSpan Expiry { get; }
    }
}
