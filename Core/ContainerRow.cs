namespace Core;

public class ContainerRow
{
    private const decimal MaxWeightDifferencePercentage = 0.2m;
    public readonly List<ContainerStack> Stacks = new();

    private bool IsFull { get; set; }

    public bool TryAddContainer(Container container, int totalLeftWeight, int totalRightWeight)
    {
        if (IsFull)
            return false;

        // Sort the stacks in ascending order based on their total weight
        var sortedStacks = Stacks.OrderBy(stack => stack.CalculateTotalWeight()).ToList();

        foreach (var stack in sortedStacks)
        {
            /*if (IsWeightDifferenceAcceptable(container, stack.Position, totalLeftWeight, totalRightWeight))
            {*/
            if (stack.TryAddContainer(container))
            {
                UpdateFullStatus();
                return true;
            }
            /*}*/
        }

        return false;
    }

    public bool MakeValuableRow(List<Container> valuableContainers)
    {
        if (IsFull)
            return false;

        bool containsWrongType = valuableContainers.Any(container =>
            container.Type != ContainerType.Valuable && container.Type != ContainerType.ValuableCooled);
        if (containsWrongType)
            throw new ArgumentException("Not all containers are of type Valuable");

        if (valuableContainers.Count > Stacks.Count)
            throw new ArgumentException(
            $"Too many valuable containers for this row, only add the ships width amount of valuable containers (= {Stacks.Count})");

        foreach (var stack in Stacks)
        {
            bool result = stack.TryAddContainer(valuableContainers[0]);
            if (!result)
                return false;

            valuableContainers.RemoveAt(0);
        }

        if (valuableContainers.Count > 0)
            throw new Exception(
            "Not all valuable containers could be added to the row, this should not happen, and should have been caught earlier");

        UpdateFullStatus();
        return true;
    }

    private void UpdateFullStatus()
    {
        IsFull = Stacks.All(stack => stack.IsFull());
    }

    [Obsolete("This method is not used anymore, and should be removed")]
    private bool IsWeightDifferenceAcceptable(Container container, ShipSide side, int leftWeight, int rightWeight)
    {
        if (side == ShipSide.Left)
        {
            leftWeight += container.Weight;
        }
        else if (side == ShipSide.Right)
        {
            rightWeight += container.Weight;
        }
        int totalWeight = leftWeight + rightWeight;

        int weightDifference = Math.Abs(leftWeight - rightWeight);
        double maxAllowedDifference = totalWeight * (double)MaxWeightDifferencePercentage;

        return weightDifference <= maxAllowedDifference;
    }

    [Obsolete("This method is not used anymore, and should be removed")]
    private int CalculateSideWeight(ShipSide side)
    {
        int totalWeight = 0;
        foreach (var stack in Stacks)
        {
            if (stack.Position == side)
            {
                totalWeight += stack.CalculateTotalWeight();
            }
        }
        return totalWeight;
    }

    public bool MoveBottomContainersToTop()
    {
        foreach (var stack in Stacks)
        {
            bool success = stack.MoveBottomContainerToTop();
            if (!success)
                return false;
        }
        return true;
    }

    #region Init

    public ContainerRow(int shipWidth)
    {
        bool hasCenterStack = shipWidth % 2 != 0;
        if (hasCenterStack)
        {
            Stacks.Add(new ContainerStack(ShipSide.Center));
            shipWidth--;
            AddStacks(shipWidth);
        }
        else
        {
            AddStacks(shipWidth);
        }
    }

    private void AddStacks(int shipWidth)
    {
        for (int i = 0; i < shipWidth / 2; i++)
        {
            Stacks.Add(new ContainerStack(ShipSide.Left));
            Stacks.Add(new ContainerStack(ShipSide.Right));
        }
    }

    #endregion
}