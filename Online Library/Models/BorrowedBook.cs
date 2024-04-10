using System;
using System.Collections.Generic;

namespace Online_Library.Models
{
    public partial class BorrowedBook
    {
        public DateTime? DateOfReturn { get; set; }
        public int? OrderNumber { get; set; }
        public bool? IsAccepted { get; set; }
        public string? BookIsbn { get; set; }
        public int? UserId { get; set; }

        public virtual Book? BookIsbnNavigation { get; set; }
        public virtual User? User { get; set; }
    }
}
