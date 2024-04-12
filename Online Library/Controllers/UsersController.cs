using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _repo.GetAll();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
            {
                return NotFound(user);
            }
            return Ok(user);
        }

        [HttpPost]

        public IActionResult Add(User user)
        {

            ModelState.Remove("user.Id");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Add(user);
            return CreatedAtAction(nameof(Add), new { id = user.Id }, user);
        }
    }
}
