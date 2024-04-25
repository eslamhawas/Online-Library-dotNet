using System.ComponentModel.DataAnnotations;

namespace Online_Library.DTOS
{
    public class BorrowedBookUpdateDto
    {
        [Required]
        public int OrderNumber { get; set; }
        [Required]
        public bool IsAccepted { get; set; }
        public DateTime? DateOfReturn { get; set; }
    }
}
