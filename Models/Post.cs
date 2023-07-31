using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Memoriesx.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Name { get; set; }
        public int? CreatorId { get; set; }
        public string? SelectedFile{ get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedAt { get; set; } = System.DateTime.Now;
        public ICollection<Like>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Report>? Reports { get; set; }
    }
}
