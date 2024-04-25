using Online_Library.Models;

namespace Online_Library.Interfaces
{
    public interface IBooksRepository
    {
        void UpdateBook(Book book);
        void DeleteBook(Book book);
        string AddBook(Book book);
        IEnumerable<Book> GetAllBooks();
        Book GetByIsbn(string ISBN);



    }
}
