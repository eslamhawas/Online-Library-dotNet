using System;
using System.Collections.Generic;

namespace Online_Library.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsAccepted { get; set; }
        public string? Password { get; set; }
    }
}
