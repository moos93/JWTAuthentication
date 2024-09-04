using Application.Contracts;
using Application.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser user;
        public UserController(IUser user)
        {
            this.user = user;
        }
        [HttpPost("login")]
        public async Task<ActionResult <LoginRespons>> LogUserIn(LogInDTO logInDTO)
        {
            var result = await user.LoginUserAsync(logInDTO);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<ActionResult<LoginRespons>> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            var result = await user.RegisterUserAsync(registerUserDTO);
            return Ok(result);
        }
    }
}
