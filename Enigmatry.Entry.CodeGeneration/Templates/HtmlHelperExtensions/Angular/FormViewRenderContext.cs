using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Component-level settings the signals form view renderers need while emitting markup.
/// The defaults describe root-level controls; <see cref="AngularSignalsFormViewHtmlHelperExtensions"/>
/// derives an array-item context so children of a FormArray row resolve against the row's
/// FormGroup and use collision-free keys and member names.
/// </summary>
public record FormViewRenderContext(bool EnableI18N, bool UseReadonlyDisplay)
{
    /// <summary>TypeScript expression for the FormGroup enclosing the rendered control ("form", or the @for loop variable inside an array row).</summary>
    public string FormGroupAccessor { get; init; } = "form";

    /// <summary>Prefix for dictionary keys (labels, hide/label expression inputs), e.g. "addresses." inside the addresses array.</summary>
    public string ControlKeyPrefix { get; init; } = "";

    /// <summary>Prefix for generated component member names, e.g. "addresses" turns cityOptions into addressesCityOptions.</summary>
    public string MemberNamePrefix { get; init; } = "";

    /// <summary>Key a control is looked up under in the labels map and the expression-dictionary inputs.</summary>
    public string Key(FormControl field) => $"{ControlKeyPrefix}{field.PropertyName}";

    /// <summary>Name of a generated component member belonging to the control (e.g. suffix "Options").</summary>
    public string MemberName(FormControl field, string suffix) => MemberNamePrefix.Length == 0
        ? $"{field.PropertyName}{suffix}"
        : $"{MemberNamePrefix}{AngularSignalsFormModelExtensions.Capitalize(field.PropertyName)}{suffix}";
}
