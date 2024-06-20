using Core.ContainerStorage;
using Core.Enums;

namespace Core;

public class ContainerDivider
{
    public (List<Container> valuableCooledContainers, List<Container> valuableContainers, List<Container> cooledContainers, List<Container> normalContainers) DivideContainers(
        List<Container> containers)
    {
        var valuableCooledContainers = new List<Container>();
        var valuableContainers = new List<Container>();
        var cooledContainers = new List<Container>();
        var normalContainers = new List<Container>();

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