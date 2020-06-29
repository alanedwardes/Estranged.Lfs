using Estranged.Lfs.Authenticator.BitBucket.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Estranged.Lfs.Authenticator.BitBucket
{
    internal interface IBitBucketClient
    {
        Task<RepositoryPermissions> GetRepositoryPermissions(string repositoryFullName, CancellationToken token);
    }
}