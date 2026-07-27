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
        var labelledControls = model.AllControlsIncludingArrayItems()
            .Where(control => control is not ButtonFormControl and not ArrayFormControl and not FormControlGroup and not CustomFormControl)
            .GroupBy(control => control.PropertyName)
            .Select(propertyGroup => propertyGroup.First())
            .ToList();

        var labelEntries = labelledControls
            .Select(control => $"        {control.PropertyName}: {htmlHelper.Localize(control.Label, enableI18N)},");

        return htmlHelper.Raw(
            "    private readonly defaultLabels: Record<string, string> = {\r\n" +
            String.Concat(labelEntries.Select(entry => entry + "\r\n")) +
            "    };\r\n" +
            "\r\n" +
            "    protected readonly label = (propertyName: string): string => {\r\n" +
            "        const labelExpression = this.fieldsLabelExpressions()?.[propertyName];\r\n" +
            "        return labelExpression ? String(labelExpression(this.model())) : this.defaultLabels[propertyName] ?? '';\r\n" +
            "    };\r\n");
    }

    public static IHtmlContent SelectAllHelperMethod(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        if (!model.FormControlsOfType<MultiSelectFormControl>().Any(multiSelect => multiSelect.Options.SelectAllOption != null))
        {
            return htmlHelper.Raw("");
        }

        return htmlHelper.Raw(
            "\r\n" +
            "    protected readonly toggleSelectAll = (propertyName: string, options: { value: unknown; displayName: unknown }[]): void => {\r\n" +
            "        const control = this.form.get(propertyName);\r\n" +
            "        const values = options.map(option => option.value);\r\n" +
            "        const selectedValues = ((control?.value as unknown[] | null) ?? []).filter(value => values.includes(value));\r\n" +
            "        control?.setValue(selectedValues.length === values.length ? [] : values);\r\n" +
            "        control?.markAsDirty();\r\n" +
            "    };\r\n");
    }

    public static IHtmlContent ReadonlyDisplayHelperMethods(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        if (!model.UseReadonlyDisplay)
        {
            return htmlHelper.Raw("");
        }

        return htmlHelper.Raw(
            "\r\n" +
            "    protected readonly readonlyValue = (propertyName: string): string =>\r\n" +
            "        String(this.form.get(propertyName)?.value ?? '');\r\n" +
            "\r\n" +
            "    protected readonly selectedDisplayName = (value: unknown, options: { value: unknown; displayName: unknown }[]): string =>\r\n" +
            "        Array.isArray(value)\r\n" +
            "            ? value.map(item => options.find(option => option.value === item)?.displayName ?? item).join(', ')\r\n" +
            "            : String(options.find(option => option.value === value)?.displayName ?? value ?? '');\r\n");
    }

    public static IHtmlContent MultiCheckboxHelperMethods(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        if (!model.FormControlsOfType<MultiCheckboxFormControl>().Any())
        {
            return htmlHelper.Raw("");
        }

        return htmlHelper.Raw(
            "\r\n" +
            "    protected readonly isOptionSelected = (propertyName: string, value: unknown): boolean =>\r\n" +
            "        ((this.form.get(propertyName)?.value as unknown[] | null) ?? []).includes(value);\r\n" +
            "\r\n" +
            "    protected readonly toggleOption = (propertyName: string, value: unknown, checked: boolean): void => {\r\n" +
            "        const control = this.form.get(propertyName);\r\n" +
            "        const currentValues = (control?.value as unknown[] | null) ?? [];\r\n" +
            "        control?.setValue(checked ? [...currentValues, value] : currentValues.filter(item => item !== value));\r\n" +
            "        control?.markAsDirty();\r\n" +
            "    };\r\n");
    }
}
