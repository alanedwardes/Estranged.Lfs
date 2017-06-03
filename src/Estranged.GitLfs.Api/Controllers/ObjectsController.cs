using Estranged.GitLfs.Api.Entities;
using Estranged.GitLfs.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Estranged.GitLfs.Api.Controllers
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
                var downloadUriTasks = request.Objects.Select(ob => blobStore.UriForUpload(ob.Oid, ob.Size));

                SignedBlob[] signedBlobs = await Task.WhenAll(downloadUriTasks);

                BatchResponse response = new BatchResponse
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
                var test = JsonConvert.SerializeObject(response, Formatting.Indented);
                return response;
            }

            throw new Exception("Method not implemented");
        }
    }
}
