using System.Data;
using PowerPlantOptimizerApi.Domain;

namespace PowerPlantOptimizerApi.Application;

/// <summary>
/// Unfortunately I skipped fact that I cannot create to much power, too late realised about it to handle it in 4hrs
/// </summary>
public class Optimizer
{
    public static PowerPlantOutput[] Calculate(Fuels fuels, int load, IEnumerable<PowerPlant> powerPlants)
    {
        var sortedPlants = powerPlants
            .Select(plant => (Plant: plant, UnitCost: CalculateCostPerMWh(plant, fuels)))
            .OrderBy(pair => pair.UnitCost)
            .Select(pair => pair.Plant)
            .ToList();

        var (output, leftLoad) = OptimizedAlghorithm(sortedPlants, load, fuels.Wind);

        if (leftLoad > 0)
        {
            throw new Exception("Left Load Is Different than 0,");
        }

        return output;
    }

    private static (PowerPlantOutput[] Output, int LeftLoad) OptimizedAlghorithm(List<PowerPlant> sortedPlantsByCost, int load, int wind)
    {
        var result = new List<PowerPlantOutput>();

        foreach (var plant in sortedPlantsByCost)
        {
            var productionRation = plant.Type == PlantType.Windturbine ? wind / 100f : 1.0f;
            var powerProduced = plant.Pmax * productionRation; 

            if (load <= 0.0f)
            {
                break;
            }

            var canUseFullPower = powerProduced <= load;
            if (canUseFullPower)
            {
                result.Add(new PowerPlantOutput(plant.Name, powerProduced));
                load -= plant.Pmax;
                continue;
            }
            if (plant.Pmin > load)
            {
                result.Add(new PowerPlantOutput(plant.Name, plant.Pmin));
                load = -plant.Pmin;
                return (result.ToArray(), load);
            }
            
            result.Add(new PowerPlantOutput(plant.Name, load));
        }

        return (result.ToArray(), load);
    }

    private static float CalculateCostPerMWh(PowerPlant plant, Fuels fuels)
    {
        return plant.Type switch
        {
            PlantType.Gasfired => (float)fuels.Gas + (float)fuels.Co2 * plant.Efficiency / 100,
            PlantType.Turbojet => (float)fuels.Kerosine + (float)fuels.Co2 * plant.Efficiency / 100,
            PlantType.Windturbine => fuels.Wind > 0 ? 0f : float.MaxValue,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}