using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

namespace Enigmatry.Entry.CodeGeneration.Angular;

/// <summary>
/// Guards combinations the signals templates cannot generate valid standalone components for.
/// Only runs for the signals path; the deprecated Formly templates resolve these at runtime.
/// </summary>
public static class SignalsFormComponentValidator
{
    public static void Validate(FormComponentModel model)
    {
        // Throws for autocomplete controls inside array items, which the declarations cannot support.
        _ = model.SelectControlsWithMemberPrefixes().ToList();

        foreach (var array in model.FlatFormControls().OfType<ArrayFormControl>())
        {
            var children = ((FormControlGroup)array.FormControlGroup).FormControls;
            if (children.AllControlsIncludingArrayItems().OfType<ArrayFormControl>().Any())
            {
                throw new InvalidOperationException(
                    $"Array control '{array.PropertyName}' on component '{model.ComponentInfo.Name}' contains a nested array control. " +
                    $"Nested arrays are not supported by the signals templates (item factories and declarations only descend one level).");
            }
        }

        foreach (var control in model.AllControlsIncludingArrayItems())
        {
            var requiresImport = control is RichTextInputFormControl or CustomFormControl;
            if (requiresImport && control.Import == null)
            {
                throw new InvalidOperationException(
                    $"Control '{control.PropertyName}' on component '{model.ComponentInfo.Name}' renders a custom element, " +
                    $"but no import is configured. Add .WithImport(\"<ComponentSymbol>\", \"<npm-package>\") to the control " +
                    $"so the generated standalone component can import the element.");
            }
        }
    }
}
