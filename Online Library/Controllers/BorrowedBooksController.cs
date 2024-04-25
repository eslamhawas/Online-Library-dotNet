using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Library.DTOS;
using Online_Library.Interfaces;

namespace Online_Library.Controllers
{
    [Route("api/v1/borrowedBooks")]
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



        [HttpGet("{id}"), AllowAnonymous]

        public IActionResult GetBorrowedBookByID(int id)
        {
            var books = _borrowedBooksRepository.GetBorrowedBooksById(id);

            if (books == null)
            {
                return NotFound("There is no record for this user");
            }

            return Ok(books);
        }



        [HttpPost(), AllowAnonymous]
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

        [HttpGet("report")]
        public IActionResult GetBorrowedBooksReport()
        {
            var BorrowedBooks = _borrowedBooksRepository.GetBorrowedBooksReport();

            return Ok(BorrowedBooks);
        }




        [HttpDelete("{ordernumber}")]

        public IActionResult DeleteBorrowedBook(int ordernumber)
        {
            var book = _borrowedBooksRepository.GetBorrowedBookByOrderNum(ordernumber);
            if (book == null)
            {
                return NotFound("Borrowed book with order number " + ordernumber + " not found.");
            }
            _borrowedBooksRepository.RemoveBorrowedBook(book);
            return Ok("Order number: " + ordernumber + " has been deleted successfully");

        }


        [HttpPut("update")]
        public IActionResult UpdateBorrowedBooks(BorrowedBookUpdateDto borrowedBookUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _borrowedBooksRepository.UpdateBorrowedBooks(borrowedBookUpdateDto);

            return Ok(result);

        }




    }
}
