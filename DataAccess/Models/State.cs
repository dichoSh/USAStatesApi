using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        public int Population { get; set; }

        [Required]
        public DateTime LastUpdatedWhen { get; set; }
    }
}
