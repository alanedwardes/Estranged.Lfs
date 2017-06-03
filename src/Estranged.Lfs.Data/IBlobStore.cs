using System;
using System.Threading.Tasks;

namespace Estranged.Lfs.Data
{
    public class SignedBlob
    {
        public Uri Uri { get; set; }
        public TimeSpan Expiry { get; set; }
    }

    public interface IBlobStore
    {
        Task<SignedBlob> UriForUpload(string Oid, long size);
        Task<SignedBlob> UriForDownload(string Oid);
    }
}
