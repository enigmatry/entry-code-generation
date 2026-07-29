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

        return htmlHelper.Raw(
            "    private readonly controlStates = [\r\n" +
            String.Concat(entries.Select(entry => entry + "\r\n")) +
            "    ];\r\n");
    }

    /// <summary>
    /// Lines that re-disable statically readonly array-item controls after their FormArray was
    /// re-enabled (enable() on a parent cascades and wipes the per-row disabled state).
    /// </summary>
    public static IHtmlContent ReapplyArrayRowReadonlyLines(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var blocks = model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => (Array: array, ReadonlyChildren: array.ArrayItemControlsWithEffectiveReadonly()
                .Where(entry => entry.EffectiveReadonly)
                .Select(entry => entry.Control)
                .ToList()))
            .Where(entry => entry.ReadonlyChildren.Count > 0)
            .Select(entry =>
                $"            if (!this.form.controls.{entry.Array.PropertyName}.disabled) {{\r\n" +
                $"                this.form.controls.{entry.Array.PropertyName}.controls.forEach(itemGroup => {{\r\n" +
                String.Concat(entry.ReadonlyChildren.Select(child =>
                    $"                    itemGroup.get('{child.PropertyName}')?.disable({{ emitEvent: false }});\r\n")) +
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
