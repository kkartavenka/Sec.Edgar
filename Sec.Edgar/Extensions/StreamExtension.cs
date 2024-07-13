#if NETSTANDARD2_0
using System.IO;

namespace Sec.Edgar.Extensions
{
    internal static class StreamExtension
    {
        internal static string GetString(this Stream stream)
        {
            using (var sReader = new StreamReader(stream))
            {
                return sReader.ReadToEnd();
            }
        }
    }
}
#endif