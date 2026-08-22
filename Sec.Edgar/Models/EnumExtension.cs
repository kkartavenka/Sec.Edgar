using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sec.Edgar.Enums;

namespace Sec.Edgar.Models
{
    internal static class EnumExtension
    {
        private static readonly ConcurrentDictionary<Type, List<KeyValuePair<string, string>>> AttributeToItem =
            new ConcurrentDictionary<Type, List<KeyValuePair<string, string>>>();

        private static readonly ConcurrentDictionary<Type, List<KeyValuePair<string, string>>> ItemToAttribute =
            new ConcurrentDictionary<Type, List<KeyValuePair<string, string>>>();

        internal static List<KeyValuePair<string, string>> GetMapping<T>() where T : Enum
        {
            if (AttributeToItem.ContainsKey(typeof(T)))
            {
                return AttributeToItem[typeof(T)];
            }

            var returnVar = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .SelectMany(x =>
                {
                    var feAttribute = x.GetCustomAttribute<SpecialEnumAttribute>();

                    if (feAttribute is null)
                    {
                        return Array.Empty<(string, string)>();
                    }

                    var aliasCount = feAttribute.Aliases?.Length ?? 0;
                    var returnElements = new (string, string)[aliasCount + 1];
                    returnElements[0] = (x.Name, feAttribute.Value.ToLower());

                    if (feAttribute.Aliases != null)
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

            AttributeToItem[typeof(T)] = returnVar;

            return returnVar;
        }

        internal static List<KeyValuePair<string, string>> GetAttributeMapping<T>() where T : Enum
        {
            if (ItemToAttribute.ContainsKey(typeof(T)))
            {
                return ItemToAttribute[typeof(T)];
            }

            var returnVar = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(x =>
                {
                    var attribute = x.GetCustomAttribute<SpecialEnumAttribute>();
                    return (x.Name, attribute?.Value.ToLower());
                })
                .Where(x => !string.IsNullOrEmpty(x.Item2))
                .Select(x => new KeyValuePair<string, string>(x.Name, x.Item2))
                .ToList();

            ItemToAttribute[typeof(T)] = returnVar;

            return returnVar;
        }
    }
}