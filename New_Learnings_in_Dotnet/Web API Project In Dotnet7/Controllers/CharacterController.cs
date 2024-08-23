using Microsoft.AspNetCore.Mvc;
using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Controllers
{
    [ApiController]
    [Route("api")]
    public class CharacterController : ControllerBase
    {
        //private static List<Character> characters = new List<Character>
        //{
        //    new Character (),
        //    new Character { Id = 1,  Name = "Sam" }
        //};

        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        // Http Request Methods
        //1). A set of request methods to indicate the desired action to be performed for a given resource.
        //2). GET, POST, PUT, DELETE etc.
        //      a). GET :- this method requests a representation of the specified resource.
        //      b). POST :- this method is used to submit an entity to the specified resource, often causing
        //                  a change in state or side effects on the server.
        //      c). PUT/UPDATE :- this method replaces or updates all current representations of the target resources with the
        //                 request payload.
        //      d). DELETE :- this method deletes the specified resource.
        //3). Create:  POST
        //    Read:    GET
        //    Update:  PUT
        //    Delete:  DELETE
        // All together forms CRUD Operations.

        [HttpGet("GetAll")]
        //[Route("GetAll")] // OR
        public ActionResult<List<Character>> Get()
        {
            //return BadRequest(knight); // returns http status 404
            //return NotFound(knight); // returns http status 404
            //return Ok(characters); // returns http status 200
            return Ok(_characterService.GetAllCharacters());
        }

        [HttpGet("GetSingle")]
        public ActionResult<Character> GetSingle(int id)
        {
            //return BadRequest(knight); // returns http status 404
            //return NotFound(knight); // returns http status 404
            //return Ok(characters.FirstOrDefault(x => x.Id == id)); // returns http status 200
            return Ok(_characterService.GetCharacterById(id));
        }

        [HttpPost("AddCharacter")]
        public ActionResult<List<Character>> AddCharacter(Character newCharacter)
        {            
            return Ok(_characterService.AddCharacter(newCharacter));
        }
    }
}
