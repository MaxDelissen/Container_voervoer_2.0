using Core.Enums;

namespace Core;

public class ShipValidation(Ship ship)
{
    public SortResult? CheckProblems()
    {
        if (IsOverWeight())
            return SortResult.OverWeight;
        if (IsUnderWeight())
            return SortResult.UnderWeight;
        if (HasTooManyCooledValuableContainers())
            return SortResult.TooManyCooledValuableContainers;
        if (HasTooManyCooledContainers())
            return SortResult.TooManyCooledContainers;

        return null;
    }

    private bool IsOverWeight()
    {
        int totalWeight = ship.ContainersToSort.Sum(c => c.Weight);
        return totalWeight > ship.MaxWeight;
    }

    private bool IsUnderWeight()
    {
        int totalWeight = ship.ContainersToSort.Sum(c => c.Weight);
        return totalWeight < ship.MinWeight;
    }

    private bool HasTooManyCooledValuableContainers()
    {
        int cooledValuableContainers = ship.ContainersToSort.Count(c => c.Type == ContainerType.ValuableCooled);
        return cooledValuableContainers > ship.Width;
    }

    private bool HasTooManyCooledContainers()
    {
        int cooledTotalWeight = ship.ContainersToSort.Where(c => c.Type == ContainerType.Cooled).Sum(c => c.Weight);
        int weightPerStack = cooledTotalWeight / ship.Width;
        return weightPerStack > 150;
    }
}