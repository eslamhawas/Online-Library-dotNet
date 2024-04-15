using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Online_Library.Models
{
    public partial class BorrowedBook
    {
        [DataType(DataType.Date)]
        public DateTime? DateOfReturn { get; set; }
        public int? OrderNumber { get; set; }

        [DefaultValue(null)]
        public bool? IsAccepted { get; set; }
        [Required]
        public string? BookIsbn { get; set; }
        [Required]
        public int? UserId { get; set; }

        public virtual Book? BookIsbnNavigation { get; set; }
        public virtual User? User { get; set; }
    }
}
