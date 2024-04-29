using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Online_Library.Models
{
    public partial class BorrowedBook
    {
        [DataType(DataType.Date)]
        public DateTime? DateOfReturn { get; set; }

        [Key]
        public int? OrderNumber { get; set; }

        [DefaultValue(null)]
        public bool? IsAccepted { get; set; }
        [Required]
        public string? BookIsbn { get; set; }
        [Required]
        public int? UserId { get; set; }
        [JsonIgnore]
        public virtual Book? BookIsbnNavigation { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
