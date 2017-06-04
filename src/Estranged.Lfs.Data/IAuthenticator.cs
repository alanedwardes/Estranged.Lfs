namespace Estranged.Lfs.Data
{
    public interface IAuthenticator
    {
        bool Authenticate(string username, string password);
    }
}
