namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// Assigns a select-optgroup label to an enum member, for use with WithFixedValues&lt;T&gt;()
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class SelectOptionGroupAttribute(string group) : Attribute
{
    public string Group { get; } = group;
}
