using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Humanizer;

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

    /// <summary>
    /// Trailing argument for isHidden/label/isDisabled calls: empty at root; inside an array row
    /// the current row item, so expressions for array children evaluate per row (Formly parity —
    /// a nested field's expression received the nested model).
    /// </summary>
    public string ExpressionArgument { get; init; } = "";

    /// <summary>Key a control is looked up under in the labels map and the expression-dictionary inputs.</summary>
    public string Key(FormControl field) => $"{ControlKeyPrefix}{field.PropertyName}";

    /// <summary>Generated isHidden(...) call for the control, with the row item appended inside an array.</summary>
    public string IsHiddenCall(FormControl field) =>
        $"isHidden('{Key(field)}', {field.Visible.ToString().ToLower()}{ExpressionArgument})";

    /// <summary>Generated isDisabled(...) call for the control, with the row item appended inside an array.</summary>
    public string IsDisabledCall(FormControl field) =>
        $"isDisabled('{Key(field)}', {field.Readonly.ToString().ToLower()}{ExpressionArgument})";

    /// <summary>Generated label(...) call for the control, with the row item appended inside an array.</summary>
    public string LabelCall(FormControl field) => $"label('{Key(field)}'{ExpressionArgument})";

    /// <summary>Name of a generated component member belonging to the control (e.g. suffix "Options").</summary>
    public string MemberName(FormControl field, string suffix) => MemberNamePrefix.Length == 0
        ? $"{field.PropertyName}{suffix}"
        : $"{MemberNamePrefix}{AngularSignalsFormModelExtensions.Capitalize(field.PropertyName)}{suffix}";

    /// <summary>
    /// Property segment of a translation id minted at render time, e.g. "mock-radio" at root or
    /// "addresses.city" inside the addresses array — so an array child never collides with a
    /// root control of the same name.
    /// </summary>
    public string TranslationIdSegment(FormControl field) => MemberNamePrefix.Length == 0
        ? field.PropertyName.Kebaberize()
        : $"{MemberNamePrefix.Kebaberize()}.{field.PropertyName.Kebaberize()}";
}
