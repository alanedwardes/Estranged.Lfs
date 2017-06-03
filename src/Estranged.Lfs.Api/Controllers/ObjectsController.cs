using Estranged.Lfs.Api.Entities;
using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estranged.Lfs.Api.Controllers
{
    [Route("objects")]
    public class ObjectsController : ControllerBase
    {
        private readonly ILogger<ObjectsController> logger;
        private readonly IBlobStore blobStore;

        public ObjectsController(ILogger<ObjectsController> logger, IBlobStore blobStore)
        {
            this.logger = logger;
            this.blobStore = blobStore;
        }

        [HttpPost("batch")]
        public async Task<BatchResponse> BatchAsync([FromBody] BatchRequest request)
        {
            if (request.Operation == LfsOperation.Upload)
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

            if (request.Operation == LfsOperation.Download)
            {
                IEnumerable<Task<SignedBlob>> downloadUriTasks = request.Objects.Select(ob => blobStore.UriForDownload(ob.Oid));

                SignedBlob[] signedBlobs = await Task.WhenAll(downloadUriTasks);

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
                            Download = new Entities.Action
                            {
                                Href = signedBlobs[index].Uri,
                                ExpiresIn = (long)signedBlobs[index].Expiry.TotalSeconds
                            }
                        }
                    })
                };
            }

            throw new Exception("Method not implemented");
        }
    }
}
