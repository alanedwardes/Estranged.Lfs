using System;

namespace Estranged.Lfs.Authenticator.GitHub
{
    public sealed class GitHubAuthenticatorConfig : IGitHubAuthenticatorConfig
    {
        public Uri BaseAddress { get; set; } = new Uri("https://api.github.com/");
        public string Organisation { get; set; }
        public string Repository { get; set; }
    }
}
