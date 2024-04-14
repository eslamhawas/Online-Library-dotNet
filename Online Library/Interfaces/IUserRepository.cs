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

        string Login(UserlLoginDto user);






    }
}
