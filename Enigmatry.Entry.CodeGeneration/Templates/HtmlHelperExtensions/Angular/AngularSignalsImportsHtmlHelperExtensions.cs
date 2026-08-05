using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsImportsHtmlHelperExtensions
{
    private const string EntryFormPackage = "@enigmatry/entry-form";

    // Symbols from @enigmatry/entry-form are skipped here: the component template always emits
    // its own import statement for that package (expression dictionary types etc.), and the
    // component-import symbols are merged into it via EntryFormImportSymbols.
    public static IHtmlContent MaterialImportStatements(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.MaterialImports()
            .Where(import => import.Path != EntryFormPackage)
            .GroupBy(import => import.Path)
            .Select(pathGroup => $"import {{ {String.Join(", ", pathGroup.Select(import => import.Symbol).Distinct())} }} from '{pathGroup.Key}';\r\n")));

    public static string AngularImportsList(this FormComponentModel model) =>
        String.Join(", ", new[] { "ReactiveFormsModule" }
            .Concat(model.MaterialImports().Select(import => import.Symbol)));

    /// <summary>
    /// Symbols registered via WithImport that live in the @enigmatry/entry-form package, rendered
    /// as ", Symbol" suffixes. The component template appends them to its own entry-form import
    /// statement because <see cref="MaterialImportStatements"/> skips that package.
    /// </summary>
    public static string EntryFormImportSymbols(this FormComponentModel model)
    {
        var alreadyImportedSymbols = model.HasFormattedControls()
            ? new[] { "EntryFieldFormatDirective" }
            : Array.Empty<string>();

        return String.Concat(model.AllControlsIncludingArrayItems()
            .Where(control => control.Import != null && control.Import.Path == EntryFormPackage)
            .Select(control => control.Import!.Symbol)
            .Distinct()
            .Except(alreadyImportedSymbols)
            .Select(symbol => $", {symbol}"));
    }

    private static IReadOnlyList<(string Symbol, string Path)> MaterialImports(this FormComponentModel model)
    {
        var controls = model.AllControlsIncludingArrayItems().ToList();

        var usesMatInput = controls.Any(control => control
            is (InputControlBase and not RichTextInputFormControl)
            or AutocompleteFormControl or DatepickerFormControl or DateTimePickerFormControl);

        var imports = new List<(string Symbol, string Path)>
        {
            // The form-buttons ng-container in the view template.
            ("NgTemplateOutlet", "@angular/common")
        };

        if (model.UseReadonlyDisplay)
        {
            imports.AddRange(controls
                .Where(control => control.SupportsReadonlyDisplay() && control is not SelectControlBase)
                .Select(control => control.Formatter?.PipeSymbol())
                .Where(pipeSymbol => pipeSymbol != null)
                .Distinct()
                .Select(pipeSymbol => (pipeSymbol!, "@angular/common")));
        }

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

        // Always imported: every rendered field can emit <mat-error> (DynamicRequiredError covers
        // controls without a static required rule), and checkbox/radio/multicheckbox/rich-text/custom
        // renderers emit mat-error/mat-hint without a surrounding mat-form-field.
        imports.Add(("MatFormFieldModule", "@angular/material/form-field"));

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

        if (model.HasFormattedControls())
        {
            imports.Add(("EntryFieldFormatDirective", EntryFormPackage));
        }

        imports.AddRange(controls
            .Where(control => control.Import != null)
            .Select(control => (control.Import!.Symbol, control.Import!.Path))
            .Distinct());

        return imports;
    }
}
