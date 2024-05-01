using System.ComponentModel.DataAnnotations;

namespace Online_Library.DTOS
{
    public class UserRegisterDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; }
        [Required]
        public string RePassword { get; set; }
    }
}
