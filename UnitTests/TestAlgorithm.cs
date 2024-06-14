using Core.ContainerStorage;
using Core.Enums;
using Exceptions;

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
        
        Assert.AreEqual(3, ship.SortedRows.Count);
        Assert.AreEqual(3, ship.SortedRows[0].Stacks.Count);
        Assert.AreEqual(3, ship.SortedRows[2].Stacks.Count);
        string link = new LinkGenerator().ConvertShipToLink(ship);
        var failed = ship.GetTotalFailedContainers();
        var failedCount = failed.Count;
        int placed = ship.GetTotalPlacedContainers().Count;
        int total = failedCount + placed;
        Assert.AreEqual(69, total);
    }

    [TestMethod]
    public void WideShipTest()
    {
        //Arrange
        Ship ship = GenerateTestShip(8, 3, 6, 18, 17, 60);
        
        //Act
        var result = ship.SortContainers();

        //Assert
        bool goodSort = result == SortResult.Success || result == SortResult.SuccesWithFailedContainers;
        List<Container> placedContainers = ship.GetTotalPlacedContainers();
        List<Container> failedContainers = ship.GetTotalFailedContainers();
        List<Container> allOutputContainers = placedContainers.Concat(failedContainers).ToList();
        
        Assert.IsTrue(goodSort);
        Assert.AreEqual(3, ship.SortedRows.Count);
        Assert.AreEqual(8, ship.SortedRows[0].Stacks.Count);
        Assert.AreEqual(8, ship.SortedRows[2].Stacks.Count);
        
        Assert.AreEqual(101, allOutputContainers.Count);

    }
    
    //[ExpectedException(typeof(ContainerOverWeightException))]
}