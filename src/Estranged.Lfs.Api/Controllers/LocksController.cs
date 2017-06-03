using Microsoft.AspNetCore.Mvc;

namespace Estranged.Lfs.Api.Controllers
{
    [Route("locks")]
    public class LocksController : ControllerBase
    {
        [HttpPost("verify")]
        public NotFoundResult Verify()
        {
            return NotFound();
        }
    }
}
