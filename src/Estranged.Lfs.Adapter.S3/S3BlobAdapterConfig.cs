using System;

namespace Estranged.Lfs.Adapter.S3
{
    public sealed class S3BlobAdapterConfig : IS3BlobAdapterConfig
    {
        public string Bucket { get; set; }
        public string KeyPrefix { get; set; }
        public TimeSpan Expiry { get; set; } = TimeSpan.FromHours(1);
    }
}
