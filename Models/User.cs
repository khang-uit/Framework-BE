using System.ComponentModel.DataAnnotations.Schema;

namespace Memoriesx.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Report>? Creators { get; set; }
        public ICollection<Report>? Reporteds { get; set; }

    }
}
