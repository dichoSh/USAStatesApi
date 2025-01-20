using System.Text.Json.Serialization;

namespace RestClients.Dtos
{
    internal class CountyDto
    {
        [JsonPropertyName("POPULATION")]
        public int? Population { get; set; }

        [JsonPropertyName("STATE_NAME")]
        public string? StateName { get; set; }
    }
}
