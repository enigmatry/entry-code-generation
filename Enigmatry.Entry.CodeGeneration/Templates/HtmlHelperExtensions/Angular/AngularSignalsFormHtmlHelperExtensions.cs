using System.Globalization;
using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormHtmlHelperExtensions
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

    public static string GetTypeScriptTypeAnnotation(this FormControl control) => control switch
    {
        MultiSelectFormControl => "<unknown[] | null>",
        MultiCheckboxFormControl => "<unknown[] | null>",
        RichTextInputFormControl => "<string | null>",
        InputControlBase input when input.IsNumeric() => "<number | null>",
        InputControlBase => "<string | null>",
        CheckboxFormControl => "<boolean>",
        _ => ""
    };

    public static string GetInitialValue(this FormControl control) => control switch
    {
        InputControlBase { DefaultValue: not null } input when input.IsNumeric() => AsJsLiteral(input.DefaultValue),
        InputControlBase input when input.DefaultValue != null => $"'{EscapeSingleQuotes(input.DefaultValue)}'",
        CheckboxFormControl { DefaultValue: not null } checkbox => checkbox.DefaultValue.Value.ToString().ToLower(),
        CheckboxFormControl => "false",
        SelectFormControl { DefaultValue: not null } select => AsJsLiteral(select.DefaultValue),
        RadioGroupFormControl { DefaultValue: not null } radioGroup => AsJsLiteral(radioGroup.DefaultValue),
        DatepickerFormControl { DefaultValue: not null } datepicker => $"'{datepicker.DefaultValue.Value.ToString("O", CultureInfo.InvariantCulture)}'",
        DateTimePickerFormControl { DefaultValue: not null } dateTimePicker => $"'{dateTimePicker.DefaultValue.Value.ToString("O", CultureInfo.InvariantCulture)}'",
        _ => "null"
    };

    private static string AsJsLiteral(string value) =>
        value is "true" or "false" || decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out _)
            ? value
            : $"'{EscapeSingleQuotes(value)}'";

    private static string EscapeSingleQuotes(string value) => value.Replace("'", "\\'");

    public static IHtmlContent FormControlDeclaration(this IHtmlHelper htmlHelper, FormControl control, string indent = "        ")
    {
        if (control is ArrayFormControl array)
        {
            return htmlHelper.Raw($"{indent}{array.PropertyName}: new FormArray<FormGroup>([]),\r\n");
        }

        var typeAnnotation = control.GetTypeScriptTypeAnnotation();
        var initialValue = control.GetInitialValue();
        var valueExpression = control.Readonly
            ? $"{{ value: {initialValue}, disabled: true }}"
            : initialValue;

        var options = BuildFormControlOptions(htmlHelper, control);

        var declaration = options.Count > 0
            ? $"{indent}{control.PropertyName}: new FormControl{typeAnnotation}({valueExpression}, {{ {String.Join(", ", options)} }}),"
            : $"{indent}{control.PropertyName}: new FormControl{typeAnnotation}({valueExpression}),";

        return htmlHelper.Raw(declaration + "\r\n");
    }

    public static IHtmlContent AllFormControlDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.FlatFormControls().Select(control => htmlHelper.FormControlDeclaration(control).ToString())));

    public static IHtmlContent ArrayItemFactoryMethod(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl)
    {
        var group = (FormControlGroup)arrayControl.FormControlGroup;
        var propertyName = arrayControl.PropertyName;
        var methodName = Capitalize(propertyName);

        var childDeclarations = group.FormControls
            .Where(control => control is not ButtonFormControl)
            .Select(control => htmlHelper.FormControlDeclaration(control, "            ").ToString());

        return htmlHelper.Raw(
            $"    protected readonly create{methodName}Item = (): FormGroup => {{\r\n" +
            $"        return new FormGroup({{\r\n" +
            String.Concat(childDeclarations) +
            $"        }});\r\n" +
            $"    }};\r\n");
    }

    public static IHtmlContent AllArrayItemFactoryMethods(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => htmlHelper.ArrayItemFactoryMethod(array).ToString())));

    public static IHtmlContent ArrayResizeMethod(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl)
    {
        var propertyName = arrayControl.PropertyName;
        var methodName = Capitalize(propertyName);

        return htmlHelper.Raw(
            $"    private readonly resize{methodName}Array = (length: number): void => {{\r\n" +
            $"        const formArray = this.form.get('{propertyName}') as FormArray<FormGroup>;\r\n" +
            $"        while (formArray.length > length) {{\r\n" +
            $"            formArray.removeAt(formArray.length - 1, {{ emitEvent: false }});\r\n" +
            $"        }}\r\n" +
            $"        while (formArray.length < length) {{\r\n" +
            $"            const item = this.create{methodName}Item();\r\n" +
            $"            if (this.form.disabled) {{\r\n" +
            $"                item.disable({{ emitEvent: false }});\r\n" +
            $"            }}\r\n" +
            $"            formArray.push(item, {{ emitEvent: false }});\r\n" +
            $"        }}\r\n" +
            $"    }};\r\n");
    }

    public static IHtmlContent AllArrayResizeMethods(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => htmlHelper.ArrayResizeMethod(array).ToString())));

    public static IHtmlContent ResizeArrayCalls(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var lines = model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => $"            this.resize{Capitalize(array.PropertyName)}Array(model.{array.PropertyName}?.length ?? 0);");

        return htmlHelper.Raw(String.Concat(lines.Select(line => line + "\r\n")));
    }

    public static IHtmlContent ReEnableStaticReadonlyControls(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var lines = model.FlatFormControls()
            .Where(control => control.Readonly && control is not ArrayFormControl)
            .Select(control => $"                this.form.get('{control.PropertyName}')?.disable({{ emitEvent: false }});");

        var arrayItemLines = model.FlatFormControls().OfType<ArrayFormControl>()
            .SelectMany(array => ((FormControlGroup)array.FormControlGroup).FormControls
                .Where(child => child.Readonly && child is not ButtonFormControl)
                .Select(child =>
                    $"                (this.form.get('{array.PropertyName}') as FormArray<FormGroup>).controls" +
                    $".forEach(itemGroup => itemGroup.get('{child.PropertyName}')?.disable({{ emitEvent: false }}));"));

        return htmlHelper.Raw(String.Join("\r\n", lines.Concat(arrayItemLines)));
    }

    private static string Capitalize(string propertyName) => Char.ToUpper(propertyName[0]) + propertyName.Substring(1);

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

        imports.AddRange(controls
            .Where(control => control.Import != null)
            .Select(control => (control.Import!.Symbol, control.Import!.Path))
            .Distinct());

        return imports;
    }

    private static IEnumerable<FormControl> AllControlsIncludingArrayItems(this FormComponentModel model) =>
        model.FormControls.AllControlsIncludingArrayItems();

    private static IEnumerable<FormControl> AllControlsIncludingArrayItems(this IEnumerable<FormControl> controls)
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

    public static IHtmlContent SelectInputDeclarations(this IHtmlHelper htmlHelper, SelectControlBase select, bool enableI18N)
    {
        var lines = new List<string>();

        if (select.Options.HasDynamicValues)
        {
            lines.Add($"    protected readonly {select.PropertyName}Callback = input<Observable<unknown[]> | null>(null);");
            lines.Add($"    private readonly {select.PropertyName}RawOptions = toSignal(toObservable(this.{select.PropertyName}Callback).pipe(switchMap(callback => callback ?? of([]))), {{ initialValue: [] }});");
        }
        else
        {
            var options = htmlHelper.JsArray(select.Options.FixedOptions,
                option => $"{{ {option.GetValueExpression()}, displayName: {htmlHelper.Localize(option.DisplayName, enableI18N)} }}");
            lines.Add($"    private readonly {select.PropertyName}RawOptions = signal({options});");
        }

        if (select.Options.HasCustomValueAndDisplayKeys)
        {
            lines.Add($"    protected readonly {select.PropertyName}OptionsConfiguration = input<SelectConfiguration>({{ valueProperty: '{select.Options.OptionValueKey}', labelProperty: '{select.Options.OptionDisplayKey}', sortProperty: '{select.Options.OptionSortKey}' }});");
        }
        else
        {
            lines.Add($"    protected readonly {select.PropertyName}OptionsConfiguration = input<SelectConfiguration>({select.Options.DefaultOptionsAsString});");
        }

        lines.Add($"    protected readonly {select.PropertyName}Options = computed(() => {{");
        lines.Add($"        const configuration = this.{select.PropertyName}OptionsConfiguration();");
        lines.Add($"        const options = sortOptions(this.{select.PropertyName}RawOptions() as Record<string, unknown>[], configuration.valueProperty ?? 'value', configuration.sortProperty ?? '', this.localeId) as Record<string, unknown>[];");
        lines.Add($"        return options.map(option => ({{ value: option[configuration.valueProperty ?? 'value'], displayName: option[configuration.labelProperty ?? 'displayName'] }}));");
        lines.Add($"    }});");

        if (select is AutocompleteFormControl)
        {
            var propertyNameCapitalized = Capitalize(select.PropertyName);
            lines.Add($"    protected readonly display{propertyNameCapitalized} = (value: unknown): string =>");
            lines.Add($"        (this.{select.PropertyName}Options().find(option => option.value === value)?.displayName as string) ?? '';");
        }

        return htmlHelper.Raw(String.Join("\r\n", lines) + "\r\n");
    }

    public static IHtmlContent AllSelectInputDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model, bool enableI18N) =>
        htmlHelper.Raw(String.Concat(model.FormControlsOfType<SelectControlBase>()
            .Select(select => htmlHelper.SelectInputDeclarations(select, enableI18N).ToString())));

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
            $"    }});\r\n");

        return htmlHelper.Raw("\r\n" + String.Concat(declarations));
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

    private static List<string> BuildFormControlOptions(IHtmlHelper htmlHelper, FormControl control)
    {
        var options = new List<string>();

        if (control.ValidationRules.Any())
        {
            options.Add($"validators: {htmlHelper.AngularValidators(control)}");
        }

        if (control.Validators.Any())
        {
            var asyncValidatorParts = String.Join(", ",
                control.Validators.Select(validator => $"this.asyncValidatorResolver('{validator.Name.Camelize()}')"));
            options.Add($"asyncValidators: this.asyncValidatorResolver ? [{asyncValidatorParts}] : []");
        }

        if (control.ValueUpdateTrigger.HasValue && control.ValueUpdateTrigger.Value != ValueUpdateTrigger.OnChange)
        {
            var updateOnValue = control.ValueUpdateTrigger.Value switch
            {
                ValueUpdateTrigger.OnBlur => "blur",
                ValueUpdateTrigger.OnSubmit => "submit",
                _ => "change"
            };
            options.Add($"updateOn: '{updateOnValue}'");
        }

        return options;
    }
}
