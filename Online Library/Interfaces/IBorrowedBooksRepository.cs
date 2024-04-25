using Online_Library.DTOS;
using Online_Library.Models;

namespace Online_Library.Interfaces
{
    public interface IBorrowedBooksRepository
    {
        IEnumerable<object> GetBorrowedBooks();
        IEnumerable<object> GetBorrowedBooksById(int UserId);
        void RemoveBorrowedBook(BorrowedBook book);
        void AddBorrowedBook(AddBorrowedBookDto Book);
        BorrowedBook GetBorrowedBookByOrderNum(int OrderNumber);
        string GetBorrowedBooksReport();
        string UpdateBorrowedBooks(BorrowedBookUpdateDto borrowedBookUpdateDto);


    }
}
