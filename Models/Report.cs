using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Memoriesx.Models
{
    public class Report
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? CreatorId { get; set; }
        public string? Message { get; set; }
        public int? PostId { get; set; }
        public int? ReportedId { get; set; }
        public User Creator { get; set; }
        public User Reported { get; set; }
        public Post Post { get; set; }
    }
}
