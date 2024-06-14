namespace Exceptions;

public class ContainerOverWeightException : Exception
{
    public ContainerOverWeightException() : base("The container is over the maximum weight of 30.") { }
    public ContainerOverWeightException(string message) : base(message) { }
    public ContainerOverWeightException(string message, Exception inner) : base(message, inner) { }
}