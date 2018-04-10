using Microsoft.AspNetCore.Mvc;
using Swashbuckle.FluentValidation.NetCore21WebApi.Models;

namespace Swashbuckle.FluentValidation.NetCore21WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SampleApiController : Controller
    {
        [HttpPost("[action]")]
        public IActionResult AddSample([FromBody] Sample sample)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}