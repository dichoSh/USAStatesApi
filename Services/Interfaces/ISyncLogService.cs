using DataAccess.Models;

namespace Services.Interfaces
{
    public interface ISyncLogService
    {
        SyncLog Start();
        Task<SyncLog> Finish(SyncLog log, string message, int countiesSynced = default, CancellationToken ct = default);
    }
}
