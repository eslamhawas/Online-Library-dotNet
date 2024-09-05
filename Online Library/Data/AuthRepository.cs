using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Online_Library.DTOS;
using Online_Library.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Online_Library.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly OnlineLibraryContext _context;
        public AuthRepository(IMapper mapper, IConfiguration configuration, OnlineLibraryContext context)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }
        public string CreateToken(Users user)
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
            if (user.IsSuperAdmin is true)
            {
                Role = "SuperAdmin";
            }
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role,Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public void Register(UserRegisterDto userDto)
        {
            var user = _mapper.Map<Users>(userDto);
            user.IsAccepted = null;
            user.IsAdmin = false;
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PassordSalt = passwordSalt;
            // auto accept new user and make them admin if there is no users in DB
            //bool anyUsers = _context.Users.Any();
            //if (!anyUsers)
            //{
            user.IsAccepted = true;
            user.IsAdmin = true;
            //}

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public Users CheckForExistingUsers(UserRegisterDto user)
        {
            string userName = user.UserName;
            string email = user.Email;
            var existingUser = _context.Users.Where(e => e.UserName == userName || e.Email == email).FirstOrDefault();

            return existingUser;
        }

        public Users GetUserByEmail(UserLoginDto user)
        {
            string email = user.Email;
            var existingUser = _context.Users.Where(u => u.Email == email).FirstOrDefault();

            return existingUser;
        }


    }
}
