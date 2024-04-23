using System.ComponentModel.DataAnnotations;

namespace Online_Library.DTOS
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        public string Password { get; set; }

        public string RePassword { get; set; }
    }
}
