#if NETSTANDARD2_0
using Newtonsoft.Json.Linq;

namespace Sec.Edgar.Extensions
{
    public static class JsonExtension
    {
        public static bool TryGetInt32(this JToken token, out int value)
        {
            if (token == null)
            {
                value = 0;
                return false;
            }

            if (token.Type == JTokenType.Integer)
            {
                value = token.Value<int>();
                return true;
            }

            if (token.Type == JTokenType.String && int.TryParse(token.Value<string>(), out value))
            {
                return true;
            }

            value = 0;
            return false;
        }  
    }
}
#endif