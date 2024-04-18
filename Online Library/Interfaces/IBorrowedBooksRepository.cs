using Online_Library.Models;

namespace Online_Library.Interfaces
{
    public interface IBorrowedBooksRepository
    {
        IEnumerable<object> GetBorrowedBooks();
        IEnumerable<object> GetBorrowedBooksById(int UserId);
        void UpdateBorrowedBook(int OrderNumber,bool state); // set the date of return
        void AddBorrowedBook(BorrowedBook Book); // DateOfReturn = null
        void RemoveBorrowedBook(int OrderNumber);




    }
}
