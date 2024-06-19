namespace UnitTests;

[TestClass]
public class TestSortResults
{
    [TestMethod]
    public void TestShipOverWeight_ShouldReturnOverWeight()
    {
        //Arrange
        Ship ship = TestHelpers.GenerateTestShip(3, 3,
        0, 0,
        0, 0,
        0, 0,
        500, 30); // 500 containers of 30 weight

        //Act
        SortResult result = ship.SortContainers();

        //Assert
        Assert.AreEqual(SortResult.OverWeight, result);
    }

    [TestMethod]
    public void TestShipUnderWeight_ShouldReturnUnderWeight()
    {
        //Arrange
        Ship ship = TestHelpers.GenerateTestShip(3, 3,
        0, 0,
        0, 0,
        0, 0,
        10, 10);

        //Act
        SortResult result = ship.SortContainers();

        //Assert
        Assert.AreEqual(SortResult.UnderWeight, result);
    }

    [TestMethod]
    public void TestTooManyCooledValuableContainers_ShouldReturnTooManyCooledValuableContainers()
    {
        //Arrange
        Ship ship = TestHelpers.GenerateTestShip(3, 3,
        4, 30,
        0, 0,
        0, 0,
        60, 20); //Add normal containers as to not trigger UnderWeight

        //Act
        SortResult result = ship.SortContainers();

        //Assert
        Assert.AreEqual(SortResult.TooManyCooledValuableContainers, result);
    }

    [TestMethod]
    public void TestTooManyCooledContainers_ShouldReturnTooManyCooledContainers()
    {
        //Arrange
        Ship ship = TestHelpers.GenerateTestShip(3, 3,
        0, 0,
        0, 0,
        20, 30,
        10, 30); //Add normal containers as to not trigger UnderWeight

        //Act
        SortResult result = ship.SortContainers();

        //Assert
        Assert.AreEqual(SortResult.TooManyCooledContainers, result);
    }

    [TestMethod]
    public void TestShipWithTooManyValuableContainers_ShouldReturnSuccesWithFailedContainers()
    {
        //Arrange
        Ship ship = TestHelpers.GenerateTestShip(3, 3,
        2, 10,
        30, 10,
        10, 5,
        16, 30);

        //Act
        SortResult result = ship.SortContainers();

        //Assert
        Assert.AreEqual(SortResult.SuccesWithFailedContainers, result);
        var failedContainers = ship.GetTotalFailedContainers();
        bool onlyValuableContainers = failedContainers.All(c => c.Type == ContainerType.Valuable);
        Assert.IsTrue(onlyValuableContainers);
    }
}