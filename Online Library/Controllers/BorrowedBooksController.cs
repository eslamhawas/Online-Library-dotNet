using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Library.DTOS;
using Online_Library.Interfaces;

namespace Online_Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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

        [HttpGet("{id}")]

        public IActionResult GetBorrowedBookByID(int id)
        {
            var books = _borrowedBooksRepository.GetBorrowedBooksById(id);

            return Ok(books);
        }


        [HttpPut("borrowedBooks/{ordernumber}/{state}")]
        public IActionResult updateborrowedbooks(int ordernumber, bool state)
        {


            _borrowedBooksRepository.UpdateBorrowedBook(ordernumber, state);
            return Ok();


        }

        [HttpPost()]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddBorrowedBook(AddBorrowedBookDto borrowedBook)
        {

            if (borrowedBook == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            _borrowedBooksRepository.AddBorrowedBook(borrowedBook);

            return Ok();



        }
    }
}
