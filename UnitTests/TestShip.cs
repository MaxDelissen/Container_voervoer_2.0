namespace UnitTests;

[TestClass]
public class TestShip
{
    [TestMethod]
    public void TestCreateShip_ReturnCorrectWeight()
    {
        // Arrange
        var ship = new Ship(3, 3);

        // Act
        var minweight = ship.MinWeight;
        var maxweight = ship.MaxWeight;

        // Assert
        Assert.AreEqual(1350, maxweight);
        Assert.AreEqual(675, minweight);
    }

    [TestMethod]
    public void TestAddContainer_ReturnCorrectCount()
    {
        // Arrange
        var ship = new Ship(3, 3);
        var container = new Container(ContainerType.Normal, 10);

        // Act
        ship.AddContainer(container);

        // Assert
        Assert.AreEqual(1, ship.ContainerToSortCount());
    }

    [TestMethod]
    public void TestAddContainers_ReturnCorrectCount()
    {
        // Arrange
        var ship = new Ship(3, 3);
        var containers = new List<Container>
        {
            new Container(ContainerType.Valuable, 30),
            new Container(ContainerType.ValuableCooled, 20),
            new Container(ContainerType.Cooled, 10),
            new Container(ContainerType.Normal, 10)
        };

        // Act
        ship.AddContainers(containers);

        // Assert
        Assert.AreEqual(4, ship.ContainerToSortCount());
        Assert.AreEqual(ContainerType.Valuable, ship.ContainersToSort[0].Type);
        Assert.AreEqual(ContainerType.ValuableCooled, ship.ContainersToSort[1].Type);
        Assert.AreEqual(ContainerType.Cooled, ship.ContainersToSort[2].Type);
        Assert.AreEqual(ContainerType.Normal, ship.ContainersToSort[3].Type);
        Assert.AreEqual(30, ship.ContainersToSort[0].Weight);
        Assert.AreEqual(20, ship.ContainersToSort[1].Weight);
        Assert.AreEqual(10, ship.ContainersToSort[2].Weight);
        Assert.AreEqual(10, ship.ContainersToSort[3].Weight);
    }

    [TestMethod]
    public void TestAddOverWeightContainer_ReturnFalse()
    {
        // Arrange
        var ship = new Ship(3, 3);
        var container = new Container(ContainerType.Normal, 100);

        // Act
        var result = ship.AddContainer(container);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestAddOverWeightContainerInList_ReturnFalse()
    {
        // Arrange
        var ship = new Ship(3, 3);
        var containers = new List<Container>
        {
            new Container(ContainerType.Normal, 10),
            new Container(ContainerType.Normal, 100),
            new Container(ContainerType.Normal, 10)
        };

        // Act
        var result = ship.AddContainers(containers);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestIsOverWeight_ReturnTrue()
    {
        // Arrange
        var ship = new Ship(1, 1);
        var containers = new List<Container>();
        for (int i = 0; i < 20; i++)
        {
            containers.Add(new Container(ContainerType.Normal, 30));
        }
        ship.AddContainers(containers);

        // Act
        var result = ship.IsOverWeight();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestIsNotOverWeight_ReturnFalse()
    {
        // Arrange
        var ship = new Ship(5, 10);
        var containers = new List<Container>();

        containers.Add(new Container(ContainerType.Normal, 30));

        ship.AddContainers(containers);

        // Act
        var result = ship.IsOverWeight();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestIsUnderWeight_ReturnTrue()
    {
        // Arrange
        var ship = new Ship(8, 10);
        var containers = new List<Container>();
        containers.Add(new Container(ContainerType.Normal, 30));
        ship.AddContainers(containers);

        // Act
        var result = ship.IsUnderWeight();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestIsNotUnderWeight_ReturnFalse()
    {
        // Arrange
        var ship = new Ship(1, 1);
        var containers = new List<Container>();
        for (int i = 0; i < 10; i++)
        {
            containers.Add(new Container(ContainerType.Normal, 10));
        }
        ship.AddContainers(containers);

        // Act
        var result = ship.IsUnderWeight();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestHasTooManyCooledValuableContainers_ReturnTrue()
    {
        // Arrange
        var ship = new Ship(3, 3);
        var containers = new List<Container>
        {
            new Container(ContainerType.ValuableCooled, 10),
            new Container(ContainerType.ValuableCooled, 10),
            new Container(ContainerType.ValuableCooled, 10),
            new Container(ContainerType.ValuableCooled, 10)
        };
        ship.AddContainers(containers);

        // Act
        var result = ship.HasTooManyCooledValuableContainers();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestDoesNotHaveTooManyCooledValuableContainers_ReturnFalse()
    {
        // Arrange
        var ship = new Ship(3, 3);
        var containers = new List<Container>
        {
            new Container(ContainerType.ValuableCooled, 10),
            new Container(ContainerType.ValuableCooled, 10),
            new Container(ContainerType.ValuableCooled, 10)
        };
        ship.AddContainers(containers);

        // Act
        var result = ship.HasTooManyCooledValuableContainers();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestHasTooManyCooledContainers_ReturnTrue()
    {
        // Arrange
        var ship = new Ship(2, 3);
        var containers = new List<Container>();
        for (int i = 0; i < 50; i++)
        {
            containers.Add(new Container(ContainerType.Cooled, 30));
        }
        containers.Add(new Container(ContainerType.Normal, 20));
        ship.AddContainers(containers);

        // Act
        var result = ship.HasTooManyCooledContainers();

        // Assert
        Assert.IsTrue(result);
    }
}