using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

namespace Enigmatry.Entry.CodeGeneration.Angular;

/// <summary>
/// Guards combinations the signals templates cannot generate valid standalone components for,
/// and reports warnings for configurations that silently lose behavior (returned to the caller
/// for logging). Only runs for the signals path; the deprecated Formly templates resolve these
/// at runtime.
/// </summary>
public static class SignalsFormComponentValidator
{
    public static IReadOnlyList<string> Validate(FormComponentModel model)
    {
        ValidateArrayControls(model);
        ValidateCustomElementImports(model);
        ValidateGeneratedMemberNames(model);
        return CollectValidationRuleWarnings(model);
    }

    private static void ValidateArrayControls(FormComponentModel model)
    {
        foreach (var array in model.FlatFormControls().OfType<ArrayFormControl>())
        {
            var children = ((FormControlGroup)array.FormControlGroup).FormControls;
            if (children.AllControlsIncludingArrayItems().OfType<ArrayFormControl>().Any())
            {
                throw new InvalidOperationException(
                    $"Array control '{array.PropertyName}' on component '{model.ComponentInfo.Name}' contains a nested array control. " +
                    $"Nested arrays are not supported by the signals templates (item factories and declarations only descend one level).");
            }

            var autocomplete = children.FlatFormControls().OfType<AutocompleteFormControl>().FirstOrDefault();
            if (autocomplete != null)
            {
                throw new InvalidOperationException(
                    $"Autocomplete controls are not supported inside array items (per-row filtering state cannot be generated). " +
                    $"Property '{array.PropertyName}.{autocomplete.PropertyName}' on component '{model.ComponentInfo.Name}'.");
            }
        }
    }

    private static void ValidateCustomElementImports(FormComponentModel model)
    {
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

    /// <summary>
    /// Rejects two controls whose generated component members would have the same name — for
    /// example a root select named like an array child select ('addressesCountry' next to
    /// 'addresses[].country'), or a control whose name equals another's name plus a generated
    /// suffix ('regionFiltered' next to the autocomplete 'region', both yielding
    /// regionFilteredOptions). Every emitted member name is registered, because comparing name
    /// prefixes cannot catch the second shape.
    /// </summary>
    private static void ValidateGeneratedMemberNames(FormComponentModel model)
    {
        var memberOwners = new Dictionary<string, string>(StringComparer.Ordinal);

        void Register(string memberName, string propertyPath)
        {
            if (!memberOwners.TryAdd(memberName, propertyPath))
            {
                throw new InvalidOperationException(
                    $"Controls '{memberOwners[memberName]}' and '{propertyPath}' on component '{model.ComponentInfo.Name}' " +
                    $"generate colliding member names ('{memberName}'). Rename one of the properties.");
            }
        }

        foreach (var entry in model.SelectControlsWithMemberPrefixes())
        {
            var propertyPath = entry.MemberNamePrefix.Length == 0
                ? entry.Select.PropertyName
                : $"{entry.MemberNamePrefix}.{entry.Select.PropertyName}";

            foreach (var memberName in entry.Select.GeneratedMemberNames(entry.MemberNamePrefix))
            {
                Register(memberName, propertyPath);
            }
        }

        foreach (var array in model.FlatFormControls().OfType<ArrayFormControl>())
        {
            foreach (var memberName in array.GeneratedMemberNames())
            {
                Register(memberName, array.PropertyName);
            }
        }
    }

    // A rule that is not 'required' and carries no exact "RuleName: value" template option cannot
    // be turned into a generation-time Angular validator; surface that instead of dropping it silently.
    private static IReadOnlyList<string> CollectValidationRuleWarnings(FormComponentModel model) =>
        model.AllControlsIncludingArrayItems()
            .SelectMany(control => control.ValidationRules
                .Where(validationRule => !validationRule.TranslatesToAngularValidator())
                .Select(validationRule =>
                    $"Validation rule '{validationRule.GetRuleName()}' on '{model.ComponentInfo.Name}.{control.PropertyName}' has no " +
                    $"'{validationRule.GetRuleName()}: <value>' template option, so no Angular validator is generated for it. Its error message " +
                    $"only shows if a validator producing that error key is attached at runtime " +
                    $"(e.g. .WithValidators(\"{validationRule.GetRuleName()}\") resolved through ENTRY_ASYNC_VALIDATOR_RESOLVER)."))
            .ToList();
}
