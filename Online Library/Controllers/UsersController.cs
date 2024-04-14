using Microsoft.AspNetCore.Mvc;
using Online_Library.DTOS;
using Online_Library.Interfaces;
using Online_Library.Models;

namespace Online_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        public UsersController(IUserRepository repo)
        {
            _repo = repo;
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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
            {
                return NotFound(user);
            }
            return Ok(user);
        }

        [HttpPost("Register")]

        public IActionResult Register(UserRegisterDto user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Register(user);
            return CreatedAtAction(nameof(Register), new { user.Email }, user);
        }

        [HttpPut("Accept/{id}")]

        public IActionResult Accept(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
            {
                return NotFound(user);
            }
            _repo.Accept(user);

            return NoContent();


        }

        [HttpPut("Promote/{id}")]

        public IActionResult MakeLibrarian(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
            {
                return NotFound(user);
            }
            _repo.MakeLibrarian(user);
            return Ok(user);
        }

        [HttpPut("Reject/{id}")]

        public IActionResult Reject(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
            {
                return NotFound(user);
            }
            _repo.Reject(user);

            return NoContent();
        }

    }
}
