namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Escaping for configured text emitted into generated Angular artifacts. Signals-path only —
/// the deprecated Formly templates keep their historical (unescaped) emission.
/// </summary>
internal static class AngularSignalsTextEscaping
{
    internal static string EscapeHtmlAttributeValue(this string value) =>
        value.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;");

    internal static string EscapeHtmlText(this string value) =>
        value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");

    internal static string EscapeTsSingleQuoted(this string value) =>
        value.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n");

    internal static string EscapeTemplateLiteralText(this string value) =>
        value.Replace("\\", "\\\\").Replace("`", "\\`").Replace("${", "\\${");
}
