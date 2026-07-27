using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Wraps a field's interactive markup in an isReadonly() switch that renders a plain
/// label/value display while the form is readonly. Opt-in via FormComponentBuilder.WithReadonlyDisplay.
/// </summary>
internal static class AngularSignalsReadonlyDisplayRenderer
{
    internal static IHtmlContent WrapWithReadonlyDisplay(this IHtmlHelper htmlHelper, FormControl field, IHtmlContent controlMarkup, FormViewRenderContext context)
    {
        if (!context.UseReadonlyDisplay || !field.SupportsReadonlyDisplay())
        {
            return controlMarkup;
        }

        return htmlHelper.Raw(
            $"@if (isReadonly()) {{\r\n" +
            $"@if (!isHidden('{field.PropertyName}', {field.Visible.ToString().ToLower()})) {{\r\n" +
            $"<div class=\"entry-{field.PropertyName.Kebaberize()}-field entry-readonly-field\">\r\n" +
            $"    <span class=\"entry-readonly-label\">{{{{ label('{field.PropertyName}') }}}}</span>\r\n" +
            $"    <span class=\"entry-readonly-value\">{{{{ {field.ReadonlyValueExpression()} }}}}</span>\r\n" +
            $"</div>\r\n" +
            $"}}\r\n" +
            $"}} @else {{\r\n" +
            controlMarkup.ToString() +
            $"}}\r\n");
    }

    // Password stays masked (never displayed), rich text and custom controls keep their own
    // readonly handling, arrays/groups/buttons are structural.
    private static bool SupportsReadonlyDisplay(this FormControl field) => field switch
    {
        PasswordFormControl => false,
        RichTextInputFormControl => false,
        SelectControlBase => true,
        InputControlBase => true,
        CheckboxFormControl => true,
        DatepickerFormControl => true,
        DateTimePickerFormControl => true,
        _ => false
    };

    private static string ReadonlyValueExpression(this FormControl field) => field is SelectControlBase
        ? $"selectedDisplayName(form.get('{field.PropertyName}')?.value, {field.PropertyName}Options())"
        : $"readonlyValue('{field.PropertyName}')";
}
