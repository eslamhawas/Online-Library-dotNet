using System.ComponentModel.DataAnnotations;

namespace Online_Library.DTOS
{
    public class ModifyUserDTO
    {
        [Required]
        public string UserName { get; set; } = null!;

    }
}
