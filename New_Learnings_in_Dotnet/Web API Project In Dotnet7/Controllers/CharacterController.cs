using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIProject_In_Dotnet_7.DTOs.Character;
using WebAPIProject_In_Dotnet_7.IServices;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Controllers;

[Authorize]
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
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
    {
        //return BadRequest(knight); // returns http status 404
        //return NotFound(knight); // returns http status 404
        //return Ok(characters); // returns http status 200

        var response = await _characterService.GetAllCharacters();
        if (response.Data is null)
            return NotFound(response);

        return Ok(response);
    }

    [HttpGet("GetSingle")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
    {
        var response = await _characterService.GetCharacterById(id);
        if (response.Data is null)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPost("AddCharacter")]
    public async Task<ActionResult<ServiceResponse<List<AddCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var response = await _characterService.AddCharacter(newCharacter);
        if (response.Data is null)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPut("UpdateCharacter")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(updatedCharacter);
        if (response.Data is null)
            return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("DeleteCharacter")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var response = await _characterService.DeleteCharacter(id);
        if (response.Data is null)
            return NotFound(response);

        return Ok(response);
    }
}