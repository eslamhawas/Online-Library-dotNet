using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Library.Interfaces;

namespace Online_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BookController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BookController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
    }
}
