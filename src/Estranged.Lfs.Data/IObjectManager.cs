using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Estranged.Lfs.Data.Entities;

namespace Estranged.Lfs.Data
{
    public interface IObjectManager
    {
        Task<IEnumerable<ResponseObject>> DownloadObjects(IList<RequestObject> objects, CancellationToken token);
        Task<IEnumerable<ResponseObject>> UploadObjects(IList<RequestObject> objects, CancellationToken token);
    }
}