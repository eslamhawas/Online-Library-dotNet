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
   
    //[Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public UsersController(IUserRepository repo)
        {
            _repo = repo;
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

        [HttpPost("Register"),AllowAnonymous]

        public IActionResult Register(UserRegisterDto user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Register(user);
            return CreatedAtAction(nameof(Register), new { user.Email }, user);
        }

        [HttpPost("Login"),AllowAnonymous]

        public IActionResult Login(UserlLoginDto user)
        {

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
            return Ok(token);
        }


        [HttpPut("Modify/{id}")]

        public IActionResult Modify(int id,ModifyUserDTO DTO) 
        {
            var existingUser = _repo.GetUserByName(DTO);

            if (existingUser is null)
            {
                return BadRequest("Please Enter a Valid UserName");
            }

            if (id == 0)
            {
                _repo.Accept(existingUser);
            }
            if (id == 1)
            {
                _repo.Reject(existingUser);
            }

            if(id == 2)
            {
                _repo.MakeLibrarian(existingUser);
            }

            if (id==3)
            {
                _repo.MakeUser(existingUser);
            }

            return Ok("Success");


        }

    }
}
