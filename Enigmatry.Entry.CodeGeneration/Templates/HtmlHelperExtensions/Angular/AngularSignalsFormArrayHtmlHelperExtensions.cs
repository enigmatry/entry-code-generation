using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

public static class AngularSignalsFormArrayHtmlHelperExtensions
{
    public static IHtmlContent ArrayItemFactoryMethod(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl)
    {
        var group = (FormControlGroup)arrayControl.FormControlGroup;
        var propertyName = arrayControl.PropertyName;
        var methodName = AngularSignalsFormModelExtensions.Capitalize(propertyName);

        // FlatFormControls flattens nested groups and skips buttons, matching how the view
        // renderer emits the item's controls against the row FormGroup.
        var childDeclarations = group.FormControls
            .FlatFormControls()
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
        var methodName = AngularSignalsFormModelExtensions.Capitalize(propertyName);

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

    public static IHtmlContent ArrayMutationMethods(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl)
    {
        var propertyName = arrayControl.PropertyName;
        var methodName = AngularSignalsFormModelExtensions.Capitalize(propertyName);

        return htmlHelper.Raw(
            $"    protected readonly add{methodName}Item = (): void => {{\r\n" +
            $"        this.form.controls.{propertyName}.push(this.create{methodName}Item());\r\n" +
            $"        this.form.markAsDirty();\r\n" +
            $"    }};\r\n" +
            $"\r\n" +
            $"    protected readonly remove{methodName}Item = (index: number): void => {{\r\n" +
            $"        this.form.controls.{propertyName}.removeAt(index);\r\n" +
            $"        this.form.markAsDirty();\r\n" +
            $"    }};\r\n");
    }

    public static IHtmlContent AllArrayMutationMethods(this IHtmlHelper htmlHelper, FormComponentModel model) =>
        htmlHelper.Raw(String.Concat(model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => htmlHelper.ArrayMutationMethods(array).ToString())));

    public static IHtmlContent ResizeArrayCalls(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var lines = model.FlatFormControls().OfType<ArrayFormControl>()
            .Select(array => $"            this.resize{AngularSignalsFormModelExtensions.Capitalize(array.PropertyName)}Array(model.{array.PropertyName}?.length ?? 0);");

        return htmlHelper.Raw(String.Concat(lines.Select(line => line + "\r\n")));
    }

    // Re-applies the disabled state to statically readonly controls after the blanket
    // form.enable() that runs when the form leaves readonly mode.
    public static IHtmlContent DisableStaticReadonlyControls(this IHtmlHelper htmlHelper, FormComponentModel model)
    {
        var lines = model.FlatFormControls()
            .Where(control => control.Readonly && control is not ArrayFormControl)
            .Select(control => $"                this.form.get('{control.PropertyName}')?.disable({{ emitEvent: false }});");

        var arrayItemLines = model.FlatFormControls().OfType<ArrayFormControl>()
            .SelectMany(array => ((FormControlGroup)array.FormControlGroup).FormControls
                .FlatFormControls()
                .Where(child => child.Readonly && child is not ArrayFormControl)
                .Select(child =>
                    $"                (this.form.get('{array.PropertyName}') as FormArray<FormGroup>).controls" +
                    $".forEach(itemGroup => itemGroup.get('{child.PropertyName}')?.disable({{ emitEvent: false }}));"));

        return htmlHelper.Raw(String.Join("\r\n", lines.Concat(arrayItemLines)));
    }
}
