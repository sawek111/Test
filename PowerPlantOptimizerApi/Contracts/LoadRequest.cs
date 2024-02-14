namespace PowerPlantOptimizerApi.Domain;

public record LoadRequest(
    int Load,
    Fuels Fuels,
    IList<PowerPlant> Powerplants
);