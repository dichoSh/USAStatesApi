using DataAccess.Models;

namespace Services.Interfaces
{
    public interface ISyncService
    {
        Task<SyncLog> SyncStatesPopulation(CancellationToken ct);
    }
}
