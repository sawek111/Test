using System.Text.Json.Serialization;

namespace PowerPlantOptimizerApi.Domain;

[JsonConverter(typeof(JsonStringEnumConverter<PlantType>))]
public enum PlantType
{
    Turbojet,
    Windturbine,
    Gasfired,
}