using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Online_Library.Data;
using Online_Library.DTOS;
using Online_Library.Interfaces;
using Online_Library.Models;

namespace Online_Library.Repositories
{
    public class BorrowedBooksRepository : IBorrowedBooksRepository
    {
        private readonly OnlineLibraryContext _context;
        private readonly IMapper _mapper;
        private static Random random = new Random();
        public BorrowedBooksRepository(OnlineLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }




        public IEnumerable<BorrowedBookDto> GetBorrowedBooks()
        {
            var returnedBooks = _context.BorrowedBooks
              .Join(
                _context.Books, // No lambda needed here (automatic include)
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
                _context.Users,
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
              .ToList();

            return returnedBooks;
        }






        public IEnumerable<BorrowedBookDto> GetBorrowedBooksById(int UserId)
        {
            var returnedBooks = _context.BorrowedBooks.Where(u => u.UserId == UserId)
       .Join(
         _context.Books, // No lambda needed here (automatic include)
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
         _context.Users,
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
       .ToList();

            return returnedBooks;
        }






        public void AddBorrowedBook(AddBorrowedBookDto BookDto)
        {

            var Book = _mapper.Map<BorrowedBook>(BookDto);

            Book.IsAccepted = null;
            Book.DateOfReturn = null;
            Book.OrderNumber = GetRandomNumber(10, 100000);

            _context.BorrowedBooks.Add(Book);
            _context.SaveChanges();

        }



        public void RemoveBorrowedBook(BorrowedBook book)
        {
            _context.BorrowedBooks.Remove(book);
            _context.SaveChanges();
        }

        public BorrowedBook GetBorrowedBookByOrderNum(int OrderNumber)
        {
            var borrowedBook = _context.BorrowedBooks.FirstOrDefault(x => x.OrderNumber == OrderNumber);

            return borrowedBook;
        }

        public string GetBorrowedBooksReport()
        {
            int TotalBorrowedBooks = _context.BorrowedBooks.Where(x => x.IsAccepted == true && x.IsAccepted != null).Count();

            int TotalUsers = _context.BorrowedBooks.Where(x => x.IsAccepted == true && x.IsAccepted != null)
                .Select(x => x.UserId).Distinct().Count();

            string MostBorrowedBook = _context.BorrowedBooks.Where(x => x.IsAccepted == true && x.IsAccepted != null)
                .GroupBy(x => x.BookIsbn).OrderByDescending(b => b.Count()).Select(b => b.FirstOrDefault()
                .BookIsbnNavigation.Title).FirstOrDefault();

            string leastBorrowedBooks = _context.BorrowedBooks.Where(x => x.IsAccepted == true && x.IsAccepted != null)
                .GroupBy(x => x.BookIsbn).OrderBy(b => b.Count()).Select(b => b.FirstOrDefault().BookIsbnNavigation.Title).FirstOrDefault();

            return ("Total Borrowed Books is :" + TotalBorrowedBooks + "\n Total Users number is :" + TotalUsers +
                "\n The Most Borrowed Book is :" + MostBorrowedBook + "\n The Least Borrowed Book is :" + leastBorrowedBooks);
        }



        private int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max); // Generates a random number between min (inclusive) and max (exclusive)
        }

        public string UpdateBorrowedBooks(BorrowedBookUpdateDto borrowedBookUpdateDto)
        {
             
            var borrowedBook = _context.BorrowedBooks.FirstOrDefault(x => x.OrderNumber == borrowedBookUpdateDto.OrderNumber);

            
            borrowedBook.IsAccepted = borrowedBookUpdateDto.IsAccepted;
            
            if (borrowedBookUpdateDto.IsAccepted)
            {
                borrowedBook.DateOfReturn = borrowedBookUpdateDto.DateOfReturn;
                var book = _context.Books.Where(x => x.Isbn == borrowedBook.BookIsbn).FirstOrDefault();
                book.StockNumber -= 1;
                _context.Books.Update(book);
                _context.SaveChanges();
            }
            
            _context.BorrowedBooks.Update(borrowedBook);
            _context.SaveChanges();

            return "updated successfully";
                
          
        }
    }
}
