namespace Online_Library.DTOS
{
    public class BorrowedBookDto
    {
            public DateTime? DateOfReturn { get; set; }
            public int OrderNumber { get; set; } // Change to int (not nullable for primary key)
            public bool? IsAccepted { get; set; }
            public string? BookIsbn { get; set; }
            public int? UserId { get; set; }
            public string? BookTitle { get; set; }
        
    }
}
