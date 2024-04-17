using Online_Library.DTOS;
using Online_Library.Models;

namespace Online_Library.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAcceptedUsers();
        IEnumerable<User> GetPendingUsers();

        void Register(UserRegisterDto user);

        void Reject(User user);

        User GetById(int Id);

        void Accept(User user);

        void MakeLibrarian(User user);

        IEnumerable<object> GetUsers();

        string GenerateBooksReport();

        string GenerateBorrowedBooksReport();

        User GetUserByEmail(UserlLoginDto user);

        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        string CreateToken(User user);

        User GetUserByName(ModifyUserDTO user);

        void MakeUser(User user);




    }
}
