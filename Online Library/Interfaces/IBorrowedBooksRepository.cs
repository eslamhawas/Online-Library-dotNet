using Online_Library.DTOS;

namespace Online_Library.Interfaces
{
    public interface IBorrowedBooksRepository
    {
        IEnumerable<object> GetBorrowedBooks();
        IEnumerable<object> GetBorrowedBooksById(int UserId);
        void UpdateBorrowedBook(int OrderNumber, bool state); // set the date of return
        void AddBorrowedBook(AddBorrowedBookDto Book); // DateOfReturn = null
        void RemoveBorrowedBook(int OrderNumber);




    }
}
