namespace Online_Library.DTOS
{
    public class UserDTO
    {

        public string? UserName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsAccepted { get; set; }
        public string? Password { get; set; }
    }
}
