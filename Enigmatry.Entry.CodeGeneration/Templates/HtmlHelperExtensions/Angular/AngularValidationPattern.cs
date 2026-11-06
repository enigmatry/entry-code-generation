using System.Text.RegularExpressions;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularValidationPattern
{
    // ngx-formly copies props.pattern onto the native pattern attribute, which the browser compiles
    // with the v flag, so the value has to be the regex source and not a stringified RegExp.
    internal static string AsTypeScriptLiteral(this string pattern) =>
        Regex.IsMatch(pattern, "^/.*/[a-z]*$") ? pattern : $"'{pattern.EscapeTsSingleQuoted()}'";
}
