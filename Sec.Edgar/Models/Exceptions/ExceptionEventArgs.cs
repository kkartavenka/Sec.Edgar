using Microsoft.Extensions.Logging;

namespace Sec.Edgar.Models.Exceptions;

internal class ExceptionEventArgs : EventArgs
{
    internal ExceptionEventArgs(Exception e)
    {
        LoggedException = e;
    }

    internal ExceptionEventArgs(Exception e, LogLevel logLevel)
    {
        LoggedException = e;
        LoggedLevel = logLevel;
    }

    internal ExceptionEventArgs(Exception e, bool reThrow)
    {
        LoggedException = e;
        ReThrow = reThrow;
    }

    internal ExceptionEventArgs(Exception e, LogLevel logLevel, bool reThrow)
    {
        LoggedException = e;
        LoggedLevel = logLevel;
        ReThrow = reThrow;
    }

    internal Exception LoggedException { get; private set; }
    internal LogLevel LoggedLevel { get; private set; } = LogLevel.Information;
    internal bool ReThrow { get; private set; }
}