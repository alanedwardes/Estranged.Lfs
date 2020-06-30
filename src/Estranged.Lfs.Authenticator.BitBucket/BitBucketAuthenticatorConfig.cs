namespace Estranged.Lfs.Authenticator.BitBucket
{
    public sealed class BitBucketAuthenticatorConfig : IBitBucketAuthenticatorConfig
    {
        public string Workspace { get; set; }
        public string Repository { get; set; }
    }
}
