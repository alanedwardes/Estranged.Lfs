using System;
using System.Threading.Tasks;

namespace Estranged.GitLfs.Storage
{
    public interface IBlobStore
    {
        Task<Uri> UriForUpload(string Oid, long size);
        Task<Uri> UriForDownload(string Oid);
    }
}
