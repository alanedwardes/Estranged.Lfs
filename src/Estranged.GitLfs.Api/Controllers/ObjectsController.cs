using Estranged.GitLfs.Api.Entities;
using Estranged.GitLfs.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

                Uri[] downloadUris = await Task.WhenAll(downloadUriTasks);

                return new BatchResponse
                {
                    Transfer = request.Transfers.First(), // TODO: this is not correct
                    Objects = request.Objects.Select((ob, index) => new ResponseObject
                    {
                        Oid = ob.Oid,
                        Size = ob.Size,
                        Actions = new Dictionary<LfsOperation, Entities.Action>
                        {
                            {
                                LfsOperation.Upload,
                                new Entities.Action
                                {
                                    Href = downloadUris[index]
                                }
                            }
                        }
                    })
                };
            }

            throw new Exception("Method not implemented");
        }
    }
}
