namespace Sec.Edgar.Models;

public class TickerDuplicateException : Exception
{
    public TickerDuplicateException(string message) : base(message)
    {
        
    }

    public TickerDuplicateException(string message, Exception inner) : base(message, inner)
    {
        
    }
}