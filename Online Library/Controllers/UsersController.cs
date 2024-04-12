using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Library.Interfaces;
using Online_Library.Models;
using Online_Library.Repositories;

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

        [HttpPost("AddUser")]

        public IActionResult AddUser(User user)
        {

            ModelState.Remove("user.Id");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Add(user);
            return CreatedAtAction(nameof(AddUser), new { id = user.Id }, user);
        }

        [HttpPut("Accept/{id}")]

        public IActionResult Accept(int id)
        {
            var user = _repo.GetById(id);
            if (user ==null)
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
