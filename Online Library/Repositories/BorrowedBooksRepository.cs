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
        public BorrowedBooksRepository(OnlineLibraryContext context) 
        {
            _context = context;
        }
        public void AddBorrowedBook(BorrowedBook Book)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetBorrowedBooks()
        {
            var returnedBooks = _context.BorrowedBooks.Join(
             _context.Books,
             borrowedBook => borrowedBook.BookIsbn,
             book => book.Isbn,
             (borrowedBook, book) => new BorrowedBookDto
             {
                 DateOfReturn = borrowedBook.DateOfReturn,
                 OrderNumber = (int)borrowedBook.OrderNumber,
                 IsAccepted = borrowedBook.IsAccepted,
                 BookIsbn = borrowedBook.BookIsbn,
                 UserId = borrowedBook.UserId,
                 BookTitle = book.Title
             }
                )
                .ToList();
            return returnedBooks;
        }

        public IEnumerable<object> GetBorrowedBooksById(int UserId)
        {
            var returnedBooks = _context.BorrowedBooks.Where(u=>u.UserId==UserId).Join(
             _context.Books,
             borrowedBook => borrowedBook.BookIsbn,
             book => book.Isbn,
             (borrowedBook, book) => new BorrowedBookDto
             {
                 DateOfReturn = borrowedBook.DateOfReturn,
                 OrderNumber = (int)borrowedBook.OrderNumber,
                 IsAccepted = borrowedBook.IsAccepted,
                 BookIsbn = borrowedBook.BookIsbn,
                 UserId = borrowedBook.UserId,
                 BookTitle = book.Title
             }
                )
                .ToList();
            return returnedBooks;
        }

        public void RemoveBorrowedBook(int OrderNumber)
        {
            throw new NotImplementedException();
        }

        public void UpdateBorrowedBook(int OrderNumber, bool state)
        {
           
                var borrowedBook = _context.BorrowedBooks.FirstOrDefault(x => x.OrderNumber == OrderNumber);
                borrowedBook.IsAccepted = state;
                _context.BorrowedBooks.Update(borrowedBook);
                _context.SaveChanges();

        }
    }
}
