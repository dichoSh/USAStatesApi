using Microsoft.Extensions.Options;
using RestClients.Dtos;
using RestClients.Interfaces;
using RestClients.Models;
using RestClients.Options;
using RestSharp;

namespace RestClients
{
    public class USACountiesClient(IOptions<USACountiesClientOptions> clientOptions) : IUSACountiesClient
    {
        public async Task<IEnumerable<County>> GetCounties(CancellationToken ct)
        {
            var options = new RestClientOptions(clientOptions.Value.BaseUrl);

            using var client = new RestClient(options);

            var request = new RestRequest(clientOptions.Value.Get);

            request.AddParameter("where", clientOptions.Value.Where);
            request.AddParameter("outFields", string.Join(',', clientOptions.Value.OutFields));
            request.AddParameter("returnGeometry", clientOptions.Value.ReturnGeometry);
            request.AddParameter("f", clientOptions.Value.F);

            var response = await client.GetAsync<USACountyResponse>(request, ct);

            return response?.Counties?.Select(x => new County(x.County)) ?? [];
        }
    }
}
