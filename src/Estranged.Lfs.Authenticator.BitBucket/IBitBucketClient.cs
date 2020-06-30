using Estranged.Lfs.Authenticator.BitBucket.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    internal interface IBitBucketClient
    {
        Task<Repository> GetRepository(string workspace, string repository, CancellationToken token);
        Task<RepositoryPermissions> GetRepositoryPermissions(string workspace, string repository, CancellationToken token);
    }
}