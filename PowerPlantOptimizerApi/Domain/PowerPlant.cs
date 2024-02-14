namespace PowerPlantOptimizerApi.Domain;

public record PowerPlant(
    string Name,
    PlantType Type,
    float Efficiency,
    int Pmin,
    int Pmax
);