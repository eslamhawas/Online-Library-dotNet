using Online_Library.Models;

namespace Online_Library.Interfaces
{
    public interface IBorrowedBooksRepository
    {
        IEnumerable<BorrowedBook> GetBorrowedBooks();
        IEnumerable<BorrowedBook> GetBorrowedBooksById(int UserId);
        void UpdateBorrowedBook(int OrderNumber,int state); // set the date of return
        void AddBorrowedBook(BorrowedBook Book); // DateOfReturn = null
        void RemoveBorrowedBook(int OrderNumber);




    }
}
