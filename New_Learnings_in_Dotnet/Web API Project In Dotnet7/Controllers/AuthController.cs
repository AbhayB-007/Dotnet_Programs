using Microsoft.AspNetCore.Mvc;
using WebAPIProject_In_Dotnet_7.Data;
using WebAPIProject_In_Dotnet_7.DTOs.User;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authrepo)
        {
            _authRepo = authrepo;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {            
            var response = await _authRepo.Register(new Models.User { Username = request.Username }, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }            
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
