using Online_Library.Models;

namespace Online_Library.DTOS
{
    public class TokenID
    {
        public string jwt { get; set; }

        public Users user { get; set; }
    }
}
