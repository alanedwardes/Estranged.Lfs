using Microsoft.AspNetCore.Mvc;

namespace Estranged.GitLfs.Controllers
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
