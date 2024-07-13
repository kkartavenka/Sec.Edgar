using System;

namespace Sec.Edgar.Enums
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class SpecialEnumAttribute : Attribute
    {
        internal SpecialEnumAttribute(string value)
        {
            Value = value;
        }

        internal SpecialEnumAttribute(string value, string[] aliases)
        {
            Value = value;
            Aliases = aliases;
        }

        internal string Value { get; }

#if NET8
        internal string[]? Aliases { get; }
#else
        internal string[] Aliases { get; }
#endif
    }
}