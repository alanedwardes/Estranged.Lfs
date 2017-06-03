using Estranged.Lfs.Api.Entities;
using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Estranged.Lfs.Api.Controllers
{
    [Route("objects")]
    public class ObjectsController : ControllerBase
    {
        private readonly IObjectManager objectManager;

        public ObjectsController(IObjectManager objectManager)
        {
            this.objectManager = objectManager;
        }

        [HttpPost("batch")]
        public async Task<BatchResponse> BatchAsync([FromBody] BatchRequest request)
        {
            if (request.Operation == LfsOperation.Upload)
            {
                return new BatchResponse
                {
                    Transfer = request.Transfers.First(), // TODO: this is not correct
                    Objects = await objectManager.UploadObjects(request.Objects)
                                                 .ConfigureAwait(false)
                };
            }

            if (request.Operation == LfsOperation.Download)
            {
                return new BatchResponse
                {
                    Transfer = request.Transfers.First(), // TODO: this is not correct
                    Objects = await objectManager.DownloadObjects(request.Objects)
                                                 .ConfigureAwait(false)
                };
            }

            throw new Exception("Method not implemented");
        }
    }
}
