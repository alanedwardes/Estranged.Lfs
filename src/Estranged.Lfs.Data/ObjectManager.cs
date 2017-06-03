using Estranged.Lfs.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estranged.Lfs.Data
{
    public class ObjectManager : IObjectManager
    {
        private readonly IBlobAdapter blobAdapter;

        public ObjectManager(IBlobAdapter blobAdapter)
        {
            this.blobAdapter = blobAdapter;
        }

        public async Task<IEnumerable<ResponseObject>> UploadObjects(IList<RequestObject> objects)
        {
            IEnumerable<Task<SignedBlob>> uploadUriTasks = objects.Select(ob => blobAdapter.UriForUpload(ob.Oid, ob.Size));

            SignedBlob[] signedBlobs = await Task.WhenAll(uploadUriTasks);

            return objects.Select((ob, index) => new ResponseObject
            {
                Oid = ob.Oid,
                Size = ob.Size,
                Authenticated = true,
                Actions = new Actions
                {
                    Upload = new Action
                    {
                        Href = signedBlobs[index].Uri,
                        ExpiresIn = (long)signedBlobs[index].Expiry.TotalSeconds
                    }
                }
            });
        }

        public async Task<IEnumerable<ResponseObject>> DownloadObjects(IList<RequestObject> objects)
        {
            IEnumerable<Task<SignedBlob>> downloadUriTasks = objects.Select(ob => blobAdapter.UriForDownload(ob.Oid));

            SignedBlob[] signedBlobs = await Task.WhenAll(downloadUriTasks);

            return objects.Select((ob, index) => new ResponseObject
            {
                Oid = ob.Oid,
                Size = ob.Size,
                Authenticated = true,
                Actions = new Actions
                {
                    Download = new Action
                    {
                        Href = signedBlobs[index].Uri,
                        ExpiresIn = (long)signedBlobs[index].Expiry.TotalSeconds
                    }
                }
            });
        }
    }
}
