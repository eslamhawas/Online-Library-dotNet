using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Online_Library.Data;
using Online_Library.DTOS;
using Online_Library.Interfaces;
using Online_Library.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

            var user = _mapper.Map<User>(userDto);
            user.IsAccepted = null;
            user.IsAdmin = false;
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
            string encryptionkey = GenerateKey();
            string iv;
            user.UserName = Encrypt(user.UserName, encryptionkey, out iv);
            user.encryptionkey = encryptionkey;
            user.IVKey = iv;
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
            var pendingUsers = _context.Users.Where(u => u.IsAccepted == null).OrderBy(e => e.Id).ToList();

            foreach (var user in pendingUsers)
            {
                string decryptedUsername = Decrypt(user.UserName, user.encryptionkey, user.IVKey);
                user.UserName = decryptedUsername;
            }

            return pendingUsers;
        }
        public IEnumerable<User> GetAcceptedUsers()
        {
            var acceptedUsers = _context.Users.Where(u => u.IsAccepted == true).OrderBy(e => e.Id).ToList();

            foreach (var user in acceptedUsers)
            {
                string decryptedUsername = Decrypt(user.UserName, user.encryptionkey, user.IVKey);
                user.UserName = decryptedUsername;
            }

            return acceptedUsers;
        }

        public UserDto GetById(int Id)
        {
            var user = _context.Users.Where(e => e.Id == Id).FirstOrDefault();

            if (user != null)
            {
                string decryptedUsername = Decrypt(user.UserName, user.encryptionkey, user.IVKey);
                user.UserName = decryptedUsername;
            }
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
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
        public User CheckForExistingUsers(UserRegisterDto user)
        {
            string userName = user.UserName;
            string email = user.Email;
            var existingUser = _context.Users.Where(e => e.UserName == userName || e.Email == email).FirstOrDefault();

            return existingUser;
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
        public string GenerateKey()
        {
            string KeyBase64 = "";
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                KeyBase64 = Convert.ToBase64String(aes.Key);
            }
            return KeyBase64;
        }
        public string Encrypt(string Plaintext, string Key, out string IVKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                aes.Key = Convert.FromBase64String(Key);
                aes.GenerateIV();
                IVKey = Convert.ToBase64String(aes.IV);
                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] encryptedData;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(Plaintext);
                        }
                        encryptedData = ms.ToArray();
                    }
                }
                return Convert.ToBase64String(encryptedData);
            }
        }
        public string Decrypt(string ciphertext, string Key, string IVKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.Zeros;
                aes.Key = Convert.FromBase64String(Key);
                aes.IV = Convert.FromBase64String(IVKey);

                ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] cipher = Convert.FromBase64String(ciphertext);

                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {

                        byte[] decryptedBytes = new byte[cipher.Length];
                        int decryptedByteCount = cs.Read(decryptedBytes, 0, decryptedBytes.Length);


                        string plainText = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedByteCount).TrimEnd('\0');

                        return plainText;
                    }
                }
            }
        }


        private string GetUserEncryptionKey(int userId)
        {

            var userEncryptionKey = _context.Users
                .Where(uek => uek.Id == userId)
                .Select(uek => uek.encryptionkey)
                .FirstOrDefault();

            return userEncryptionKey;
        }

        public IEnumerable<object> GetUsers()
        {
            List<object> decryptedUsers = new List<object>();

            var usersWithKeys = _context.Users
                .Where(u => u.encryptionkey != null && u.IVKey != null)
                .ToList();

            foreach (var user in usersWithKeys)
            {
                string decryptionKey = GetUserEncryptionKey(user.Id);
                string decryptedUserName = Decrypt(user.UserName, decryptionKey, user.IVKey);


                var decryptedUser = new
                {
                    user.Id,
                    UserName = decryptedUserName,
                    user.DateOfBirth,
                    user.Email,
                    user.IsAdmin,
                    user.IsAccepted

                };

                decryptedUsers.Add(decryptedUser);
            }

            return decryptedUsers;

        }

        public User GetUserByEmail(UserlLoginDto user)
        {
            string email = user.Email;
            var existingUser = _context.Users.Where(u => u.Email == email).FirstOrDefault();

            if (existingUser != null)
            {
                string decryptedUsername = Decrypt(existingUser.UserName, existingUser.encryptionkey, existingUser.IVKey);
                existingUser.UserName = decryptedUsername;
            }

            return existingUser;
        }


        public User GetUserByID(ModifyUserDTO user)
        {
            int ID = user.userid;
            var existingUser = _context.Users.Where(u => u.Id == ID).FirstOrDefault();
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