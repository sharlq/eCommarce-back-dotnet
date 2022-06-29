using Core.Dtos;
using Core.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {

        private readonly IAuthRepo _authRepo;

        public AuthenticationController(

            IAuthRepo authRepo
            )
        {

            _authRepo = authRepo;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ActionResponse<ReturnUserDto>>> Login(LoginDto login)
        {
            return await  _authRepo.Login(login);
        }

        [HttpPost("register")]
        public async Task<ActionResponse<ReturnUserDto>> Register(CreateUserDto user)
        {
            return await _authRepo.CreateUser(user);
        }

        [HttpGet("IsEmailUsed/{email}")]
        public async Task<bool> CheckEmail(string email)
        {
            bool isUser = await _authRepo.UserExists(email);
            return isUser;
        }
    }
}
