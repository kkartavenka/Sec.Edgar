namespace Sec.Edgar.Models.Exceptions;

public class CikDuplicateException : Exception
{
    public CikDuplicateException(string message) : base(message)
    {
        
    }
    
    public CikDuplicateException(string message, Exception inner) : base(message, inner)
    {
        
    }
}