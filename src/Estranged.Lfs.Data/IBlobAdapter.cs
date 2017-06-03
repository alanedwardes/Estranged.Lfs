using System.Threading.Tasks;

namespace Estranged.Lfs.Data
{
    public interface IBlobAdapter
    {
        Task<SignedBlob> UriForUpload(string Oid, long size);
        Task<SignedBlob> UriForDownload(string Oid);
    }
}
