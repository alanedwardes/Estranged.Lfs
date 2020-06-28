namespace Estranged.Lfs.Authenticator.GitHub
{
    public sealed class GitHubAuthenticatorConfig : IGitHubAuthenticatorConfig
    {
        public string Organisation { get; set; }
        public string Repository { get; set; }
    }
}
