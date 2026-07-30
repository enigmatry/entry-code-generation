using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

/// <summary>
/// Generates the per-control state table the component's single hidden/disabled effect walks:
/// a hidden control (statically invisible or hidden by an expression) is disabled so it stops
/// contributing to form validity, and disabled state combines readonly mode, static readonly
/// (including enclosing group readonly), and disable expressions in one place.
/// </summary>
public static class AngularSignalsControlStateHtmlHelperExtensions
{
    public static IHtmlContent ControlStateDeclarations(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var entries = model.FormControls.ControlsWithEffectiveReadonly(ancestorReadonly: false)
            .Select(entry =>
                $"        {{ key: '{entry.Control.PropertyName}', staticVisible: {entry.Control.Visible.ToString().ToLower()}, staticReadonly: {entry.EffectiveReadonly.ToString().ToLower()} }},");

        var itemTables = model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array =>
                $"    private readonly {array.PropertyName}ItemControlStates = [\r\n" +
                String.Concat(array.ArrayItemControlsWithEffectiveReadonly().Select(entry =>
                    $"        {{ key: '{array.PropertyName}.{entry.Control.PropertyName}', controlName: '{entry.Control.PropertyName}', " +
                    $"staticVisible: {entry.Control.Visible.ToString().ToLower()}, staticReadonly: {entry.EffectiveReadonly.ToString().ToLower()} }},\r\n")) +
                $"    ];\r\n");

        return htmlHelper.Raw(
            "    private readonly controlStates = [\r\n" +
            String.Concat(entries.Select(entry => entry + "\r\n")) +
            "    ];\r\n" +
            String.Concat(itemTables));
    }

    /// <summary>
    /// Per-row state lines for the control-state effect: array-item controls are re-evaluated per
    /// row (hidden children are disabled so they stop blocking submission, statically readonly
    /// children are re-disabled after enable() on the FormArray cascaded and wiped them). Skipped
    /// while the array control itself is disabled — the cascade already covers every child.
    /// </summary>
    public static IHtmlContent ArrayItemControlStateLines(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var blocks = model.FlatFormControls().OfType<ArrayFormControl>()
            .Where(array => array.ArrayItemControlsWithEffectiveReadonly().Any())
            .Select(array =>
                $"            if (!this.form.controls.{array.PropertyName}.disabled) {{\r\n" +
                $"                this.form.controls.{array.PropertyName}.controls.forEach((itemGroup, index) => {{\r\n" +
                $"                    const rowModel = this.{array.PropertyName}RowModel(index);\r\n" +
                $"                    this.{array.PropertyName}ItemControlStates.forEach(({{ key, controlName, staticVisible, staticReadonly }}) => {{\r\n" +
                $"                        const control = itemGroup.get(controlName);\r\n" +
                $"                        if (!control) {{\r\n" +
                $"                            return;\r\n" +
                $"                        }}\r\n" +
                $"                        const disabled = this.isHidden(key, staticVisible, rowModel) || staticReadonly;\r\n" +
                $"                        if (disabled !== control.disabled) {{\r\n" +
                $"                            if (disabled) {{\r\n" +
                $"                                control.disable({{ emitEvent: false }});\r\n" +
                $"                            }} else {{\r\n" +
                $"                                control.enable({{ emitEvent: false }});\r\n" +
                $"                            }}\r\n" +
                $"                        }}\r\n" +
                $"                    }});\r\n" +
                $"                }});\r\n" +
                $"            }}\r\n");

        return htmlHelper.Raw(String.Concat(blocks));
    }

    internal static IEnumerable<(FormControl Control, bool EffectiveReadonly)> ArrayItemControlsWithEffectiveReadonly(this ArrayFormControl arrayControl)
    {
        var itemGroup = (FormControlGroup)arrayControl.FormControlGroup;
        return itemGroup.FormControls.ControlsWithEffectiveReadonly(itemGroup.Readonly);
    }

    private static IEnumerable<(FormControl Control, bool EffectiveReadonly)> ControlsWithEffectiveReadonly(this IEnumerable<FormControl> controls, bool ancestorReadonly)
    {
        foreach (var control in controls)
        {
            switch (control)
            {
                case ButtonFormControl:
                    break;
                case FormControlGroup group:
                    foreach (var child in group.FormControls.ControlsWithEffectiveReadonly(ancestorReadonly || group.Readonly))
                    {
                        yield return child;
                    }
                    break;
                default:
                    yield return (control, ancestorReadonly || control.Readonly);
                    break;
            }
        }
    }
}
