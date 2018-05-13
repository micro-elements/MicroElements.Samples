using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Swashbuckle.AuthWebApi
{
    [Route("api/[controller]")]
    public class SampleApiController : Controller
    {
        [HttpGet("[action]")]
        public string UnprotectedGet()
        {
            return "Safe response";
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("[action]")]
        public string ProtectedGet()
        {
            return "Safe response";
        }

        //[Authorize(Roles = "Administrator")]
        [HttpPost("[action]")]
        public IActionResult AddCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public decimal Discount { get; set; }
        public string Address { get; set; }
    }
}