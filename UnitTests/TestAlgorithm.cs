namespace UnitTests;

[TestClass]
public class TestAlgorithm
{
    [TestMethod]
    public void TestSortBasicShip_ReturnSortedContainers()
    {
        //Arrange
        Ship ship = new Ship(2, 2);
        List<Container> containers = new List<Container>
        {
            new Container(ContainerType.Normal, 10),
            new Container(ContainerType.Normal, 10),
            new Container(ContainerType.Normal, 10),
            new Container(ContainerType.Normal, 10)
        };
        ship.AddContainers(containers);
        
        //Act
        var result = ship.SortContainers();
        
        //Assert
        Assert.AreEqual(2, result.Count);
        
    }

    [TestMethod]
    public void DemienTest()
    {
        Ship ship = new Ship(3, 3);
        List<Container> containers = new List<Container>();
        containers.Add(new Container(ContainerType.ValuableCooled, 14));
        containers.Add(new Container(ContainerType.ValuableCooled, 14));
        containers.Add(new Container(ContainerType.ValuableCooled, 14));
        for (int i = 0; i < 6; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, 24));
        }
        for (int i = 0; i < 60; i++)
        {
            containers.Add(new Container(ContainerType.Normal, 10));
        }
        
        ship.AddContainers(containers);
        var result = ship.SortContainers();
        
        string link = new LinkGenerator().ConvertShipToLink(result, ship);
    }
}