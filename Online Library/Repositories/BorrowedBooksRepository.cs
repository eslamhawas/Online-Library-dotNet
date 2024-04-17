using Online_Library.Data;
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

        public IEnumerable<BorrowedBook> GetBorrowedBooks()
        {
            var books = _context.BorrowedBooks.ToList();

            return books;
        }

        public IEnumerable<BorrowedBook> GetBorrowedBooksById(int UserId)
        {
            throw new NotImplementedException();
        }

        public void RemoveBorrowedBook(int OrderNumber)
        {
            throw new NotImplementedException();
        }

        public void UpdateBorrowedBook(int OrderNumber, int state)
        {
            throw new NotImplementedException();
        }
    }
}
