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

    public bool TryAddContainer(Container container)
    {
        int newContainerWeight = container.Weight;
        if (WillBeOverWeight(newContainerWeight))
        {
            return false;
        }

        Containers.Add(container);
        return true;
    }

    private bool WillBeOverWeight(int newContainerWeight)
    {
        int maxWeight = CalculateMaxWeight();
        int totalWeight = Containers.Sum(c => c.Weight);
        return totalWeight + newContainerWeight > maxWeight;
    }

    private int CalculateMaxWeight()
    {
        int maxWeightOnTop = 120;

        int bottomContainerWeight = 0;
        try
        {
            //Get second container's weight, as the first one will be moved to the top at the end.
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
            var bottomContainer = Containers[0];
            Containers.RemoveAt(0);
            Containers.Add(bottomContainer);
        }
        return true;
    }
}