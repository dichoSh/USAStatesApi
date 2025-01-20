using RestClients.Dtos;

namespace RestClients.Models
{
    public class County
    {
        public string StateName { get; set; } = string.Empty;
        public int Population { get; set; } = default;

        public County()
        {
        }

        internal County(CountyDto dto)
        {
            StateName = dto.StateName ?? string.Empty;
            Population = dto.Population ?? default;
        }
    }
}
