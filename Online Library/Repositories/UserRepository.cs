using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Online_Library.Data;
using Online_Library.DTOS;
using Online_Library.Interfaces;
using Online_Library.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Online_Library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineLibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public UserRepository(OnlineLibraryContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
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
            user.PasswordHash = passwordHash;
            user.PassordSalt = passwordSalt;
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

        public string CreateToken(User user)
        {
            string Role;
            if (user.IsAdmin is true)
            {
                Role = "Admin";
            }
            else
            {
                Role = "User";
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.Role,Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public IEnumerable<object> GetUsers()
        {
            var users = _context.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.DateOfBirth,
                u.Email,
                u.IsAdmin,
                u.IsAccepted
            }).ToList();
            return users;
        }

        public User GetUserByEmail(UserlLoginDto user)
        {
            string email = user.Email;
            var existingUser = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            return existingUser;
        }

        public string GenerateBooksReport()
        {
            throw new NotImplementedException();
        }

        public string GenerateBorrowedBooksReport()
        {
            throw new NotImplementedException();
        }


        public User GetUserByName(ModifyUserDTO user)
        {
            string username = user.UserName;
            var existingUser = _context.Users.Where(u => u.UserName == username).FirstOrDefault();
            return existingUser;
        }

        public void MakeUser(User user)
        {
            if (user != null)
            {
                user.IsAdmin = false;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }
    }
}