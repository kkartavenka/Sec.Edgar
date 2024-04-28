namespace Sec.Edgar.Models.Exceptions;

internal class ExceptionEventArgs : EventArgs
{
    internal ExceptionEventArgs(Exception e, bool reThrow)
    {
        Exception = e;
        ReThrow = reThrow;
    }
    
    internal Exception Exception { get; private set; }
    internal bool ReThrow { get; private set; }
}