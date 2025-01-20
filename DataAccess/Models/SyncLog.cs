using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class SyncLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime StartedWhen { get; set; }
        public DateTime FinishedWhen { get; set; }
        public string? Message { get; set; }

        public int CountiesSynced { get; set; }
    }
}
