namespace Sec.Edgar.Models;

public class ExceptionEventArgs : EventArgs
{
    public ExceptionEventArgs(Exception e, bool reThrow)
    {
        Exception = e;
        ReThrow = reThrow;
    }
    
    public Exception Exception { get; private set; }
    public bool ReThrow { get; private set; }
}