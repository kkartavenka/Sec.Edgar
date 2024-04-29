using System.Reflection;

namespace Sec.Edgar.Models;

internal static class EnumExtension
{
    internal static List<KeyValuePair<string, string>> GetMapping<T>() where T : Enum
    {
        var returnVar = typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .SelectMany(x =>
            {
                var feAttribute = x.GetCustomAttribute<FormEnumAttribute>();

                if (feAttribute is null)
                {
                    return Array.Empty<(string, string)>();
                }

                var aliasCount = feAttribute.Aliases?.Length ?? 0;
                var returnElements = new (string, string)[aliasCount + 1];
                returnElements[0] = (x.Name, feAttribute.Value.ToLower());

                if (feAttribute.Aliases is not null)
                {
                    var writeArrayPosition = 1;
                    foreach (var alias in feAttribute.Aliases)
                    {
                        returnElements[writeArrayPosition] = (x.Name, alias.ToLower());
                        writeArrayPosition++;
                    }
                }

                return returnElements;
            })
            .Select(x => new KeyValuePair<string, string>(x.Item2, x.Item1))
            .ToList();

        return returnVar;
    }
}