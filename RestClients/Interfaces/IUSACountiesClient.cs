using RestClients.Models;

namespace RestClients.Interfaces
{
    public interface IUSACountiesClient
    {
        Task<IEnumerable<County>> GetCounties(CancellationToken ct);
    }
}
