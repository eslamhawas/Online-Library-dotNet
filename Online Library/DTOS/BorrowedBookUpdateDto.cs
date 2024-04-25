using System.ComponentModel.DataAnnotations;

namespace Online_Library.DTOS
{
    public class BorrowedBookUpdateDto
    {
        [Required]
        public int OrderNumber { get; set; }
        [Required]
        public bool IsAccepted { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfReturn { get; set; }
    }
}
