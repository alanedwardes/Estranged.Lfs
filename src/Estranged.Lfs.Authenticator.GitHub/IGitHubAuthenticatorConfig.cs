using System;

namespace Estranged.Lfs.Authenticator.GitHub
{
    public interface IGitHubAuthenticatorConfig
    {
        Uri BaseAddress { get; }
        string Organisation { get; }
        string Repository { get; }
    }
}
