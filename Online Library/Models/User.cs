using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Online_Library.Models
{
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }

        [DefaultValue(false)]
        public bool? IsAdmin { get; set; }
        
        [DefaultValue(null)]
        public bool? IsAccepted { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
