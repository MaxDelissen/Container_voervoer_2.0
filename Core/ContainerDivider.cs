using Core.ContainerStorage;
using Core.Enums;

namespace Core;

public class ContainerDivider
{
    public (List<Container> valuableCooledContainers, List<Container> valuableContainers, List<Container> cooledContainers, List<Container> normalContainers) DivideContainers(List<Container> containers)
    {
        List<Container> valuableCooledContainers = new List<Container>();
        List<Container> valuableContainers = new List<Container>();
        List<Container> cooledContainers = new List<Container>();
        List<Container> normalContainers = new List<Container>();

        foreach (Container container in containers)
        {
            switch (container.Type)
            {
                case ContainerType.ValuableCooled:
                    valuableCooledContainers.Add(container);
                    break;
                case ContainerType.Valuable:
                    valuableContainers.Add(container);
                    break;
                case ContainerType.Cooled:
                    cooledContainers.Add(container);
                    break;
                case ContainerType.Normal:
                    normalContainers.Add(container);
                    break;
            }
        }

        return (valuableCooledContainers, valuableContainers, cooledContainers, normalContainers);
    }
}