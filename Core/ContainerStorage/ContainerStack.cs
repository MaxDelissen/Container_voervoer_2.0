using Core.Enums;

namespace Core.ContainerStorage;

public class ContainerStack
{
    public ContainerStack(ShipSide position, int leftRightIndex)
    {
        Position = position;
        LeftRightIndex = leftRightIndex;
    }

    public List<Container> Containers { get; } = new();
    public int LeftRightIndex { get; private set; }

    public ShipSide Position { get; private set; }

    public int CalculateTotalWeight()
    {
        return Containers.Sum(c => c.Weight);
    }

    public bool TryAddContainer(Container container, ContainerStack? nextStack, ContainerStack? previousStack)
    {
        bool nextHasValuable = nextStack != null && nextStack.HasValueble();
        bool previousHasValuable = previousStack != null && previousStack.HasValueble();
        if (nextHasValuable && nextStack.Containers.Count - 1 <= Containers.Count)
        {
            return false;
        }
        if (previousHasValuable && previousStack.Containers.Count - 1 <= Containers.Count)
        {
            return false;
        }

        int newContainerWeight = container.Weight;
        if (WillBeOverWeight(newContainerWeight))
        {
            return false;
        }

        Containers.Add(container);
        return true;
    }

    private bool HasValueble()
    {
        return Containers.Any(container => container.Type == ContainerType.Valuable);
    }

    private bool WillBeOverWeight(int newContainerWeight)
    {
        int maxWeight = CalculateMaxWeight();
        int totalWeight = Containers.Sum(c => c.Weight);
        return totalWeight + newContainerWeight > maxWeight;
    }

    private int CalculateMaxWeight()
    {
        var maxWeightOnTop = 120;

        var bottomContainerWeight = 0;
        try
        {
            //Get second container's weight, as the first one will be moved to the top at the end.
            bottomContainerWeight = Containers.Count >= 2 ? Containers[1].Weight : 0;
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e);
        }

        return bottomContainerWeight + maxWeightOnTop;
    }

    public bool IsFull() => CalculateTotalWeight() >= CalculateMaxWeight();

    public bool MoveBottomContainerToTop()
    {
        if (Containers.Count() != 0)
        {
            Container bottomContainer = Containers[0];
            Containers.RemoveAt(0);
            Containers.Add(bottomContainer);
        }
        return true;
    }
}