using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound(); // Returns a 404 Not Found response
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            throw new Exception("This is a server error.");
            return Ok();
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(); // 400
        }
        [HttpGet("badrequest/{id}")]
        public IActionResult GetBadRequest(int id) // Validation error example
        {
            return BadRequest(); // 400
        }
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized(); // 401
        }

    }
}
