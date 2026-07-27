using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsImportsHtmlHelperExtensions
{
    public static IHtmlContent MaterialImportStatements(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.MaterialImports()
            .Select(import => $"import {{ {import.Symbol} }} from '{import.Path}';\r\n")));

    public static string AngularImportsList(this FormComponentModel model) =>
        String.Join(", ", new[] { "ReactiveFormsModule", "NgTemplateOutlet" }
            .Concat(model.MaterialImports().Select(import => import.Symbol)));

    private static IReadOnlyList<(string Symbol, string Path)> MaterialImports(this FormComponentModel model)
    {
        var controls = model.AllControlsIncludingArrayItems().ToList();

        var usesFormField = controls.Any(control => control
            is (InputControlBase and not RichTextInputFormControl)
            or SelectFormControl or MultiSelectFormControl or AutocompleteFormControl
            or DatepickerFormControl or DateTimePickerFormControl);
        var usesMatInput = controls.Any(control => control
            is (InputControlBase and not RichTextInputFormControl)
            or AutocompleteFormControl or DatepickerFormControl or DateTimePickerFormControl);

        var imports = new List<(string Symbol, string Path)>();

        if (controls.OfType<AutocompleteFormControl>().Any())
        {
            imports.Add(("MatAutocompleteModule", "@angular/material/autocomplete"));
        }

        // The default form action buttons always render as mat-button.
        imports.Add(("MatButtonModule", "@angular/material/button"));

        if (controls.Any(control => control is CheckboxFormControl or MultiCheckboxFormControl))
        {
            imports.Add(("MatCheckboxModule", "@angular/material/checkbox"));
        }

        if (controls.OfType<DatepickerFormControl>().Any())
        {
            imports.Add(("MatDatepickerModule", "@angular/material/datepicker"));
        }

        if (usesFormField)
        {
            imports.Add(("MatFormFieldModule", "@angular/material/form-field"));
        }

        if (usesMatInput)
        {
            imports.Add(("MatInputModule", "@angular/material/input"));
        }

        if (controls.OfType<RadioGroupFormControl>().Any())
        {
            imports.Add(("MatRadioModule", "@angular/material/radio"));
        }

        if (controls.Any(control => control is SelectFormControl or MultiSelectFormControl))
        {
            imports.Add(("MatSelectModule", "@angular/material/select"));
        }

        if (controls.OfType<TextareaFormControl>().Any(textarea => textarea.AutoResize))
        {
            imports.Add(("TextFieldModule", "@angular/cdk/text-field"));
        }

        if (controls.Any(control => control.Tooltip.Value.HasContent()))
        {
            imports.Add(("MatTooltipModule", "@angular/material/tooltip"));
        }

        if (controls.OfType<DateTimePickerFormControl>().Any())
        {
            imports.Add(("MatDatetimepickerModule", "@mat-datetimepicker/core"));
        }

        if (controls.Any(control => control.Formatter != null && control.Formatter.JsFormatterName.HasContent()))
        {
            imports.Add(("EntryFieldFormatDirective", "@enigmatry/entry-form"));
        }

        imports.AddRange(controls
            .Where(control => control.Import != null)
            .Select(control => (control.Import!.Symbol, control.Import!.Path))
            .Distinct());

        return imports;
    }
}
