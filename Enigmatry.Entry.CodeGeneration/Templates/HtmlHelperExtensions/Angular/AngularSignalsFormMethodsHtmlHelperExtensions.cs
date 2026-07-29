using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormMethodsHtmlHelperExtensions
{
    public static IHtmlContent DefaultLabelDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model, bool enableI18N)
    {
        var labelEntries = model.LabelledControlsWithKeys()
            .Select(entry => $"        {entry.Key}: {htmlHelper.LocalizeEscaped(entry.Control.Label, enableI18N)},");

        return htmlHelper.Raw(
            "    private readonly defaultLabels: Record<string, string> = {\r\n" +
            String.Concat(labelEntries.Select(entry => entry + "\r\n")) +
            "    };\r\n" +
            "\r\n" +
            "    // Array-item children pass the current row as arrayItemModel so label expressions evaluate per row.\r\n" +
            "    protected readonly label = (propertyName: string, arrayItemModel?: unknown): string => {\r\n" +
            "        const labelExpression = this.fieldsLabelExpressions()?.[propertyName];\r\n" +
            $"        return labelExpression ? String(labelExpression((arrayItemModel ?? this.currentModel()) as I{model.ComponentInfo.ModelType})) : this.defaultLabels[propertyName] ?? '';\r\n" +
            "    };\r\n");
    }

    // Array-item children are namespaced by their array's property name ('addresses.city'),
    // matching FormViewRenderContext.Key, so they never collide with a root control.
    private static IEnumerable<(string Key, FormControl Control)> LabelledControlsWithKeys(this FormComponentModel model)
    {
        static bool IsLabelled(FormControl control) =>
            control is not ButtonFormControl and not ArrayFormControl and not FormControlGroup;

        foreach (var control in model.FlatFormControls().Where(IsLabelled))
        {
            yield return (control.PropertyName, control);
        }

        foreach (var array in model.FlatFormControls().OfType<ArrayFormControl>())
        {
            var children = ((FormControlGroup)array.FormControlGroup).FormControls;
            foreach (var child in children.FlatFormControls().Where(IsLabelled))
            {
                yield return ($"'{array.PropertyName}.{child.PropertyName}'", child);
            }
        }
    }

    public static IHtmlContent SelectAllHelperMethod(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        if (!model.AllControlsIncludingArrayItems().OfType<MultiSelectFormControl>().Any(multiSelect => multiSelect.Options.SelectAllOption != null))
        {
            return htmlHelper.Raw("");
        }

        return htmlHelper.Raw(
            "\r\n" +
            "    protected readonly toggleSelectAll = (control: AbstractControl | null, options: { value: unknown; displayName: unknown }[]): void => {\r\n" +
            "        const values = options.map(option => option.value);\r\n" +
            "        const selectedValues = ((control?.value as unknown[] | null) ?? []).filter(value => values.includes(value));\r\n" +
            "        control?.setValue(selectedValues.length === values.length ? [] : values);\r\n" +
            "        control?.markAsDirty();\r\n" +
            "    };\r\n");
    }

    public static IHtmlContent ReadonlyDisplayHelperMethods(this IHtmlHelper htmlHelper, FormComponentModel model, bool enableI18N)
    {
        if (!model.UseReadonlyDisplay)
        {
            return htmlHelper.Raw("");
        }

        var booleanValueMethod = "";
        if (model.AllControlsIncludingArrayItems().Any(control => control.Formatter?.JsFormatterName == "boolean"))
        {
            var yesText = enableI18N ? "$localize`:@@entry.readonly.boolean.yes:Yes`" : "'Yes'";
            var noText = enableI18N ? "$localize`:@@entry.readonly.boolean.no:No`" : "'No'";
            booleanValueMethod =
                "\r\n" +
                "    protected readonly readonlyBooleanValue = (control: AbstractControl | null): string =>\r\n" +
                $"        control?.value ? {yesText} : {noText};\r\n";
        }

        return htmlHelper.Raw(
            "\r\n" +
            "    protected readonly readonlyValue = (control: AbstractControl | null): string =>\r\n" +
            "        String(control?.value ?? '');\r\n" +
            booleanValueMethod +
            "\r\n" +
            "    protected readonly selectedDisplayName = (value: unknown, options: { value: unknown; displayName: unknown }[]): string =>\r\n" +
            "        Array.isArray(value)\r\n" +
            "            ? value.map(item => options.find(option => option.value === item)?.displayName ?? item).join(', ')\r\n" +
            "            : String(options.find(option => option.value === value)?.displayName ?? value ?? '');\r\n");
    }

    public static IHtmlContent MultiCheckboxHelperMethods(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        if (!model.AllControlsIncludingArrayItems().OfType<MultiCheckboxFormControl>().Any())
        {
            return htmlHelper.Raw("");
        }

        return htmlHelper.Raw(
            "\r\n" +
            "    protected readonly isOptionSelected = (control: AbstractControl | null, value: unknown): boolean =>\r\n" +
            "        ((control?.value as unknown[] | null) ?? []).includes(value);\r\n" +
            "\r\n" +
            "    protected readonly toggleOption = (control: AbstractControl | null, value: unknown, checked: boolean): void => {\r\n" +
            "        const currentValues = (control?.value as unknown[] | null) ?? [];\r\n" +
            "        control?.setValue(checked ? [...currentValues, value] : currentValues.filter(item => item !== value));\r\n" +
            "        control?.markAsDirty();\r\n" +
            "    };\r\n");
    }
}
