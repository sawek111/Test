using System.Text.Json.Serialization;

namespace PowerPlantOptimizerApi.Domain;

public record Fuels
{
    [JsonPropertyName("gas(euro/MWh)")]
    public double Gas { get; init; }
    
    [JsonPropertyName("kerosine(euro/MWh)")]
    public double Kerosine { get; init; }
    
    [JsonPropertyName("co2(euro/ton)")]
    public double Co2 { get; init; }
    
    [JsonPropertyName("wind(%)")]
    public int Wind { get; init; }
}