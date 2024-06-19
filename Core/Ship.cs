using Core.ContainerStorage;
using Core.Enums;
using Exceptions;

namespace Core;

public class Ship
{
    #region Properties

    // List of containers to be sorted
    private readonly List<Container> containersToSort = new();
    public List<Container> ContainersToSort => containersToSort;
    private List<ContainerRow>? sortedRows;
    
    private readonly ShipValidation validator;
    private readonly ContainerDivider divider = new();

    public List<ContainerRow> SortedRows
    {
        get => sortedRows ?? throw new NotSortedException();
        private set => sortedRows = value;
    }
    public int Width { get; } // Width of the ship (left to right)
    public int Length { get; } // Length of the ship (front to back)
    public int MaxWeight { get; }
    public int MinWeight { get; }
    private List<Container> FailedContainers { get; } = new();

    #endregion

    public Ship(int width, int length)
    {
        Width = width;
        Length = length;
        MaxWeight = CalculateMaxWeight();
        MinWeight = MaxWeight / 2; // 50% of the max weight needs to be loaded to avoid tipping over
        validator = new ShipValidation(this);
    }
        
    private int CalculateMaxWeight()
    {
        const int maxWeightPerStack = 150;
        int maxWeight = maxWeightPerStack * Width * Length;
        return maxWeight;
    }

    #region Container Management

    public void AddContainers(List<Container> containers)
    {
        if (containers.Any(c => c.Weight > 30))
            throw new ContainerOverWeightException();

        containersToSort.AddRange(containers);
    }

    public SortResult SortContainers()
    {
        var error = validator.CheckProblems();
        if (error != null)
            return error.Value;

        //Splitting the containers into different categories using the divider class.
        //ValuableCooled, Valuable, Cooled, Normal
        var (valuableCooledContainers, valuableContainers, cooledContainers, normalContainers) =
            divider.DivideContainers(containersToSort);

        // Creating rows on the ship to place the containers in.
        List<ContainerRow> rows = new List<ContainerRow>();
        for (int i = 0; i < Length; i++)
        {
            rows.Add(new ContainerRow(Width));
        }

        // Placing valuable cooled containers on the first row
        if (valuableCooledContainers.Any())
        {
            ContainerRow? nextRow = Length > 1 ? rows[1] : null;
            rows[0].MakeValuableRow(valuableCooledContainers, null, nextRow);
        }

        // Placing valuable containers on the ship, in a 1,2,skip pattern
        if (valuableContainers.Any())
        {
            var valuableContainersRemaining = PlaceValuableContainers(valuableContainers, rows);
            FailedContainers.AddRange(valuableContainersRemaining);
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
                PlaceNormalContainer(rows, container);
            }
        }
        
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

    #endregion

    #region Container Placement

    private List<Container> PlaceValuableContainers(List<Container> valuableContainers, List<ContainerRow> rows)
    {
        int valuableIndex = 1;
        while (valuableIndex < Length)
        {
            var containersToAdd = valuableContainers.Take(Width).ToList();
            rows[valuableIndex].MakeValuableRow(containersToAdd, rows[valuableIndex - 1], valuableIndex + 1 < Length ? rows[valuableIndex + 1] : null);
            valuableContainers.RemoveRange(0, Width);
            valuableIndex++;
            if (((float)valuableIndex + 1) % 3 == 0)
            {
                valuableIndex++;
            }
        }
        return valuableContainers;
    }

    private void PlaceNormalContainer(List<ContainerRow> rows, Container container)
    {
        var lightestSortRows = rows.OrderBy(r => r.CalculateTotalWeight()).ToList();
        foreach (var row in lightestSortRows)
        {
            if (row.TryAddContainer(container))
            {
                return;
            }
        }
        FailedContainers.Add(container);
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

    #endregion
}