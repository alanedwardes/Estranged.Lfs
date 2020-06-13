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

            SignedBlob[] signedBlobs = await Task.WhenAll(uploadUriTasks).ConfigureAwait(false);

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
                        ExpiresIn = (long)signedBlobs[index].Expiry.TotalSeconds,
                        Headers = signedBlobs[index].Headers
                    }
                }
            });
        }

        public async Task<IEnumerable<ResponseObject>> DownloadObjects(IList<RequestObject> objects)
        {
            var responseObjects = new List<ResponseObject>();
            foreach ((RequestObject requestObject, Task<SignedBlob> signedBlobTask) in objects.Select(x => (x, blobAdapter.UriForDownload(x.Oid))))
            {
                var signedBlob = await signedBlobTask.ConfigureAwait(false);

                var responseObject = new ResponseObject();

                if (signedBlob.ErrorCode.HasValue)
                {
                    responseObjects.Add(new ResponseObject
                    {
                        Oid = requestObject.Oid,
                        Size = requestObject.Size,
                        Error = new ResponseObjectError
                        {
                            Code = signedBlob.ErrorCode.Value,
                            Message = signedBlob.ErrorMessage
                        }
                    });
                }
                else
                {
                    responseObjects.Add(new ResponseObject
                    {
                        Oid = requestObject.Oid,
                        Size = signedBlob.Size.Value,
                        Authenticated = true,
                        Actions = new Actions
                        {
                            Download = new Action
                            {
                                Href = signedBlob.Uri,
                                ExpiresIn = (long)signedBlob.Expiry.TotalSeconds,
                                Headers = signedBlob.Headers
                            }
                        }
                    });
                }
            }

            return responseObjects;
        }
    }
}
