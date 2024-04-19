using System.ComponentModel.DataAnnotations;

namespace Online_Library.DTOS
{
    public class AddBorrowedBookDto
    {
        [Required]
        public string? BookIsbn { get; set; }
        [Required]
        public int? UserId { get; set; }
    }
}
