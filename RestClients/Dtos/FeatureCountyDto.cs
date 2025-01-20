using System.Text.Json.Serialization;

namespace RestClients.Dtos
{
    internal class FeatureCountyDto
    {
        [JsonPropertyName("attributes")]
        public required CountyDto County { get; set; }
    }
}
