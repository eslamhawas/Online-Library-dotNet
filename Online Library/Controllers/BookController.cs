using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Library.Interfaces;

namespace Online_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BookController(IBooksRepository booksRepository)
        {
                _booksRepository = booksRepository;
        }
    }
}
