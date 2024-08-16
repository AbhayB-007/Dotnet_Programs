using Microsoft.AspNetCore.Mvc;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static Character knight = new Character();

        public IActionResult Get()
        {
            //return BadRequest(knight); // returns http status 404
            //return NotFound(knight); // returns http status 404
            return Ok(knight); // returns http status 200
        }
    }
}
