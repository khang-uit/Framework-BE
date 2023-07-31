using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Memoriesx.Models
{
    public class View
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? Count { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
    }
}
