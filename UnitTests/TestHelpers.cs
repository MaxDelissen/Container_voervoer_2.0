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
        Ship ship = new Ship(width, length);
        List<Container> containers = new List<Container>();
        for (int i = 0; i < amountOfCVContainers; i++)
        {
            containers.Add(new Container(ContainerType.ValuableCooled, GenerateRandomWeight()));
        }
        for (int i = 0; i < amountOfVContainers; i++)
        {
            containers.Add(new Container(ContainerType.Valuable, GenerateRandomWeight()));
        }
        for (int i = 0; i < amountOfCContainers; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, GenerateRandomWeight()));
        }
        for (int i = 0; i < amountOfNContainers; i++)
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
        Ship ship = new Ship(width, length);
        List<Container> containers = new List<Container>();
        for (int i = 0; i < amountOfCVContainers; i++)
        {
            containers.Add(new Container(ContainerType.ValuableCooled, cVWeight));
        }
        for (int i = 0; i < amountOfVContainers; i++)
        {
            containers.Add(new Container(ContainerType.Valuable, vWeight));
        }
        for (int i = 0; i < amountOfCContainers; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, cWeight));
        }
        for (int i = 0; i < amountOfNContainers; i++)
        {
            containers.Add(new Container(ContainerType.Normal, nWeight));
        }

        ship.AddContainers(containers);
        return ship;
    }

    private static int GenerateRandomWeight()
    {
        Random random = new Random();
        return random.Next(4, 30);
    }
}