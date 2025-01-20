using DataAccess;
using DataAccess.Models;
using RestClients.Interfaces;
using RestClients.Models;
using Services.Dto;
using Services.Interfaces;
using System.Data;

namespace Services
{
    public class SyncService(IUSACountiesClient countiesClient,
        IUSAStatesService usaStatesService,
        ISyncLogService syncLogService) : ISyncService
    {
        public async Task<SyncLog> SyncStatesPopulation(CancellationToken ct)
        {
            var log = syncLogService.Start();
            var counties = await GetCounties(log, ct);

            if (!counties.Any())
            {
                return await syncLogService.Finish(log, "Nothing to sync", ct: ct);
            }

            var statesSummedInfo = counties.Where(c => !string.IsNullOrWhiteSpace(c.StateName))
                            .AggregateBy(x => x.StateName, 0, (sum, c) => sum + c.Population)
                            .Select(x => new PopulationInfoDto { Name = x.Key, Population = x.Value });

            return await SaveCounties(statesSummedInfo, log, ct);
        }

        private async Task<IEnumerable<County>> GetCounties(SyncLog log, CancellationToken ct)
        {
            try
            {
                return await countiesClient.GetCounties(ct);
            }
            catch (Exception ex)
            {
                await syncLogService.Finish(log, $"Error getting counties from {nameof(IUSACountiesClient)}: {ex.Message} - {ex.StackTrace}", ct: ct);
                throw;
            }
        }

        private async Task<SyncLog> SaveCounties(IEnumerable<PopulationInfoDto> statesSummedInfo, SyncLog log, CancellationToken ct)
        {
            try
            {
                var updatedStates = await usaStatesService.AddOrUpdateStates(statesSummedInfo, UpdateState, ct);
                return await syncLogService.Finish(log, "Success", updatedStates.Count(), ct);
            }
            catch (Exception ex)
            {
                await syncLogService.Finish(log, $"Error saving counties in {nameof(EsriDbContext)}: {ex.Message} - {ex.StackTrace}", ct: ct);
                throw;
            }
        }

        private void UpdateState(State state, BaseStateInfoDto stateInfo)
        {
            if (stateInfo is PopulationInfoDto info)
            {
                state.Population = info.Population;
            }
        }
    }
}
