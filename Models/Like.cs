using System.ComponentModel.DataAnnotations.Schema;

namespace Memoriesx.Models
{
    public class Like
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public Post Post { get; set; }
        public int? PostId { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
    }
}
