using Microsoft.EntityFrameworkCore;
using Online_Library.Data;
using Online_Library.Interfaces;
using Online_Library.Models;

namespace Online_Library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineLibraryContext _context;
        public UserRepository(OnlineLibraryContext context)
        {
            _context = context;
        }

        public void accept(User user)
        {
            if (user != null)
            {
                user.IsAccepted = true;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public void Add(User user)
        {
            bool anyUsers = _context.Users.Any();
            if (!anyUsers) 
            {
                user.IsAccepted = true;
                user.IsAdmin = true;
            }
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.OrderBy(e=>e.Id).ToList();
        }

        public User GetById(int Id)
        {
            var user = _context.Users.Where(e => e.Id == Id).FirstOrDefault();
            return user;
        }

        public void MakeLibrarian(User user)
        {
            if (user != null)
            {
                user.IsAdmin = true;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }
    }
}