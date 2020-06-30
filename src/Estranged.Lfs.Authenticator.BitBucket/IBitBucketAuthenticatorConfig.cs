namespace Estranged.Lfs.Authenticator.BitBucket
{
    public interface IBitBucketAuthenticatorConfig
    {
        string Workspace { get; }
        string Repository { get; }
    }
}
