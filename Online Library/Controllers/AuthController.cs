using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Library.Data;
using Online_Library.DTOS;

namespace Online_Library.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _httpcontextacessor;
        public AuthController(IAuthRepository authRepository,[FromServices] IHttpContextAccessor httpcontextacessor)
        {
            _authRepository = authRepository;
            _httpcontextacessor = httpcontextacessor;
        }



        [HttpPost("Register"), AllowAnonymous]

        public IActionResult Register(UserRegisterDto user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user.Password != user.RePassword)
            {
                return BadRequest("Password And Confirm Password Doesn't Match");
            }
            string userName = user.UserName;
            string email = user.Email;
            var existingUser = _authRepository.CheckForExistingUsers(user);

            if (!(existingUser == null))
            {
                return BadRequest("Username or email already exists");
            }


            _authRepository.Register(user);
            return CreatedAtAction(nameof(Register), new { user.Email }, user);
        }


        [HttpPost("Login"), AllowAnonymous]

        public IActionResult Login(UserlLoginDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var session = _httpcontextacessor.HttpContext.Session;
            var existingUser = _authRepository.GetUserByEmail(user);

            if (existingUser == null || !_authRepository.VerifyPasswordHash(user.Password, existingUser.PasswordHash, existingUser.PassordSalt))
            {
                return NotFound("There Is no User With this credntials");
            }
            if (existingUser.IsAccepted is false || existingUser.IsAccepted is null)
            {
                return NotFound("User not Accepted yet");
            }
            string token = _authRepository.CreateToken(existingUser);
            session.SetString("username", existingUser.UserName);
            session.SetString("email", existingUser.Email);
            var tokenid = new TokenID();
            tokenid.jwt = token;
            tokenid.id = existingUser.Id;
            return Ok(tokenid);
        }
    }
}
