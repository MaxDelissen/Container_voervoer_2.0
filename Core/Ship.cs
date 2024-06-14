namespace Core;

public class Ship
{
    // List of containers to be sorted
    private readonly List<Container> containersToSort = new();

    public List<ContainerRow> SortedRows
    {
        get => sortedRows ?? throw new NotSortedException("The rows are not sorted.");
        private set => sortedRows = value;
    }

    private List<ContainerRow>? sortedRows;


    public int AmountOfSortedContainers => (SortedRows).Sum(r => r.Stacks.Sum(s => s.Containers.Count));

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


    // Method to calculate the maximum weight the ship can carry
    private int CalculateMaxWeight()
    {
        const int maxWeightPerStack = 150;

        int maxWeight = maxWeightPerStack * Width * Length;

        return maxWeight;
    }

    public int ContainerToSortCount() => containersToSort.Count;

    // Method to add a single container to the ship
    public bool AddContainer(Container container)
    {
        if (container.Weight > 30)
            return false;

        containersToSort.Add(container);
        return true;
    }

    // Method to add multiple containers to the ship
    public bool AddContainers(List<Container> containers)
    {
        if (containers.Any(c => c.Weight > 30))
            return false;

        containersToSort.AddRange(containers);
        return true;
    }

    private bool IsOverWeight()
    {
        int totalWeight = containersToSort.Sum(c => c.Weight);
        return totalWeight > MaxWeight;
    }

    private bool IsUnderWeight()
    {
        int totalWeight = containersToSort.Sum(c => c.Weight);
        return totalWeight < MinWeight;
    }

    private bool HasTooManyCooledValuableContainers()
    {
        int cooledValuableContainers = containersToSort.Count(c => c.Type == ContainerType.ValuableCooled);
        return cooledValuableContainers > Width;
    }

    private bool HasTooManyCooledContainers()
    {
        int cooledTotalWeight = containersToSort.Where(c => c.Type == ContainerType.Cooled).Sum(c => c.Weight);
        int weightPerStack = cooledTotalWeight / Width;
        return weightPerStack > 150;
    }
    
    private List<Container> FailedContainers { get; set; } = new();

    // Method to sort the containers on the ship
    public SortResult SortContainers()
    {
        if (IsOverWeight())
            return SortResult.OverWeight;
        if (IsUnderWeight())
            return SortResult.UnderWeight;
        if (HasTooManyCooledValuableContainers())
            return SortResult.TooManyCooledValuableContainers;
        if (HasTooManyCooledContainers())
            return SortResult.TooManyCooledContainers;
        
        
        // Sorting containers by type and weight
        List<Container> cooledContainers = containersToSort
            .Where(c => c.Type == ContainerType.Cooled)
            .OrderByDescending(c => c.Weight)
            .ToList();

        List<Container> valuableContainers = containersToSort
            .Where(c => c.Type == ContainerType.Valuable)
            .OrderByDescending(c => c.Weight)
            .ToList();

        List<Container> valuableCooledContainers = containersToSort
            .Where(c => c.Type == ContainerType.ValuableCooled)
            .OrderByDescending(c => c.Weight)
            .ToList();

        List<Container> normalContainers = containersToSort
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
                if (((float)valuableIndex + 1) % 3 == 0)
                {
                    valuableIndex++;
                }
            }
            FailedContainers.AddRange(valuableContainers);
        }

        // Placing cooled containers on the first row
        if (cooledContainers.Any())
        {
            foreach (var container in cooledContainers)
            {
                rows[0].TryAddContainer(container);
            }
        }

        // Placing normal containers, on top of the valuable containers, these will later be flipped.
        if (normalContainers.Any())
        {
            foreach (var container in normalContainers)
            {
                foreach (var row in rows)
                {
                    if (row.TryAddContainer(container))
                    {
                        break;
                    }
                }
                FailedContainers.Add(container);
            }
        }

        // Moving the bottom containers to the top.
        rows = MoveBottomContainersToTop(rows);
        
        SortedRows = rows;

        if (GetTotalFailedContainers().Count > 0)
            return SortResult.SuccesWithFailedContainers;
        return SortResult.Success;
    }
    
    public List<Container> GetTotalFailedContainers()
    {
        var failedContainers = new List<Container>();
        foreach (var row in SortedRows)
        {
            failedContainers.AddRange(row.FailedContainers);
        }
        failedContainers.AddRange(this.FailedContainers);
        return failedContainers;
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

    public List<Container> GetTotalPlacedContainers()
    {
        var placedContainers = new List<Container>();
        foreach (var row in SortedRows)
        {
            foreach (var stack in row.Stacks)
            {
                placedContainers.AddRange(stack.Containers);
            }
        }
        return placedContainers;
    }
}