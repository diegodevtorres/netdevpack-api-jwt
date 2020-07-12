using Login.Lib.Converts;
using Login.Models;
using Login.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Login.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserModel registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.AllErrors());
            
            var result = await _userService.CreateUser(registerUser);
            if(result.Succeeded)
                return Ok(result.Data);

            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginUserModel loginUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.AllErrors());

            var result = await _userService.DoLogin(loginUser, false, false);
            if(result.Succeeded)
                return Ok(result.Data);

            if(result.IsLockedOut)
            {
                return BadRequest("O usuário está bloqueado.");
            }

            return BadRequest(result.Errors);
        }
    }
}