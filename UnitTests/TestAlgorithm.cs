namespace UnitTests;

[TestClass]
public class TestAlgorithm
{
    [TestMethod]
    public void DemienTest()
    {
        //Arrange
        Ship ship = TestHelpers.GenerateTestShip(3, 3, 3, 14, 0, 0, 6, 24, 60, 10);

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
        int failedCount = failed.Count;
        int placed = ship.GetTotalPlacedContainers().Count;
        Assert.AreEqual(failedCount, 0);
        Assert.AreEqual(69, placed);
    }

    [TestMethod]
    public void WideShipTest()
    {
        //Arrange
        Ship ship = TestHelpers.GenerateTestShip(8, 3,
        6, 20,
        8, 25,
        17, 30,
        69, 20);

        //Act
        SortResult result = ship.SortContainers();

        //Assert
        var placedContainers = ship.GetTotalPlacedContainers();
        var failedContainers = ship.GetTotalFailedContainers();
        var allOutputContainers = placedContainers.Concat(failedContainers).ToList();
        string link = new LinkGenerator().ConvertShipToLink(ship);

        Assert.IsTrue(result == SortResult.Success);
        Assert.AreEqual(3, ship.SortedRows.Count);
        Assert.AreEqual(8, ship.SortedRows[0].Stacks.Count);
        Assert.AreEqual(8, ship.SortedRows[2].Stacks.Count);

        Assert.AreEqual(failedContainers.Count, 0);
        Assert.AreEqual(100, placedContainers.Count);
    }
}