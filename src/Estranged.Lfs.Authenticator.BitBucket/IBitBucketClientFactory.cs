using System;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    internal interface IBitBucketClientFactory
    {
        IBitBucketClient CreateClient(Uri baseAddress, string username, string password);
    }
}