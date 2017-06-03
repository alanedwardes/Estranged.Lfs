using Amazon.S3;
using System;

namespace Estranged.Lfs.Adapter.S3
{
    public interface IS3BlobStoreConfig
    {
        string Bucket { get; }
        string KeyPrefix { get; }
        Protocol Protocol { get; }
        TimeSpan Expiry { get; }
    }
}
