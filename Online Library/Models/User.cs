using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Library.Models
{
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [DefaultValue(false)]
        public bool? IsAdmin { get; set; }

        [DefaultValue(null)]
        public bool? IsAccepted { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PassordSalt { get; set; }
        public string encryptionkey { get; set; }
        public string IVKey { get; set; }
    }
}
