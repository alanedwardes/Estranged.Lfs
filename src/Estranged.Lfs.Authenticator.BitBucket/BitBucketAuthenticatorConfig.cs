using System;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    public sealed class BitBucketAuthenticatorConfig : IBitBucketAuthenticatorConfig
    {
        public Uri BaseAddress { get; set; } = new Uri("https://api.bitbucket.org/");
        public string Workspace { get; set; }
        public string Repository { get; set; }
    }
}
