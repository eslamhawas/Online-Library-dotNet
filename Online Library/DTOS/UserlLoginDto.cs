using System.ComponentModel.DataAnnotations;

namespace Online_Library.DTOS
{
    public class UserlLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
