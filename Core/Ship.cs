namespace Core;

public class Ship
{
    // List of containers to be sorted
    public readonly List<Container> ContainersToSort = new();

    // Constructor for the Ship class
    public Ship(int width, int length)
    {
        Width = width;
        Length = length;
        MaxWeight = CalculateMaxWeight();
        MinWeight = MaxWeight / 2; //50% of the max weight needs to be loaded to avoid tipping over
    }

    // Properties for the ship's dimensions and weight limits
    public int Width { get; } // Width of the ship (left to right)
    public int Length { get; } // Length of the ship (front to back)
    public int MaxWeight { get; } // Maximum weight the ship can carry
    public int MinWeight { get; } // Minimum weight the ship needs to carry to avoid tipping over

    // List of containers that have been sorted
    public List<Container> SortedContainers { get; private set; } = new();

    // Method to calculate the maximum weight the ship can carry
    private int CalculateMaxWeight()
    {
        const int maxWeightPerStack = 150;

        int maxWeight = maxWeightPerStack * Width * Length;

        return maxWeight;
    }

    public int ContainerToSortCount() => ContainersToSort.Count;

    // Method to add a single container to the ship
    public bool AddContainer(Container container)
    {
        if (container.Weight > 30)
            return false;

        ContainersToSort.Add(container);
        return true;
    }

    // Method to add multiple containers to the ship
    public bool AddContainers(List<Container> containers)
    {
        if (containers.Any(c => c.Weight > 30))
            return false;

        ContainersToSort.AddRange(containers);
        return true;
    }

    public bool IsOverWeight()
    {
        int totalWeight = ContainersToSort.Sum(c => c.Weight);
        return totalWeight > MaxWeight;
    }

    public bool IsUnderWeight()
    {
        int totalWeight = ContainersToSort.Sum(c => c.Weight);
        return totalWeight < MinWeight;
    }

    public bool HasTooManyCooledValuableContainers()
    {
        int cooledValuableContainers = ContainersToSort.Count(c => c.Type == ContainerType.ValuableCooled);
        return cooledValuableContainers > Width;
    }

    public bool HasTooManyCooledContainers()
    {
        int cooledTotalWeight = ContainersToSort.Where(c => c.Type == ContainerType.Cooled).Sum(c => c.Weight);
        int weightPerStack = cooledTotalWeight / Width;
        return weightPerStack > 150;
    }

    // Method to sort the containers on the ship
    public List<ContainerRow> SortContainers()
    {
        // Sorting containers by type and weight
        List<Container> cooledContainers = ContainersToSort
            .Where(c => c.Type == ContainerType.Cooled)
            .OrderByDescending(c => c.Weight)
            .ToList();

        List<Container> valuableContainers = ContainersToSort
            .Where(c => c.Type == ContainerType.Valuable)
            .OrderByDescending(c => c.Weight)
            .ToList();

        List<Container> valuableCooledContainers = ContainersToSort
            .Where(c => c.Type == ContainerType.ValuableCooled)
            .OrderByDescending(c => c.Weight)
            .ToList();

        List<Container> normalContainers = ContainersToSort
            .Where(c => c.Type == ContainerType.Normal)
            .OrderByDescending(c => c.Weight)
            .ToList();

        // Creating rows on the ship to place the containers in.
        List<ContainerRow> rows = new List<ContainerRow>();
        for (int i = 0; i < Length; i++)
        {
            rows.Add(new ContainerRow(Width));
        }

        // Placing valuable cooled containers on the first row
        if (valuableCooledContainers.Any())
            rows[0].MakeValuableRow(valuableCooledContainers);

        // Placing valuable containers on the ship, in a 1,2,skip pattern
        if (valuableContainers.Any())
        {
            int valuableIndex = 1;
            while (valuableIndex < Length)
            {
                var containersToAdd = valuableContainers.Take(Width).ToList();
                rows[valuableIndex].MakeValuableRow(containersToAdd);
                valuableContainers.RemoveRange(0, Width);
                valuableIndex++;
                if (valuableIndex + 1 % 3 == 0)
                {
                    valuableIndex++;
                }
            }
        }

        int CalculateWeight(ShipSide side)
        {
            int totalWeight = 0;
            foreach (var containerRow in rows)
            {
                totalWeight += containerRow.Stacks.Where(s => s.Position == side).Sum(s => s.CalculateTotalWeight());
            }
            return totalWeight;
        }

        int totalLeftWeight;
        int totalRightWeight;

        // Placing cooled containers on the first row
        if (cooledContainers.Any())
        {
            foreach (var container in cooledContainers)
            {
                totalLeftWeight = CalculateWeight(ShipSide.Left);
                totalRightWeight = CalculateWeight(ShipSide.Right);
                rows[0].TryAddContainer(container, totalLeftWeight, totalRightWeight);
            }
        }

        // Placing normal containers, on top of the valuable containers, these will later be flipped.
        if (normalContainers.Any())
        {
            foreach (var container in normalContainers)
            {
                foreach (var row in rows)
                {
                    totalLeftWeight = CalculateWeight(ShipSide.Left);
                    totalRightWeight = CalculateWeight(ShipSide.Right);
                    if (row.TryAddContainer(container, totalLeftWeight, totalRightWeight))
                    {
                        break;
                    }
                }
            }
        }

        // Moving the bottom containers to the top.
        rows = MoveBottomContainersToTop(rows);

        return rows;
    }

    // Method to move the bottom containers to the top, this because the placing order is reversed.
    private List<ContainerRow> MoveBottomContainersToTop(List<ContainerRow> rows)
    {
        foreach (var row in rows)
        {
            row.MoveBottomContainersToTop();
        }
        return rows;
    }
}