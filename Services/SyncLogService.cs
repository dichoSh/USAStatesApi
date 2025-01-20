using DataAccess;
using DataAccess.Models;
using Services.Interfaces;

namespace Services
{
    public class SyncLogService(EsriDbContext dbContext, TimeProvider timeProvider) : ISyncLogService
    {
        public SyncLog Start() =>
            new()
            {
                StartedWhen = timeProvider.GetUtcNow().DateTime,
            };

        public async Task<SyncLog> Finish(SyncLog log, string message, int countiesSynced = default, CancellationToken ct = default)
        {
            log ??= Start();

            log.FinishedWhen = timeProvider.GetUtcNow().DateTime;
            log.Message = message;
            log.CountiesSynced = countiesSynced;

            dbContext.SyncLogs.Update(log);
            await dbContext.SaveChangesAsync(ct);

            return log;
        }
    }
}
