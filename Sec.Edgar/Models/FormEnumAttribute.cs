namespace Sec.Edgar.Models;

[AttributeUsage(AttributeTargets.Field)]
internal class FormEnumAttribute : Attribute
{
    internal FormEnumAttribute(string value)
    {
        Value = value;
    }
    
    internal FormEnumAttribute(string value, string[] aliases)
    {
        Value = value;
        Aliases = aliases;
    }
    
    internal string Value { get; }
    internal string[]? Aliases { get; }
}