using AutoMapper;
using Online_Library.Data;
using Online_Library.DTOS;
using Online_Library.Interfaces;
using Online_Library.Models;

namespace Online_Library.Repositories
{
    public class BorrowedBooksRepository : IBorrowedBooksRepository
    {
        private readonly OnlineLibraryContext _context;
        private readonly IMapper _mapper;
        private static Random random = new Random();
        public BorrowedBooksRepository(OnlineLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }




        public IEnumerable<object> GetBorrowedBooks()
        {
            var returnedBooks = _context.BorrowedBooks
              .Join(
                _context.Books, // No lambda needed here (automatic include)
                borrowedBook => borrowedBook.BookIsbn,
                book => book.Isbn,
                (borrowedBook, book) => new BorrowedBookDto
                {
                    DateOfReturn = borrowedBook.DateOfReturn,
                    OrderNumber = (int)borrowedBook.OrderNumber,
                    IsAccepted = borrowedBook.IsAccepted,
                    BookIsbn = borrowedBook.BookIsbn,
                    UserId = borrowedBook.UserId,
                    BookTitle = book.Title,
                    Price = book.Price,
                    UserName = null
                }
              )
              .Join(
                _context.Users,
                borrowedBookDto => borrowedBookDto.UserId,
                user => user.Id,
                (borrowedBookDto, user) =>
                  new BorrowedBookDto
                  {
                      DateOfReturn = borrowedBookDto.DateOfReturn,
                      OrderNumber = borrowedBookDto.OrderNumber,
                      IsAccepted = borrowedBookDto.IsAccepted,
                      BookIsbn = borrowedBookDto.BookIsbn,
                      UserId = borrowedBookDto.UserId,
                      BookTitle = borrowedBookDto.BookTitle,
                      Price = borrowedBookDto.Price,
                      UserName = user.UserName
                  }
              )
              .ToList();

            return returnedBooks;
        }






        public IEnumerable<object> GetBorrowedBooksById(int UserId)
        {
            var returnedBooks = _context.BorrowedBooks.Where(u => u.UserId == UserId)
       .Join(
         _context.Books, // No lambda needed here (automatic include)
         borrowedBook => borrowedBook.BookIsbn,
         book => book.Isbn,
         (borrowedBook, book) => new BorrowedBookDto
         {
             DateOfReturn = borrowedBook.DateOfReturn,
             OrderNumber = (int)borrowedBook.OrderNumber,
             IsAccepted = borrowedBook.IsAccepted,
             BookIsbn = borrowedBook.BookIsbn,
             UserId = borrowedBook.UserId,
             BookTitle = book.Title,
             Price = book.Price,
             UserName = null // Assuming you'll populate this later
         }
       )
       .Join(
         _context.Users,
         borrowedBookDto => borrowedBookDto.UserId,
         user => user.Id,
         (borrowedBookDto, user) =>  // Simplified lambda
           new BorrowedBookDto // Create a new instance
           {
               DateOfReturn = borrowedBookDto.DateOfReturn,
               OrderNumber = borrowedBookDto.OrderNumber,
               IsAccepted = borrowedBookDto.IsAccepted,
               BookIsbn = borrowedBookDto.BookIsbn,
               UserId = borrowedBookDto.UserId,
               BookTitle = borrowedBookDto.BookTitle,
               Price = borrowedBookDto.Price,
               UserName = user.UserName
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


        public void AddBorrowedBook(AddBorrowedBookDto BookDto)
        {

            var Book = _mapper.Map<BorrowedBook>(BookDto);

            Book.IsAccepted = null;
            Book.DateOfReturn = null;
            Book.OrderNumber = GetRandomNumber(10, 100000);

            _context.BorrowedBooks.Add(Book);
            _context.SaveChanges();

        }

        private int GetRandomNumber(int min, int max)
        {
            return random.Next(min, max); // Generates a random number between min (inclusive) and max (exclusive)
        }
    }
}
