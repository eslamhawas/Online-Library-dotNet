using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Online_Library.Data;
using Online_Library.DTOS;
using Online_Library.Interfaces;
using Online_Library.Models;
using System.Security.Cryptography;

namespace Online_Library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineLibraryContext _context;
        private readonly IMapper _mapper;
        public UserRepository(OnlineLibraryContext context,IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
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

        public void Register(UserRegisterDto userDto)
        {
            CheckForExistingUsers(userDto);
            var user = _mapper.Map<User>(userDto);
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash= passwordHash;
            user.PassordSalt= passwordSalt;
            // auto accept new user and make them admin if there is no users in DB
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
            return _context.Users.Where(u => u.IsAccepted == true).OrderBy(e => e.Id).ToList();
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

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private void CheckForExistingUsers(UserRegisterDto user)
        {
            string userName = user.UserName;
            string email = user.Email;
            var existingUser = _context.Users.Where(e => e.UserName == userName).FirstOrDefault();

            if (_context.Users.Any(u => u.UserName == userName || u.Email == email))
            {
                bool? status = existingUser.IsAccepted;
                if (status == false)
                {
                    throw new ArgumentException("This user was already rejected," +
                                                 "Call the librarian for more info");
                }
                else
                    throw new ArgumentException("Username or email already exists.");
            }
        }


    }
}