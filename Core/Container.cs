namespace Core;

public class Container
{
    public Container(ContainerType type, int weight)
    {
        Type = type;
        Weight = weight;
    }

    public ContainerType Type { get; private set; }
    public int Weight { get; private set; }
}