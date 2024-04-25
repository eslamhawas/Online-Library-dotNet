using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Library.Interfaces;
using Online_Library.Models;

namespace Online_Library.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BookController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;

        public BookController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        [HttpGet("Book"), AllowAnonymous]
        public IActionResult GetAllBooks()
        {
            var books = _booksRepository.GetAllBooks();
            return Ok(books);

        }

        [HttpPut("Book")]
        public IActionResult UpdateBook(Book updatedBook)
        {
            var existingBook = _booksRepository.GetByIsbn(updatedBook.Isbn);

            if (existingBook == null)
            {
                return NotFound("There is no book With this ISBN: " + updatedBook.Isbn);
            }

            existingBook.Isbn = updatedBook.Isbn;
            existingBook.Title = updatedBook.Title;
            existingBook.RackNumber = updatedBook.RackNumber;
            existingBook.Category = updatedBook.Category;
            existingBook.Price = updatedBook.Price;
            existingBook.StockNumber = updatedBook.StockNumber;

            _booksRepository.UpdateBook(existingBook);

            return Ok();
        }


        [HttpDelete("Book/{Isbn}")]
        public IActionResult DeleteBook(string Isbn)
        {
            var book = _booksRepository.GetByIsbn(Isbn);
            if (book == null)
            {
                return NotFound("No Book With this ISBN");
            }

            _booksRepository.DeleteBook(book);
            return NoContent();
        }

        [HttpPost("Books")]
        public IActionResult AddBook(Book book)
        {
            var isbn = _booksRepository.AddBook(book);
            return CreatedAtAction(nameof(GetBookbyISBN), new { isbn }, null);
        }



        [HttpGet("{isbn}"), AllowAnonymous]
        public IActionResult GetBookbyISBN(string isbn)
        {

            var book = _booksRepository.GetByIsbn(isbn);
            if (book == null)
            {
                return NotFound("The book with this ISBN: " + isbn + " Not Found");
            }

            return Ok(book);


        }
    }
}
