using System;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    public interface IBitBucketAuthenticatorConfig
    {
        Uri BaseAddress { get; }
        string Workspace { get; }
        string Repository { get; }
    }
}
