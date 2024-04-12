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

        public void Accept(User user)
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
            string userName = user.UserName;
            string email = user.Email;
            var existingUser = _context.Users.Where(e => e.UserName == userName).FirstOrDefault();
            bool? status = existingUser.IsAccepted;
            if (_context.Users.Any(u => u.UserName == userName || u.Email == email))
            {
                if(status == false)
                {
                    throw new ArgumentException("This user was already rejected," +
                                                 "Call the librarian for more info");
                } else
                    throw new ArgumentException("Username or email already exists.");
            }
            bool anyUsers = _context.Users.Any();
            if (!anyUsers) 
            {
                user.IsAccepted = true;
                user.IsAdmin = true;
            }
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Reject(User user)
        {
            
            if (user != null)
            {
                user.IsAccepted = false;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }
        public IEnumerable<User> GetPendingUsers()
        {
            return _context.Users.Where(u => u.IsAccepted == null).OrderBy(e => e.Id).ToList();
        }
        public IEnumerable<User> GetAcceptedUsers()
        {
            return _context.Users.Where(u => u.IsAccepted == true).OrderBy(e=>e.Id).ToList();
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