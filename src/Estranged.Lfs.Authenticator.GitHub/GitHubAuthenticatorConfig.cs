namespace Estranged.Lfs.Authenticator.GitHub
{
    public class GitHubAuthenticatorConfig : IGitHubAuthenticatorConfig
    {
        public string Organisation { get; set; }
        public string Repository { get; set; }
    }
}
