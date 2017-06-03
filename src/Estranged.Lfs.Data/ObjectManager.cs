using System;
using System.Collections.Generic;
using System.Text;

namespace Estranged.Lfs.Data
{
    public class ObjectManager
    {
        public void Test()
        {
            IEnumerable<Task<SignedBlob>> uploadUriTasks = request.Objects.Select(ob => blobStore.UriForUpload(ob.Oid, ob.Size));

            SignedBlob[] signedBlobs = await Task.WhenAll(uploadUriTasks);

            return new BatchResponse
            {
                Transfer = request.Transfers.First(), // TODO: this is not correct
                Objects = request.Objects.Select((ob, index) => new ResponseObject
                {
                    Oid = ob.Oid,
                    Size = ob.Size,
                    Authenticated = true,
                    Actions = new Actions
                    {
                        Upload = new Entities.Action
                        {
                            Href = signedBlobs[index].Uri,
                            ExpiresIn = (long)signedBlobs[index].Expiry.TotalSeconds
                        }
                    }
                })
            };
        }
    }
}
