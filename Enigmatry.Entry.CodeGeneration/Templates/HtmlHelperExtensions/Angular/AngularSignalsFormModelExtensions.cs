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
        model.FlatFormControls().Any(control => control.Validators.Any());

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

    internal static string Capitalize(string propertyName) => Char.ToUpper(propertyName[0]) + propertyName.Substring(1);
}
