using System.ComponentModel.DataAnnotations.Schema;

namespace Memoriesx.Models.Dto
{
    public class LoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
