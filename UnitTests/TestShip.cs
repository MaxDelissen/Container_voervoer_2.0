namespace UnitTests;

[TestClass]
[TestSubject(typeof(Ship))]
public class TestShip
{
    [TestMethod]
    public void TestAddContainersToShip_ShipIncludeAddedContainers()
    {
        //Arrange
        var ship = new Ship(3, 3);
        var containers = new List<Container>
        {
            new(ContainerType.ValuableCooled, 20),
            new(ContainerType.Valuable, 20),
            new(ContainerType.Cooled, 20),
            new(ContainerType.Normal, 20)
        };

        //Act
        ship.AddContainers(containers);

        //Assert
        Assert.AreEqual(containers.Count, ship.ContainersToSort.Count);
        foreach (Container container in containers)
        {
            Assert.IsTrue(ship.ContainersToSort.Contains(container));
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ContainerOverWeightException))]
    public void TestAddOverWeightContainerToShip_ThrowsException()
    {
        //Arrange
        var ship = new Ship(3, 3);
        var container = new List<Container>
        {
            new(ContainerType.Normal, 31)
        };
        ship.AddContainers(container);
    }

    [TestMethod]
    [ExpectedException(typeof(NotSortedException))]
    public void TestRequestSortedRowsBeforeSorting_ThrowsException()
    {
        //Arrange
        var ship = new Ship(3, 3);

        //Act
        var sortedRows = ship.SortedRows;
    }
}