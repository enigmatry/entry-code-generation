using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration;

public class I18NString
{
    public static readonly I18NString Empty = new I18NString(String.Empty, String.Empty);
    public string Key { get; }
    public string Value { get; }

    public I18NString(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public bool HasContent() => Value.HasContent();

    public override string ToString() => Value;
}