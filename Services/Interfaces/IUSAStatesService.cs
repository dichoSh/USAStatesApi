using DataAccess.Models;
using Services.Dto;

namespace Services.Interfaces
{
    public interface IUSAStatesService
    {
        Task<IEnumerable<State>> AddOrUpdateStates(IEnumerable<BaseStateInfoDto> statesInfo, Action<State, BaseStateInfoDto> updateAction, CancellationToken ct);
        Task<IEnumerable<StateInformation>> GetStates(string? stateName, CancellationToken ct);
    }
}
