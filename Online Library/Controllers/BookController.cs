using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Library.Data;
using Online_Library.Models;

namespace Online_Library.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BookController : ControllerBase
    {
        private readonly IDataRepository<Book> _repo;

        public BookController(IDataRepository<Book> repo)
        {
            _repo = repo;
        }

        [HttpGet("Book"), AllowAnonymous]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _repo.GetAllAsync();
            if (books is null)
            {
                return NotFound("There is no books in DB");
            }
            return Ok(books);

        }

        [HttpPut("Book")]
        public async Task<IActionResult> UpdateBook(Book updatedBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingBook = await _repo.GetByIdAsync(updatedBook.Isbn);

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

            _repo.Update(existingBook);
            await _repo.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("Book/{Isbn}")]
        public async Task<IActionResult> DeleteBook(string Isbn)
        {
            var book = await _repo.GetByIdAsync(Isbn);
            if (book == null)
            {
                return NotFound("No Book With this ISBN");
            }

            _repo.Delete(book);
            await _repo.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("Books")]
        public async Task<IActionResult> AddBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repo.Insert(book);
            await _repo.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookbyISBN), new { book.Isbn }, null);
        }



        [HttpGet("{isbn}"), AllowAnonymous]
        public async Task<IActionResult> GetBookbyISBN(string isbn)
        {

            var book = await _repo.GetByIdAsync(isbn);
            if (book == null)
            {
                return NotFound("The book with this ISBN: " + isbn + " Not Found");
            }

            return Ok(book);


        }



        [HttpGet("report")]
        public async Task<IActionResult> GetBooksReport()
        {


            var totalBooksInStock = await _repo.GetQueryable().SumAsync(b => b.StockNumber);

            var mostStockedBook = _repo.GetQueryable()
                .OrderByDescending(b => b.StockNumber)
                .FirstOrDefault();

            string report;

            if (mostStockedBook != null)
            {
                report = $"Total Books in Stock: {totalBooksInStock}," +
                    $"\n Most Stocked Book: {mostStockedBook.Title}," +
                    $"\n Category: {mostStockedBook.Category}," +
                    $"\n Stock Number: {mostStockedBook.StockNumber}";
            }
            else
            {
                report = "No books found.";
            }


            return Ok(report);
        }
    }
}
