using System.Text.Json.Serialization;

namespace RestClients.Dtos
{
    internal class USACountyResponse
    {
        [JsonPropertyName("features")]
        public required List<FeatureCountyDto> Counties { get; set; }

    }
}
