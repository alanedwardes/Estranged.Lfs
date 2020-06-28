using System.Threading.Tasks;

namespace Estranged.Lfs.Data
{
    public interface IAuthenticator
    {
        Task Authenticate(string username, string password, LfsPermission requiredPermission);
    }
}
