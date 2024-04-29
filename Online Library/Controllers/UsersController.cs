using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Library.DTOS;
using Online_Library.Interfaces;

namespace Online_Library.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    /*Authorize attribute can also be usen on controller
    to allow any access you can use AllowAnonymous*/

    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IHttpContextAccessor _httpcontextacessor;

        public UsersController(IUserRepository repo, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpcontextacessor = httpContextAccessor;
        }

        [HttpGet("Users")]

        public IActionResult GetUsers()
        {
            var users = _repo.GetUsers();

            return Ok(users);

        }


        [HttpGet("Accepted")]
        public IActionResult GetAllAcceptedUsers()
        {
            var users = _repo.GetAcceptedUsers();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpGet("Pending")]
        public IActionResult GetPendingUsers()
        {
            var users = _repo.GetPendingUsers();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpGet("Users/{id}")]
        public IActionResult GetById(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
            {
                return NotFound("No User With this Id");
            }
            return Ok(user);
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
            var existingUser = _repo.CheckForExistingUsers(user);

            if (!(existingUser == null))
            {
                return BadRequest("Username or email already exists");
            }


            _repo.Register(user);
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
            var existingUser = _repo.GetUserByEmail(user);

            if (existingUser == null || !_repo.VerifyPasswordHash(user.Password, existingUser.PasswordHash, existingUser.PassordSalt))
            {
                return NotFound("There Is no User With this credntials");
            }
            if (existingUser.IsAccepted is false || existingUser.IsAccepted is null)
            {
                return NotFound("User not Accepted yet");
            }
            string token = _repo.CreateToken(existingUser);
            session.SetString("username", existingUser.UserName);
            session.SetString("email", existingUser.Email);
            var tokenid = new TokenID();
            tokenid.jwt = token;
            tokenid.id = existingUser.Id;
            tokenid.session = session;
            return Ok(tokenid);
        }


        [HttpPut("Modify/{action}")]

        public IActionResult Modify(int action, [FromBody] ModifyUserDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = _repo.GetUserByID(DTO);

            if (existingUser is null)
            {
                return BadRequest("There is no user with this username");
            }

            if (action == 0)
            {
                _repo.Accept(existingUser);
            }
            if (action == 1)
            {
                _repo.Reject(existingUser);
            }

            if (action == 2)
            {
                _repo.MakeLibrarian(existingUser);
            }

            if (action == 3)
            {
                _repo.MakeUser(existingUser);
            }

            return Ok("Success");


        }

    }
}
