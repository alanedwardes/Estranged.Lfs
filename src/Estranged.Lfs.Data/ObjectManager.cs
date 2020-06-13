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
            IReadOnlyDictionary<RequestObject, Task<SignedBlob>> objectTaskPairs = objects.ToDictionary(x => x, x => blobAdapter.UriForDownload(x.Oid));

            var responseObjects = new List<ResponseObject>();
            foreach (var objectTaskPair in objectTaskPairs)
            {
                var blob = await objectTaskPair.Value;

                var responseObject = new ResponseObject();

                if (blob.ErrorCode.HasValue)
                {
                    responseObjects.Add(new ResponseObject
                    {
                        Oid = objectTaskPair.Key.Oid,
                        Size = objectTaskPair.Key.Size,
                        Error = new ResponseObjectError
                        {
                            Code = blob.ErrorCode.Value,
                            Message = blob.ErrorMessage
                        }
                    });
                }
                else
                {
                    responseObjects.Add(new ResponseObject
                    {
                        Oid = objectTaskPair.Key.Oid,
                        Size = blob.Size.Value,
                        Authenticated = true,
                        Actions = new Actions
                        {
                            Download = new Action
                            {
                                Href = blob.Uri,
                                ExpiresIn = (long)blob.Expiry.TotalSeconds,
                                Headers = blob.Headers
                            }
                        }
                    });
                }
            }

            return responseObjects;
        }
    }
}
