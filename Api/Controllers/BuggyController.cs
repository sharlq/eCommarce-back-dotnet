using Microsoft.AspNetCore.Mvc;

namespace dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : Controller
    {


        [HttpGet("notFound")]
        public IActionResult NotFoundErro()
        {
            return NotFound();
        }

        [HttpGet("serverError")]
        public IActionResult ServerError()
        {
           throw new NotImplementedException();
        }

        [HttpGet("badrequest")]
        public IActionResult BadRequestError()
        {
            return BadRequest();
        }



    }
}
