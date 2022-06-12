using Core.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<AppUser> _singInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthenticationController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            _singInManager = signInManager,
            _userManager = userManager
        }
    }
}
