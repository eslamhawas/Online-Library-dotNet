using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Library.Data;
using Online_Library.DTOS;
using Online_Library.Models;

namespace Online_Library.Controllers
{
    [Route("api/v1/borrowedBooks")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BorrowedBooksController : ControllerBase
    {
        private readonly IDataRepository<BorrowedBook> _borrowedbooksrepo;
        private readonly IDataRepository<Users> _userrepo;
        private readonly IDataRepository<Book> _bookrepo;
        private readonly IMapper _mapper;
        private static Random random = new Random();

        public BorrowedBooksController(IDataRepository<BorrowedBook> repo,
            IMapper mapper,
            IDataRepository<Users> userrepo,
            IDataRepository<Book> bookrepo)
        {

            _borrowedbooksrepo = repo;
            _mapper = mapper;
            _userrepo = userrepo;
            _bookrepo = bookrepo;
        }

        [HttpGet]

        public async Task<IActionResult> GetBorrowedBooks()
        {
            var returnedBooks = await _borrowedbooksrepo.GetQueryable()
            .Join(
            _bookrepo.GetQueryable(), // No lambda needed here (automatic include)
                borrowedBook => borrowedBook.BookIsbn,
                book => book.Isbn,
                (borrowedBook, book) => new BorrowedBookDto
                {
                    DateOfReturn = borrowedBook.DateOfReturn,
                    OrderNumber = (int)borrowedBook.OrderNumber,
                    IsAccepted = borrowedBook.IsAccepted,
                    BookIsbn = borrowedBook.BookIsbn,
                    UserId = borrowedBook.UserId,
                    BookTitle = book.Title,
                    Price = book.Price,
                    UserName = null
                }
            )
            .Join(
                _userrepo.GetQueryable(),
                borrowedBookDto => borrowedBookDto.UserId,
                user => user.Id,
                (borrowedBookDto, user) =>
                  new BorrowedBookDto
                  {
                      DateOfReturn = borrowedBookDto.DateOfReturn,
                      OrderNumber = borrowedBookDto.OrderNumber,
                      IsAccepted = borrowedBookDto.IsAccepted,
                      BookIsbn = borrowedBookDto.BookIsbn,
                      UserId = borrowedBookDto.UserId,
                      BookTitle = borrowedBookDto.BookTitle,
                      Price = borrowedBookDto.Price,
                      UserName = user.UserName
                  }
              )
              .ToListAsync();

            return Ok(returnedBooks);
        }



        [HttpGet("{id}"), AllowAnonymous]

        public async Task<IActionResult> GetBorrowedBookByID(int id)
        {
            var returnedBooks = await _borrowedbooksrepo.GetQueryable().Where(u => u.UserId == id)
      .Join(
        _bookrepo.GetQueryable(), // No lambda needed here (automatic include)
        borrowedBook => borrowedBook.BookIsbn,
        book => book.Isbn,
        (borrowedBook, book) => new BorrowedBookDto
        {
            DateOfReturn = borrowedBook.DateOfReturn,
            OrderNumber = (int)borrowedBook.OrderNumber,
            IsAccepted = borrowedBook.IsAccepted,
            BookIsbn = borrowedBook.BookIsbn,
            UserId = borrowedBook.UserId,
            BookTitle = book.Title,
            Price = book.Price,
            UserName = null // Assuming you'll populate this later
        }
      )
      .Join(
        _userrepo.GetQueryable(),
        borrowedBookDto => borrowedBookDto.UserId,
        user => user.Id,
        (borrowedBookDto, user) =>  // Simplified lambda
          new BorrowedBookDto // Create a new instance
          {
              DateOfReturn = borrowedBookDto.DateOfReturn,
              OrderNumber = borrowedBookDto.OrderNumber,
              IsAccepted = borrowedBookDto.IsAccepted,
              BookIsbn = borrowedBookDto.BookIsbn,
              UserId = borrowedBookDto.UserId,
              BookTitle = borrowedBookDto.BookTitle,
              Price = borrowedBookDto.Price,
              UserName = user.UserName
          }
      )
      .ToListAsync();



            if (returnedBooks == null)
            {
                return NotFound("There is no record for this user");
            }

            return Ok(returnedBooks);
        }



        [HttpPost(), AllowAnonymous]
        public async Task<IActionResult> AddBorrowedBook(AddBorrowedBookDto borrowedBook)
        {

            if (borrowedBook == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var Book = _mapper.Map<BorrowedBook>(borrowedBook);

            Book.IsAccepted = null;
            Book.DateOfReturn = null;
            Book.OrderNumber = GetRandomNumber(10, 100000);

            _borrowedbooksrepo.Insert(Book);
            await _borrowedbooksrepo.SaveChangesAsync();

            return Ok();



        }

        [HttpGet("report")]
        public async Task<IActionResult> GetBorrowedBooksReport()
        {

            int TotalBorrowedBooks = await _borrowedbooksrepo.GetQueryable().Where(x => x.IsAccepted == true && x.IsAccepted != null).CountAsync();

            int TotalUsers = await _borrowedbooksrepo.GetQueryable().Where(x => x.IsAccepted == true && x.IsAccepted != null)
                .Select(x => x.UserId).Distinct().CountAsync();

            string MostBorrowedBook = await _borrowedbooksrepo.GetQueryable().Where(x => x.IsAccepted == true && x.IsAccepted != null)
                .GroupBy(x => x.BookIsbn).OrderByDescending(b => b.Count()).Select(b => b.FirstOrDefault()
                .BookIsbnNavigation.Title).FirstOrDefaultAsync();

            string leastBorrowedBooks = await _borrowedbooksrepo.GetQueryable().Where(x => x.IsAccepted == true && x.IsAccepted != null)
                .GroupBy(x => x.BookIsbn).OrderBy(b => b.Count()).Select(b => b.FirstOrDefault().BookIsbnNavigation.Title).FirstOrDefaultAsync();

            return Ok("Total Borrowed Books is :" + TotalBorrowedBooks + "\n Total Users number is :" + TotalUsers +
                "\n The Most Borrowed Book is :" + MostBorrowedBook + "\n The Least Borrowed Book is :" + leastBorrowedBooks);

        }




        [HttpDelete("{ordernumber}")]

        public async Task<IActionResult> DeleteBorrowedBook(int ordernumber)
        {
            var book = await _borrowedbooksrepo.GetByIdAsync(ordernumber);
            if (book == null)
            {
                return NotFound("Borrowed book with order number " + ordernumber + " not found.");
            }
            _borrowedbooksrepo.Delete(book);
            await _borrowedbooksrepo.SaveChangesAsync();
            return Ok("Order number: " + ordernumber + " has been deleted successfully");

        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateBorrowedBooks(BorrowedBookUpdateDto borrowedBookUpdateDto)
        {
            var borrowedBook = await _borrowedbooksrepo.GetByIdAsync(borrowedBookUpdateDto.OrderNumber);
            if (borrowedBook == null)
            {
                return BadRequest("There is no record with this order number");
            }

            borrowedBook.IsAccepted = borrowedBookUpdateDto.IsAccepted;

            if (borrowedBookUpdateDto.IsAccepted)
            {
                borrowedBook.DateOfReturn = borrowedBookUpdateDto.DateOfReturn;
                var book = await _bookrepo.GetByIdAsync(borrowedBook.BookIsbn);
                book.StockNumber -= 1;
                _bookrepo.Update(book);
                await _borrowedbooksrepo.SaveChangesAsync();
            }

            _borrowedbooksrepo.Update(borrowedBook);
            await _borrowedbooksrepo.SaveChangesAsync();

            return Ok("updated successfully");

        }


        private int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max); // Generates a random number between min (inclusive) and max (exclusive)
        }

    }
}
