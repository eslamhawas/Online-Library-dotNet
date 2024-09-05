using Online_Library.DTOS;
using Online_Library.Models;

namespace Online_Library.Data
{
    public interface IAuthRepository
    {
        void Register(UserRegisterDto userDto);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(Users user);
        Users CheckForExistingUsers(UserRegisterDto user);
        public Users GetUserByEmail(UserLoginDto user);

    }
}
