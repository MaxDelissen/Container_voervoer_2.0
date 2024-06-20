namespace Exceptions;

public class ContainerOverWeightException : Exception
{
    public ContainerOverWeightException() : base("The container is over the maximum weight of 30.") {}
}