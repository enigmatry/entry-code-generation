using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormModelExtensions
{
    public static IEnumerable<FormControl> FlatFormControls(this FormComponentModel model) =>
        model.FormControls.FlatFormControls();

    public static IEnumerable<FormControl> FlatFormControls(this IEnumerable<FormControl> controls)
    {
        foreach (var control in controls)
        {
            switch (control)
            {
                case FormControlGroup group:
                    foreach (var child in group.FormControls.FlatFormControls())
                    {
                        yield return child;
                    }
                    break;
                case ButtonFormControl:
                    break;
                default:
                    yield return control;
                    break;
            }
        }
    }

    internal static IEnumerable<FormControl> AllControlsIncludingArrayItems(this FormComponentModel model) =>
        model.FormControls.AllControlsIncludingArrayItems();

    internal static IEnumerable<FormControl> AllControlsIncludingArrayItems(this IEnumerable<FormControl> controls)
    {
        foreach (var control in controls)
        {
            yield return control;

            var children = control switch
            {
                FormControlGroup group => group.FormControls,
                ArrayFormControl array => ((FormControlGroup)array.FormControlGroup).FormControls,
                _ => null
            };

            if (children == null)
            {
                continue;
            }

            foreach (var child in children.AllControlsIncludingArrayItems())
            {
                yield return child;
            }
        }
    }

    public static bool HasAnyAsyncValidators(this FormComponentModel model) =>
        model.AllControlsIncludingArrayItems().Any(control => control.Validators.Any());

    public static bool HasDynamicSelectControls(this FormComponentModel model) =>
        model.AllControlsIncludingArrayItems().OfType<SelectControlBase>().Any(select => select.Options.HasDynamicValues);

    public static bool HasAutocompleteControls(this FormComponentModel model) =>
        model.AllControlsIncludingArrayItems().OfType<AutocompleteFormControl>().Any();

    public static bool HasFormattedControls(this FormComponentModel model) =>
        model.AllControlsIncludingArrayItems().Any(control => control.Formatter != null && control.Formatter.JsFormatterName.HasContent());

    public static bool HasControlValueHelperMethods(this FormComponentModel model) =>
        model.UseReadonlyDisplay
        || model.AllControlsIncludingArrayItems().OfType<MultiCheckboxFormControl>().Any()
        || model.AllControlsIncludingArrayItems().OfType<MultiSelectFormControl>().Any(multiSelect => multiSelect.Options.SelectAllOption != null);

    private static readonly HashSet<System.Type> NumericPropertyTypes = new()
    {
        typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint),
        typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal)
    };

    public static bool IsNumeric(this FormControl control)
    {
        var propertyType = control.PropertyType;
        if (propertyType == null)
        {
            return false;
        }

        return NumericPropertyTypes.Contains(Nullable.GetUnderlyingType(propertyType) ?? propertyType);
    }

    internal static string Capitalize(string propertyName) => propertyName.Length == 0
        ? propertyName
        : Char.ToUpperInvariant(propertyName[0]) + propertyName[1..];
}
