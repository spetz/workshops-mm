using Microsoft.AspNetCore.Mvc;

namespace Trill.Modules.Stories.Api.Controllers
{
    [ApiController]
    [Route("api/stories-module")]
    public class HomeController
    {
        [HttpGet("hello")]
        public ActionResult<string> Get() => "Trill";
    }
}