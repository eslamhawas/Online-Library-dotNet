using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Library.Data;
using Online_Library.DTOS;
using Online_Library.Models;

namespace Online_Library.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    /*Authorize attribute can also be usen on controller
    to allow any access you can use AllowAnonymous*/

    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : ControllerBase
    {
        private readonly IDataRepository<Users> _userrepo;
        private readonly IHttpContextAccessor _httpcontextacessor;

        public UsersController([FromServices] IHttpContextAccessor httpContextAccessor, IDataRepository<Users> repo)
        {
            _httpcontextacessor = httpContextAccessor;
            _userrepo = repo;
        }

        [HttpGet("Users")]

        public async Task<IActionResult> GetUsers()
        {
            var users = await _userrepo.GetAllAsync();

            return Ok(users);

        }


        [HttpGet("Users/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userrepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("No User With this Id");
            }
            return Ok(user);
        }

        


        [HttpPut("Modify/{id}")]

        public async Task<IActionResult> Modify([FromRoute] int id, [FromBody] ModifyUserDTO DTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingUser = await _userrepo.GetByIdAsync(DTO.userid);

            if (existingUser is null)
            {
                return BadRequest("There is no user with this username");
            }

            if (id == 0)
            {
                existingUser.IsAccepted = true;
               _userrepo.Update(existingUser);
               await _userrepo.SaveChangesAsync();
            }
            if (id == 1)
            {
                existingUser.IsAccepted = false;
                _userrepo.Update(existingUser);
                await _userrepo.SaveChangesAsync();
            }

            if (id == 2)
            {
                existingUser.IsAdmin = true;
                _userrepo.Update(existingUser);
                await _userrepo.SaveChangesAsync();
            }

            if (id == 3)
            {
                existingUser.IsAdmin = false;
                _userrepo.Update(existingUser);
                await _userrepo.SaveChangesAsync();
            }

            return Ok("Success");


        }

    }
}
