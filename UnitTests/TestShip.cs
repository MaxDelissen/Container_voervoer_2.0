namespace UnitTests;

[TestClass]
[TestSubject(typeof(Ship))]
public class TestShip
{
    [TestMethod]
    public void TestAddContainersToShip_ShipIncludeAddedContainers()
    {
        //Arrange
        Ship ship = new Ship(3, 3);
        List<Container> containers = new List<Container>
        {
            new Container(ContainerType.ValuableCooled, 20),
            new Container(ContainerType.Valuable, 20),
            new Container(ContainerType.Cooled, 20),
            new Container(ContainerType.Normal, 20)
        };

        //Act
        ship.AddContainers(containers);

        //Assert
        Assert.AreEqual(containers.Count, ship.ContainersToSort.Count);
        foreach (var container in containers)
        {
            Assert.IsTrue(ship.ContainersToSort.Contains(container));
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ContainerOverWeightException))]
    public void TestAddOverWeightContainerToShip_ThrowsException()
    {
        //Arrange
        Ship ship = new Ship(3, 3);
        List<Container> container = new List<Container>
        {
            new Container(ContainerType.Normal, 31)
        };
        ship.AddContainers(container);
    }
    
    [TestMethod]
    [ExpectedException(typeof(NotSortedException))]
    public void TestRequestSortedRowsBeforeSorting_ThrowsException()
    {
        //Arrange
        Ship ship = new Ship(3, 3);
        
        //Act
        var sortedRows = ship.SortedRows;
    }
}