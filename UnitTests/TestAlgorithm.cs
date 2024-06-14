namespace UnitTests;

[TestClass]
public class TestAlgorithm
{
    #region TestHelpers
    
    private Ship GenerateTestShip(int width,
        int length,
        int amountOfCVContainers,
        int amountOfVContainers,
        int amountOfCContainers,
        int amountOfNContainers)
    {
        Ship ship = new Ship(width, length);
        List<Container> containers = new List<Container>();
        for (int i = 0; i < amountOfCVContainers; i++)
            containers.Add(new(ContainerType.ValuableCooled, GenerateRandomWeight()));
        for (int i = 0; i < amountOfVContainers; i++)
            containers.Add(new(ContainerType.Valuable, GenerateRandomWeight()));
        for (int i = 0; i < amountOfCContainers; i++)
            containers.Add(new(ContainerType.Cooled, GenerateRandomWeight()));
        for (int i = 0; i < amountOfNContainers; i++)
            containers.Add(new(ContainerType.Normal, GenerateRandomWeight()));
        
        ship.AddContainers(containers);
        return ship;
    }
    
    private Ship GenerateTestShip(int width,
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
            containers.Add(new(ContainerType.ValuableCooled, cVWeight));
        for (int i = 0; i < amountOfVContainers; i++)
            containers.Add(new(ContainerType.Valuable, vWeight));
        for (int i = 0; i < amountOfCContainers; i++)
            containers.Add(new(ContainerType.Cooled, cWeight));
        for (int i = 0; i < amountOfNContainers; i++)
            containers.Add(new(ContainerType.Normal, nWeight));
        
        ship.AddContainers(containers);
        return ship;
    }
    
    private int GenerateRandomWeight()
    {
        Random random = new Random();
        return random.Next(4, 30);
    }
    
    #endregion

    [TestMethod]
    public void DemienTest()
    {
        //Arrange
        Ship ship = GenerateTestShip(3, 3, 3, 14, 0, 0, 6, 24, 60, 10);
        
        //Act
        SortResult result = ship.SortContainers();
        
        //Assert
        bool goodSort = result == SortResult.Success || result == SortResult.SuccesWithFailedContainers;
        Assert.IsTrue(goodSort);
        
        Assert.AreEqual(69, (ship.GetTotalFailedContainers().Count + ship.AmountOfSortedContainers()));
    }

    [TestMethod]
    public void WideShipTest()
    {
        Ship ship = new Ship(8, 3);
        List<Container> containers = new List<Container>();
        for (int i = 0; i < 6; i++)
            containers.Add(new(ContainerType.ValuableCooled, GenerateRandomWeight()));
        for (int i = 0; i < 17; i++)
            containers.Add(new(ContainerType.Cooled, GenerateRandomWeight()));
        for (int i = 0; i < 60; i++)
            containers.Add(new(ContainerType.Normal, GenerateRandomWeight()));
        for (int i = 0; i < 18; i++)
            containers.Add(new(ContainerType.Valuable, GenerateRandomWeight()));
        
        ship.AddContainers(containers);
        var result = ship.SortContainers();
        
        string link = new LinkGenerator().ConvertShipToLink(ship);

        List<Container> existingContainers = ship.AmountOfSortedContainers();
        List<Container> failedContainers = ship.GetTotalFailedContainers();
        List<Container> allOutputContainers = existingContainers.Concat(failedContainers).ToList();
        
        var dissapearedContainers = containers.Except(existingContainers).ToList(); //It should be empty, it is a shame that I even have to check this.
        dissapearedContainers = dissapearedContainers.Except(failedContainers).ToList(); //It should be empty, it is a shame that I even have to check this.
        
        #region Assert

        Assert.AreEqual(3, ship.SortedRows.Count);
        
        Assert.AreEqual(0, dissapearedContainers.Count);
        
        Assert.AreEqual(101, allOutputContainers.Count);

        #endregion
    }

    [TestMethod]
    public void TestValuable()
    {
        Ship ship = new Ship(8, 6);
        List<Container> containers = new List<Container>();
        for (int i = 0; i < 6; i++)
            containers.Add(new(ContainerType.ValuableCooled, GenerateRandomWeight()));
        for (int i = 0; i < 18; i++)
            containers.Add(new(ContainerType.Valuable, GenerateRandomWeight()));
        
        ship.AddContainers(containers);
        var result = ship.SortContainers();
        
        string link = new LinkGenerator().ConvertShipToLink(result, ship);
    }
}