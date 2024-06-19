namespace Exceptions;

public class NotSortedException : Exception
{
    public NotSortedException() : base("This ship has not been sorted yet."){}
}