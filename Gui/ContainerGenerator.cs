using Core.Enums;
using Core.ContainerStorage;

namespace Gui;

internal class ContainerGenerator
{
    private int GenerateRandomWeight()
    {
        Random random = new Random();
        return random.Next(4, 30);
    }

    public List<Container> GenerateRandomContainers(int valuableCooled, int valuable, int cooled, int normal)
    {
        List<Container> containers = new List<Container>();
        for (int i = 0; i < valuableCooled; i++)
            containers.Add(new(ContainerType.ValuableCooled, GenerateRandomWeight()));
        for (int i = 0; i < valuable; i++)
            containers.Add(new(ContainerType.Valuable, GenerateRandomWeight()));
        for (int i = 0; i < cooled; i++)
            containers.Add(new(ContainerType.Cooled, GenerateRandomWeight()));
        for (int i = 0; i < normal; i++)
            containers.Add(new(ContainerType.Normal, GenerateRandomWeight()));
        
        return containers;
    }
    
    public List<Container> GenerateContainersWithWeight(
        int amountOfCVContainers,
        int cVWeight,
        int amountOfVContainers,
        int vWeight,
        int amountOfCContainers,
        int cWeight,
        int amountOfNContainers,
        int nWeight)
    {
        List<Container> containers = new List<Container>();
        for (int i = 0; i < amountOfCVContainers; i++)
            containers.Add(new(ContainerType.ValuableCooled, cVWeight));
        for (int i = 0; i < amountOfVContainers; i++)
            containers.Add(new(ContainerType.Valuable, vWeight));
        for (int i = 0; i < amountOfCContainers; i++)
            containers.Add(new(ContainerType.Cooled, cWeight));
        for (int i = 0; i < amountOfNContainers; i++)
            containers.Add(new(ContainerType.Normal, nWeight));
        
        return containers;
    }
}