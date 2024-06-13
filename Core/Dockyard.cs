namespace Core;

public class Dockyard
{
    public bool MakeShipGetVisualizerLink(int width, int length, List<Container> containers, out string? link)
    {
        link = null;
        Ship ship = new Ship(width, length);
        ship.AddContainers(containers);

        var preChecks = new Dictionary<Func<bool>, string>
        {
            {ship.IsOverWeight, "Ship is overweight!"},
            {ship.IsUnderWeight, "Ship is underweight!"},
            {ship.HasTooManyCooledValuableContainers, "Ship has too many cooled valuable containers!"},
            {ship.HasTooManyCooledContainers, "Ship has too many cooled containers!"}
        };

        foreach (var check in preChecks)
        {
            if (check.Key())
            {
                link = check.Value;
                return false;
            }
        }

        List<ContainerRow> sortedContainers = ship.SortContainers();
        link = GenerateLink(sortedContainers, ship);
        
        return true;
    }

    private string GenerateLink(List<ContainerRow> sortedContainers, Ship ship)
    {
        LinkGenerator linkGenerator = new LinkGenerator();
        return linkGenerator.ConvertShipToLink(sortedContainers, ship);
    }
}