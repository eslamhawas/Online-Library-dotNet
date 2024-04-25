using Microsoft.EntityFrameworkCore;
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
            _context = context;
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetAllBooks() => _context.Books.ToList();


        public Book GetByIsbn(string ISBN) => _context.Books.Where(x => x.Isbn == ISBN).FirstOrDefault();

        public void UpdateBook(Book book)
        {
            _context.ChangeTracker.DetectChanges();
            _context.Entry(book).State = EntityState.Detached;
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public string AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();

            return book.Isbn;
        }
    }
}
