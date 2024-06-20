namespace UnitTests;

public static class TestHelpers
{
    public static Ship GenerateTestShip(int width,
        int length,
        int amountOfCVContainers,
        int amountOfVContainers,
        int amountOfCContainers,
        int amountOfNContainers)
    {
        var ship = new Ship(width, length);
        var containers = new List<Container>();
        for (var i = 0; i < amountOfCVContainers; i++)
        {
            containers.Add(new Container(ContainerType.ValuableCooled, GenerateRandomWeight()));
        }
        for (var i = 0; i < amountOfVContainers; i++)
        {
            containers.Add(new Container(ContainerType.Valuable, GenerateRandomWeight()));
        }
        for (var i = 0; i < amountOfCContainers; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, GenerateRandomWeight()));
        }
        for (var i = 0; i < amountOfNContainers; i++)
        {
            containers.Add(new Container(ContainerType.Normal, GenerateRandomWeight()));
        }

        ship.AddContainers(containers);
        return ship;
    }

    public static Ship GenerateTestShip(int width,
        int length,
        int amountOfCVContainers,
        int cVWeight,
        int amountOfVContainers,
        int vWeight,
        int amountOfCContainers,
        int cWeight,
        int amountOfNContainers,
        int nWeight)
    {
        var ship = new Ship(width, length);
        var containers = new List<Container>();
        for (var i = 0; i < amountOfCVContainers; i++)
        {
            containers.Add(new Container(ContainerType.ValuableCooled, cVWeight));
        }
        for (var i = 0; i < amountOfVContainers; i++)
        {
            containers.Add(new Container(ContainerType.Valuable, vWeight));
        }
        for (var i = 0; i < amountOfCContainers; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, cWeight));
        }
        for (var i = 0; i < amountOfNContainers; i++)
        {
            containers.Add(new Container(ContainerType.Normal, nWeight));
        }

        ship.AddContainers(containers);
        return ship;
    }

    private static int GenerateRandomWeight()
    {
        var random = new Random();
        return random.Next(4, 30);
    }
}