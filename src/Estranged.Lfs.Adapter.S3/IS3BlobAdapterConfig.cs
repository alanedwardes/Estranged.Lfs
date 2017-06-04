using Amazon.S3;
using System;

namespace Estranged.Lfs.Adapter.S3
{
    public interface IS3BlobAdapterConfig
    {
        string Bucket { get; }
        string KeyPrefix { get; }
        Protocol Protocol { get; }
        TimeSpan Expiry { get; }
    }
}
