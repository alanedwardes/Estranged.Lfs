namespace Estranged.Lfs.Authenticator.GitHub
{
    public interface IGitHubAuthenticatorConfig
    {
        string Organisation { get; }
        string Repository { get; }
    }
}
