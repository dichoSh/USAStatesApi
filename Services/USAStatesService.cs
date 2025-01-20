using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Dto;
using Services.Interfaces;

namespace Services
{
    public class USAStatesService(EsriDbContext dbContext, TimeProvider timeProvider) : IUSAStatesService
    {
        public async Task<IEnumerable<State>> AddOrUpdateStates(IEnumerable<BaseStateInfoDto> statesInfo, Action<State, BaseStateInfoDto> updateAction, CancellationToken ct)
        {
            if (statesInfo is null || !statesInfo.Any())
            {
                return [];
            }

            var statesToUpdate = await dbContext.States
                            .Where(x => statesInfo.Select(i => i.Name).Contains(x.Name))
                            .ToListAsync(ct);

            foreach (var stateInfo in statesInfo)
            {
                var state = statesToUpdate.FirstOrDefault(x => x.Name.Equals(stateInfo.Name, StringComparison.InvariantCultureIgnoreCase));

                if (state == null)
                {
                    state = new()
                    {
                        Name = stateInfo.Name
                    };
                    statesToUpdate.Add(state);
                }

                state.LastUpdatedWhen = timeProvider.GetUtcNow().DateTime;

                updateAction?.Invoke(state, stateInfo);
            }

            dbContext.States.UpdateRange(statesToUpdate);
            await dbContext.SaveChangesAsync(ct);

            return statesToUpdate;
        }

        public async Task<IEnumerable<StateInformation>> GetStates(string? stateName, CancellationToken ct)
        {
            var states = dbContext.States.AsNoTracking();

            if (!string.IsNullOrEmpty(stateName))
            {
                states = states.Where(x => x.Name.ToLower() == stateName.ToLower());
            }

            return await states.Select(x => new StateInformation(x)).ToListAsync(ct);
        }
    }
}
