using Estranged.GitLfs.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace Estranged.GitLfs.Api.Controllers
{
    [Route("objects")]
    public class ObjectsController : ControllerBase
    {
        private readonly ILogger<ObjectsController> logger;

        public ObjectsController(ILogger<ObjectsController> logger)
        {
            this.logger = logger;
        }

        [HttpPost("batch")]
        public object Batch()
        {
            using (var sr = new StreamReader(Request.Body))
            {
                var json = JsonConvert.DeserializeObject<BatchRequest>(sr.ReadToEnd());
            }

                

            return NotFound();
        }
    }
}
