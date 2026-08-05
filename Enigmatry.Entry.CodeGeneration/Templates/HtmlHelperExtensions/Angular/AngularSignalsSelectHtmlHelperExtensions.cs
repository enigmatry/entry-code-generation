using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsSelectHtmlHelperExtensions
{
    public static IHtmlContent SelectInputDeclarations(this IHtmlHelper htmlHelper, SelectControlBase select, bool enableI18N, string memberNamePrefix = "")
    {
        string MemberName(string suffix) => SelectMemberName(select, memberNamePrefix, suffix);

        var lines = new List<string>();

        if (select.Options.HasDynamicValues)
        {
            lines.Add($"    protected readonly {MemberName("Callback")} = input<Observable<unknown[]> | null>(null);");
            lines.Add($"    private readonly {MemberName("RawOptions")} = toSignal(toObservable(this.{MemberName("Callback")}).pipe(switchMap(callback => callback ?? of([]))), {{ initialValue: [] }});");
        }
        else
        {
            var options = htmlHelper.JsArray(select.Options.FixedOptions,
                option => $"{{ {SelectOptionValueExpression(option)}, displayName: {htmlHelper.LocalizeEscaped(option.DisplayName, enableI18N)}" +
                          $"{OptionGroupProperty(htmlHelper, option, enableI18N)} }}");
            lines.Add($"    private readonly {MemberName("RawOptions")} = signal({options});");
        }

        // groupProperty is only emitted when grouping is configured, matching the shared
        // SelectOptions.DefaultOptionsAsString behavior (an always-present key would change the
        // sortOptions contract for every select).
        var groupKey = select.Options.OptionGroupKey;
        var groupProperty = groupKey.HasContent() ? $", groupProperty: '{groupKey!.EscapeTsSingleQuoted()}'" : "";

        if (select.Options.HasCustomValueAndDisplayKeys)
        {
            lines.Add($"    protected readonly {MemberName("OptionsConfiguration")} = input<SelectConfiguration>({{ valueProperty: '{select.Options.OptionValueKey.EscapeTsSingleQuoted()}', labelProperty: '{select.Options.OptionDisplayKey.EscapeTsSingleQuoted()}', sortProperty: '{select.Options.OptionSortKey.EscapeTsSingleQuoted()}'{groupProperty} }});");
        }
        else
        {
            // Signals-side mirror of SelectOptions.DefaultOptionsAsString with the consumer-provided
            // keys escaped; the shared property stays untouched for the deprecated Formly output.
            // Dynamic options carry no value/label/sort keys, but still need groupProperty when
            // grouping is configured — otherwise the group lookup falls back to 'group' and never
            // finds the configured key.
            var defaultConfiguration = select.Options.HasFixedValues
                ? $"{{ valueProperty: 'value', labelProperty: 'displayName', sortProperty: '{select.Options.OptionSortKey.Camelize().EscapeTsSingleQuoted()}'{groupProperty} }}"
                : groupKey.HasContent()
                    ? $"{{ groupProperty: '{groupKey!.EscapeTsSingleQuoted()}' }}"
                    : "{}";
            lines.Add($"    protected readonly {MemberName("OptionsConfiguration")} = input<SelectConfiguration>({defaultConfiguration});");
        }

        var sortGroupArgument = groupKey.HasContent() ? ", configuration.groupProperty" : "";
        var groupMapping = select.RendersOptionGroups() ? ", group: option[configuration.groupProperty ?? 'group']" : "";
        lines.Add($"    protected readonly {MemberName("Options")} = computed(() => {{");
        lines.Add($"        const configuration = this.{MemberName("OptionsConfiguration")}();");
        lines.Add($"        const options = sortOptions(this.{MemberName("RawOptions")}() as Record<string, unknown>[], configuration.valueProperty ?? 'value', configuration.sortProperty ?? '', this.localeId{sortGroupArgument}) as Record<string, unknown>[];");
        lines.Add($"        return options.map(option => ({{ value: option[configuration.valueProperty ?? 'value'], displayName: option[configuration.labelProperty ?? 'displayName']{groupMapping} }}));");
        lines.Add($"    }});");

        // An autocomplete groups its FILTERED options instead (see AllAutocompleteFilterDeclarations),
        // so it gets no grouping member here.
        if (select.RendersOptionGroups() && select is not AutocompleteFormControl)
        {
            lines.AddRange(OptionGroupsComputed(MemberName("OptionGroups"), MemberName("Options")));
        }

        if (select is AutocompleteFormControl)
        {
            var propertyNameCapitalized = AngularSignalsFormModelExtensions.Capitalize(select.PropertyName);
            lines.Add($"    protected readonly display{propertyNameCapitalized} = (value: unknown): string =>");
            lines.Add($"        (this.{MemberName("Options")}().find(option => option.value === value)?.displayName as string) ?? '';");
        }

        return htmlHelper.Raw(String.Join("\r\n", lines) + "\r\n");
    }

    internal static string SelectMemberName(SelectControlBase select, string memberNamePrefix, string suffix) =>
        memberNamePrefix.Length == 0
            ? $"{select.PropertyName}{suffix}"
            : $"{memberNamePrefix}{AngularSignalsFormModelExtensions.Capitalize(select.PropertyName)}{suffix}";

    /// <summary>
    /// Every component member this select contributes to the generated class. Kept in step with
    /// <see cref="SelectInputDeclarations"/> and <see cref="AllAutocompleteFilterDeclarations"/>
    /// (same conditions, same suffixes) so SignalsFormComponentValidator can reject two controls
    /// whose members would collide — comparing name prefixes is not enough, because one control's
    /// prefix plus a suffix can equal another control's full member name.
    /// </summary>
    internal static IEnumerable<string> GeneratedMemberNames(this SelectControlBase select, string memberNamePrefix)
    {
        string MemberName(string suffix) => SelectMemberName(select, memberNamePrefix, suffix);

        if (select.Options.HasDynamicValues)
        {
            yield return MemberName("Callback");
        }

        yield return MemberName("RawOptions");
        yield return MemberName("OptionsConfiguration");
        yield return MemberName("Options");

        if (select.RendersOptionGroups() && select is not AutocompleteFormControl)
        {
            yield return MemberName("OptionGroups");
        }

        if (select is not AutocompleteFormControl)
        {
            yield break;
        }

        yield return $"display{AngularSignalsFormModelExtensions.Capitalize(select.PropertyName)}";
        yield return MemberName("FilterValue");
        yield return MemberName("FilteredOptions");

        if (select.RendersOptionGroups())
        {
            yield return MemberName("FilteredOptionGroups");
        }
    }

    /// <summary>
    /// Groups an options signal into <c>{ group, options }</c> entries for mat-optgroup rendering,
    /// preserving the order sortOptions produced. Options without a group value collect under an
    /// empty group key, which the view renders without an optgroup wrapper.
    /// </summary>
    private static IEnumerable<string> OptionGroupsComputed(string groupsMemberName, string optionsMemberName) =>
    [
        $"    protected readonly {groupsMemberName} = computed(() => {{",
        $"        const groups: {{ group: string; options: {{ value: unknown; displayName: unknown }}[] }}[] = [];",
        $"        this.{optionsMemberName}().forEach(option => {{",
        $"            const group = String((option as {{ group?: unknown }}).group ?? '');",
        $"            const existing = groups.find(candidate => candidate.group === group);",
        $"            if (existing) {{",
        $"                existing.options.push(option);",
        $"            }} else {{",
        $"                groups.push({{ group, options: [option] }});",
        $"            }}",
        $"        }});",
        $"        return groups;",
        $"    }});"
    ];

    /// <summary>
    /// Whether the control renders its options as mat-optgroups: grouping must be configured
    /// (a group key for dynamic options, or at least one fixed option carrying a group — e.g.
    /// through [SelectOptionGroup] on an enum member) AND the control must be one whose markup can
    /// host optgroups. Radio groups and multi-checkboxes still carry the group on each option, but
    /// have no optgroup equivalent, so no grouping members are generated for them.
    /// </summary>
    internal static bool RendersOptionGroups(this SelectControlBase select) =>
        select is SelectFormControl or MultiSelectFormControl or AutocompleteFormControl
        && (select.Options.OptionGroupKey.HasContent()
            || select.Options.FixedOptions.Any(option => option.Group != null && option.Group.Value.HasContent()));

    private static string OptionGroupProperty(IHtmlHelper htmlHelper, SelectOption option, bool enableI18N) =>
        option.Group != null && option.Group.Value.HasContent()
            ? $", group: {htmlHelper.LocalizeEscaped(option.Group, enableI18N)}"
            : "";

    // Signals-side mirror of SelectOption.GetValueExpression with string escaping; the shared
    // helper stays untouched because the deprecated Formly templates rely on its exact output.
    private static string SelectOptionValueExpression(SelectOption option)
    {
        var value = option.Value;
        if (value != null && value.IsNumeric())
        {
            return $"value: {(value is Enum ? (int)value : value)}";
        }

        if (value is bool boolValue)
        {
            return $"value: {boolValue.ToString().ToLowerInvariant()}";
        }

        return $"value: {(value == null ? "null" : $"'{value.ToString()!.EscapeTsSingleQuoted()}'")}";
    }

    public static IHtmlContent AllSelectInputDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model, bool enableI18N) =>
        htmlHelper.Raw(String.Concat(model.SelectControlsWithMemberPrefixes()
            .Select(entry => htmlHelper.SelectInputDeclarations(entry.Select, enableI18N, entry.MemberNamePrefix).ToString())));

    internal static IEnumerable<(SelectControlBase Select, string MemberNamePrefix)> SelectControlsWithMemberPrefixes(this FormComponentModel model)
    {
        foreach (var select in model.FormControlsOfType<SelectControlBase>())
        {
            yield return (select, "");
        }

        // Autocomplete controls inside array items are rejected up front by
        // SignalsFormComponentValidator, so every select yielded here can be declared.
        foreach (var array in model.FlatFormControls().OfType<ArrayFormControl>())
        {
            var children = ((FormControlGroup)array.FormControlGroup).FormControls;
            foreach (var select in children.FlatFormControls().OfType<SelectControlBase>())
            {
                yield return (select, array.PropertyName);
            }
        }
    }

    public static IHtmlContent AllAutocompleteFilterDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var autocompleteControls = model.FormControlsOfType<AutocompleteFormControl>().ToList();
        if (autocompleteControls.Count == 0)
        {
            return htmlHelper.Raw("");
        }

        var declarations = autocompleteControls.Select(autocomplete =>
            $"    private readonly {autocomplete.PropertyName}FilterValue = toSignal(this.form.controls.{autocomplete.PropertyName}.valueChanges, {{ initialValue: null }});\r\n" +
            $"    protected readonly {autocomplete.PropertyName}FilteredOptions = computed(() => {{\r\n" +
            $"        const filterValue = this.{autocomplete.PropertyName}FilterValue();\r\n" +
            $"        const filterText = typeof filterValue === 'string' ? filterValue.toLowerCase() : '';\r\n" +
            $"        return this.{autocomplete.PropertyName}Options().filter(option =>\r\n" +
            $"            option.value === filterValue || String(option.displayName).toLowerCase().includes(filterText));\r\n" +
            $"    }});\r\n" +
            // A grouped autocomplete groups the FILTERED options, so optgroups disappear as their
            // last matching option is filtered out.
            (autocomplete.RendersOptionGroups()
                ? String.Join("\r\n", OptionGroupsComputed($"{autocomplete.PropertyName}FilteredOptionGroups", $"{autocomplete.PropertyName}FilteredOptions")) + "\r\n"
                : ""));

        return htmlHelper.Raw("\r\n" + String.Concat(declarations));
    }
}
