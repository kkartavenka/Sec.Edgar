using System;

namespace Sec.Edgar.Models.Exceptions
{
    internal class TickerDuplicateException : Exception
    {
        internal TickerDuplicateException(string message) : base(message)
        {
        }

        internal TickerDuplicateException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}