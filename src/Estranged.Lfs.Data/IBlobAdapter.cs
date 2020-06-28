using System.Threading;
using System.Threading.Tasks;

namespace Estranged.Lfs.Data
{
    public interface IBlobAdapter
    {
        Task<SignedBlob> UriForUpload(string Oid, long size, CancellationToken token);
        Task<SignedBlob> UriForDownload(string Oid, CancellationToken token);
    }
}
