using Online_Library.Data;
using Online_Library.Interfaces;
using Online_Library.Models;

namespace Online_Library.Repositories
{
    public class BookRepository : IBooksRepository
    {
        private readonly OnlineLibraryContext _context;

        public BookRepository(OnlineLibraryContext context)
        {
            _context =context;
        }
        public void AddBook(Book book)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook(int ISBN)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(Book book)
        {
            throw new NotImplementedException();
        }
    }
}
