using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Library.Interfaces;

namespace Online_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowedBooksController : ControllerBase
    {
        private readonly IBorrowedBooksRepository _borrowedBooksRepository;

        public BorrowedBooksController(IBorrowedBooksRepository borrowedBooksRepository)
        {
            _borrowedBooksRepository = borrowedBooksRepository;
        }

        [HttpGet]

        public IActionResult GetBorrowedBooks() 
        {
            var books = _borrowedBooksRepository.GetBorrowedBooks();
            return Ok(books);
        }
    }
}
