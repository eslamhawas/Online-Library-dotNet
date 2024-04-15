using Online_Library.Models;

namespace Online_Library.Interfaces
{
    public interface IBooksRepository
    {
        void UpdateBook(Book book);
        void DeleteBook(int  ISBN);
        void AddBook(Book book);
        IEnumerable<Book> GetAllBooks();

        

    }
}
