using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsFieldAttributeExtensions
{
    internal static string FieldClassAttribute(this FormControl field)
    {
        var staticClasses = $"entry-{field.PropertyName.Kebaberize()}-field entry-{field.ControlType.Kebaberize()}";
        staticClasses = field.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When == ApplyWhen.Always)
            .Aggregate(staticClasses, (current, classNameEntry) => current + $" {classNameEntry.Value}");

        return $"class=\"{staticClasses}\"{field.ConditionalClassBindings()}";
    }

    internal static string ConditionalClassBindings(this FormControl field) =>
        String.Join("", field.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When != ApplyWhen.Always)
            .Select(classNameEntry => classNameEntry.When == ApplyWhen.FormIsReadonly
                ? $" [class.{classNameEntry.Value}]=\"isReadonly()\""
                : $" [class.{classNameEntry.Value}]=\"!isReadonly()\""));

    // Only 'fill' and 'outline' are valid mat-form-field appearances; the legacy
    // Formly-era values (standard/legacy/none) fall back to the Material default.
    internal static string AppearanceAttribute(this FormControl field) => field.Appearance switch
    {
        FormControlAppearance.Fill => " appearance=\"fill\"",
        FormControlAppearance.Outline => " appearance=\"outline\"",
        _ => ""
    };

    internal static string PlaceholderAttribute(this FormControl field, bool enableI18N) =>
        field.Placeholder.Value.HasContent()
            ? $" placeholder=\"{field.Placeholder.Value}\"{field.Placeholder.I18NAttributeFor("placeholder", enableI18N)}"
            : "";

    internal static string TooltipAttribute(this FormControl field, bool enableI18N) =>
        field.Tooltip.Value.HasContent()
            ? $" matTooltip=\"{field.Tooltip.Value}\"{field.Tooltip.I18NAttributeFor("matTooltip", enableI18N)}"
            : "";

    internal static string HintLine(this FormControl field, bool enableI18N) =>
        field.Hint.Value.HasContent()
            ? $"    <mat-hint{field.Hint.I18NAttribute(enableI18N)}>{field.Hint.Value}</mat-hint>\r\n"
            : "";

    internal static string MetadataAttributes(this FormControl field) =>
        String.Concat(field.Metadata.Select(metadataEntry => $" {metadataEntry.Key}=\"{metadataEntry.Value}\""));

    // Requires the EntryFieldFormatDirective from @enigmatry/entry-form; the import is added
    // by AngularSignalsImportsHtmlHelperExtensions when any control carries a formatter.
    internal static string FormatAttribute(this FormControl field) =>
        field.Formatter != null && field.Formatter.JsFormatterName.HasContent()
            ? $" entryFieldFormat [entryFieldFormatDef]=\"{field.Formatter.ToJsObject()}\""
            : "";

    internal static string I18NAttribute(this I18NString text, bool enableI18N) =>
        enableI18N && text.Key.HasContent() && text.Value.HasContent()
            ? $" i18n=\"@@{text.Key}\""
            : "";

    internal static string I18NAttributeFor(this I18NString text, string attributeName, bool enableI18N) =>
        enableI18N && text.Key.HasContent() && text.Value.HasContent()
            ? $" i18n-{attributeName}=\"@@{text.Key}\""
            : "";
}
