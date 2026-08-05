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
            $"@if (!{context.IsHiddenCall(field)}) {{\r\n" +
            $"<div class=\"entry-{field.PropertyName.Kebaberize()}-field entry-readonly-field\">\r\n" +
            $"    <span class=\"entry-readonly-label\">{{{{ {context.LabelCall(field)} }}}}</span>\r\n" +
            $"    <span class=\"entry-readonly-value\">{{{{ {field.ReadonlyValueExpression(context)} }}}}</span>\r\n" +
            $"</div>\r\n" +
            $"}}\r\n" +
            $"}} @else {{\r\n" +
            controlMarkup.ToString() +
            $"}}\r\n");
    }

    // Password stays masked (never displayed), rich text and custom controls keep their own
    // readonly handling, arrays/groups/buttons are structural.
    internal static bool SupportsReadonlyDisplay(this FormControl field) => field switch
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

    private static string ReadonlyValueExpression(this FormControl field, FormViewRenderContext context)
    {
        if (field is SelectControlBase)
        {
            return $"selectedDisplayName({context.FormGroupAccessor}.get('{field.PropertyName}')?.value, {context.MemberName(field, "Options")}())";
        }

        if (field.Formatter?.JsFormatterName == "boolean")
        {
            return $"readonlyBooleanValue({context.FormGroupAccessor}.get('{field.PropertyName}'))";
        }

        var pipeExpression = field.Formatter?.PipeExpression();
        return pipeExpression != null
            ? $"({context.FormGroupAccessor}.get('{field.PropertyName}')?.value | {pipeExpression}) ?? ''"
            : $"readonlyValue({context.FormGroupAccessor}.get('{field.PropertyName}'))";
    }
}
