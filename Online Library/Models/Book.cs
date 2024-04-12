using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Online_Library.Models
{
    public partial class Book
    {
        
        public string Isbn { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Category { get; set; }
        public string? RackNumber { get; set; }

        [Range(0, 100)]
        public int Price { get; set; }
        public int? StockNumber { get; set; }
    }
}
