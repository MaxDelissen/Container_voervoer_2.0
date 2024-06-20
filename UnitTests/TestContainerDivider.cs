namespace UnitTests;

[TestClass]
[TestSubject(typeof(ContainerDivider))]
public class TestContainerDivider
{
    [TestMethod]
    public void DivideContainers_ShouldDivideIntoCorrectCategories()
    {
        // Arrange
        var containerDivider = new ContainerDivider();
        var containers = new List<Container>
        {
            new(ContainerType.ValuableCooled, 20),
            new(ContainerType.Valuable, 20),
            new(ContainerType.Cooled, 20),
            new(ContainerType.Normal, 20)
        };

        // Act
        var result = containerDivider.DivideContainers(containers);

        // Assert
        Assert.AreEqual(1, result.valuableCooledContainers.Count);
        Assert.AreEqual(1, result.valuableContainers.Count);
        Assert.AreEqual(1, result.cooledContainers.Count);
        Assert.AreEqual(1, result.normalContainers.Count);
    }

    [TestMethod]
    public void DivideContainers_WithEmptyList_ShouldReturnEmptyLists()
    {
        // Arrange
        var containerDivider = new ContainerDivider();
        var containers = new List<Container>();

        // Act
        var result = containerDivider.DivideContainers(containers);

        // Assert
        Assert.AreEqual(0, result.valuableCooledContainers.Count);
        Assert.AreEqual(0, result.valuableContainers.Count);
        Assert.AreEqual(0, result.cooledContainers.Count);
        Assert.AreEqual(0, result.normalContainers.Count);
    }

    [TestMethod]
    public void DivideContainers_WithNullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        var containerDivider = new ContainerDivider();

        // Act & Assert
        Assert.ThrowsException<NullReferenceException>(() => containerDivider.DivideContainers(null));
    }
}