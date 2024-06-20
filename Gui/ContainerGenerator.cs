using Core.ContainerStorage;
using Core.Enums;

namespace Gui;

internal class ContainerGenerator
{
    private int GenerateRandomWeight()
    {
        var random = new Random();
        return random.Next(4, 30);
    }

    public List<Container> GenerateRandomContainers(int valuableCooled, int valuable, int cooled, int normal)
    {
        var containers = new List<Container>();
        for (var i = 0; i < valuableCooled; i++)
        {
            containers.Add(new Container(ContainerType.ValuableCooled, GenerateRandomWeight()));
        }
        for (var i = 0; i < valuable; i++)
        {
            containers.Add(new Container(ContainerType.Valuable, GenerateRandomWeight()));
        }
        for (var i = 0; i < cooled; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, GenerateRandomWeight()));
        }
        for (var i = 0; i < normal; i++)
        {
            containers.Add(new Container(ContainerType.Normal, GenerateRandomWeight()));
        }

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
        var containers = new List<Container>();
        for (var i = 0; i < amountOfCVContainers; i++)
        {
            containers.Add(new Container(ContainerType.ValuableCooled, cVWeight));
        }
        for (var i = 0; i < amountOfVContainers; i++)
        {
            containers.Add(new Container(ContainerType.Valuable, vWeight));
        }
        for (var i = 0; i < amountOfCContainers; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, cWeight));
        }
        for (var i = 0; i < amountOfNContainers; i++)
        {
            containers.Add(new Container(ContainerType.Normal, nWeight));
        }

        return containers;
    }
}