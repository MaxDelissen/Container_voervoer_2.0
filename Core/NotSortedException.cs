namespace Core;

public class NotSortedException : Exception
{
    public NotSortedException() : base("This ship has not been sorted yet."){}
    public NotSortedException(string message) : base(message){}
    public NotSortedException(string message, Exception inner) : base(message, inner){}
}