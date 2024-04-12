using Online_Library.Models;

namespace Online_Library.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        void Add(User user);

        void Delete(User user);

        User GetById(int Id);

        void accept(User user);

        void MakeLibrarian(User user);




    }
}
