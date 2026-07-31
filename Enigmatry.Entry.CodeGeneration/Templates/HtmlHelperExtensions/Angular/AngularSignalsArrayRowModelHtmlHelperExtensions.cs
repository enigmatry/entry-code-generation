using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Per-array bookkeeping of the original model rows: <c>&lt;prop&gt;OriginalRows</c> mirrors the
/// model's rows and is kept aligned with the FormArray on add/remove, so removing a middle row
/// keeps every remaining row paired with ITS original object (a plain index into the model array
/// would graft the removed row's properties onto its successor). <c>&lt;prop&gt;RowModel</c> merges
/// the original row under the current raw row and is what row expressions and the submit merge see.
/// </summary>
public static class AngularSignalsArrayRowModelHtmlHelperExtensions
{
    /// <summary>
    /// Every component member this array contributes to the generated class, across the row-model,
    /// factory, resize, mutation and control-state emitters. Kept in step with those emitters so
    /// SignalsFormComponentValidator can reject colliding generated members.
    /// </summary>
    internal static IEnumerable<string> GeneratedMemberNames(this ArrayFormControl arrayControl)
    {
        var propertyName = arrayControl.PropertyName;
        var methodName = AngularSignalsFormModelExtensions.Capitalize(propertyName);

        yield return $"{propertyName}OriginalRows";
        yield return $"{propertyName}RowModel";
        yield return $"create{methodName}Item";
        yield return $"resize{methodName}Array";
        yield return $"add{methodName}Item";
        yield return $"remove{methodName}Item";

        if (arrayControl.ArrayItemControlsWithEffectiveReadonly().Any())
        {
            yield return $"{propertyName}ItemControlStates";
        }
    }

    public static IHtmlContent ArrayOriginalRowsDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var declarations = model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array =>
                $"    private {array.PropertyName}OriginalRows: object[] = [];\r\n" +
                $"    protected readonly {array.PropertyName}RowModel = (index: number): unknown =>\r\n" +
                $"        ({{ ...(this.{array.PropertyName}OriginalRows[index] ?? {{}}), ...(this.currentModel().{array.PropertyName}?.[index] ?? {{}}) }});\r\n");

        return htmlHelper.Raw(String.Concat(declarations));
    }

    /// <summary>Lines for the model patch effect: a new model is the new source of truth for original rows.</summary>
    public static IHtmlContent RefreshOriginalRowsCalls(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var lines = model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => $"            this.{array.PropertyName}OriginalRows = [...(model.{array.PropertyName} ?? [])];");

        return htmlHelper.Raw(String.Concat(lines.Select(line => line + "\r\n")));
    }

    // Emitted into the onSubmit merge so each array row keeps the unconfigured properties of its
    // original model row (a plain getRawValue() spread would replace whole rows). Rows added in
    // the UI merge with an empty object.
    public static IHtmlContent ArrayRowMergeProperties(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var properties = model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array =>
                $", {array.PropertyName}: rawValue.{array.PropertyName}" +
                $".map((row, index) => ({{ ...(this.{array.PropertyName}OriginalRows[index] ?? {{}}), ...row }}))");

        return htmlHelper.Raw(String.Concat(properties));
    }
}
