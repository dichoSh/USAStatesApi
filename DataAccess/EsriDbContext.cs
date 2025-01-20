using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class EsriDbContext : DbContext
    {
        public DbSet<State> States { get; set; }
        public DbSet<SyncLog> SyncLogs { get; set; }

        public EsriDbContext(DbContextOptions<EsriDbContext> options) : base(options) { }
    }
}
