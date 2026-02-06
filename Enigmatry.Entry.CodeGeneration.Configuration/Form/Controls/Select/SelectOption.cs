namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public class SelectOption(object? value, string displayName, string translationId)
{
    public object? Value { get; } = value;
    public I18NString DisplayName { get; } = new(translationId, displayName);

    public SelectOption(object? value, string displayName)
        : this(value, displayName, String.Empty)
    {
    }

    public string GetValueExpression()
    {
        if (Value != null && Value.IsNumeric())
        {
            return $"value: {(Value is Enum ? (int)Value : Value)}";
        }

        if (Value is bool boolValue)
        {
            return $"value: {boolValue.ToString().ToLowerInvariant()}";
        }

        return $"value: {(Value == null ? "null" : $"'{Value}'")}";
    }
}
