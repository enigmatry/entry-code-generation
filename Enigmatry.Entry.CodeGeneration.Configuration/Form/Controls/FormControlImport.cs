namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// An Angular import (symbol + module path) that provides a control's component,
/// added to the generated standalone component's imports. Used by the signals templates.
/// </summary>
public class FormControlImport(string symbol, string path)
{
    public string Symbol { get; } = symbol;
    public string Path { get; } = path;
}
