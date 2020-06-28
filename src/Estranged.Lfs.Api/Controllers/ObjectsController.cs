using Estranged.Lfs.Api.Entities;
using Estranged.Lfs.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
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

        private CancellationToken GenerateTimeoutToken() => new CancellationTokenSource(TimeSpan.FromSeconds(25)).Token;

        [HttpPost("batch")]
        public async Task<BatchResponse> BatchAsync([FromBody] BatchRequest request)
        {
            if (request.Operation == LfsOperation.Upload)
            {
                return new BatchResponse
                {
                    Transfer = request.Transfers.First(),
                    Objects = await objectManager.UploadObjects(request.Objects, GenerateTimeoutToken())
                                                 .ConfigureAwait(false)
                };
            }

            if (request.Operation == LfsOperation.Download)
            {
                return new BatchResponse
                {
                    Transfer = request.Transfers.First(),
                    Objects = await objectManager.DownloadObjects(request.Objects, GenerateTimeoutToken())
                                                 .ConfigureAwait(false)
                };
            }

            throw new Exception("Method not implemented");
        }
    }
}
