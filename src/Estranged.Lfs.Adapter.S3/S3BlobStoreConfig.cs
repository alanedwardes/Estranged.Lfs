using Amazon.S3;
using System;

namespace Estranged.Lfs.Adapter.S3
{
    public class S3BlobStoreConfig : IS3BlobStoreConfig
    {
        public string Bucket { get; set; }
        public string KeyPrefix { get; set; }
        public Protocol Protocol { get; set; } = Protocol.HTTPS;
        public TimeSpan Expiry { get; set; } = TimeSpan.FromHours(1);
    }
}
