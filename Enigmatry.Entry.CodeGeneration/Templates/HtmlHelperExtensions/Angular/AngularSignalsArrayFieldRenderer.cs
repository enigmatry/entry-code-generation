using Enigmatry.Entry.CodeGeneration.Configuration;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.Entry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;

internal static class AngularSignalsArrayFieldRenderer
{
    internal static IHtmlContent RenderArrayField(this IHtmlHelper htmlHelper, ArrayFormControl arrayControl, FormViewRenderContext context)
    {
        var group = (FormControlGroup)arrayControl.FormControlGroup;
        var propertyName = arrayControl.PropertyName;
        var methodName = AngularSignalsFormModelExtensions.Capitalize(propertyName);
        var itemGroupVariable = $"{propertyName}ItemGroup";
        var itemContext = context with
        {
            FormGroupAccessor = itemGroupVariable,
            ControlKeyPrefix = $"{propertyName}.",
            MemberNamePrefix = propertyName,
            ExpressionArgument = $", currentModel().{propertyName}?.[$index]"
        };
        var innerControls = htmlHelper.RenderFormControls(group.FormControls, itemContext).ToString();
        // Add/remove follow the array control's disabled state (readonly mode, static readonly,
        // or a disable expression), not just the global readonly flag.
        var mutationGuard = $"!{context.IsDisabledCall(arrayControl)}";
        return htmlHelper.Raw(
            $"@if (!{context.IsHiddenCall(arrayControl)}) {{\r\n" +
            $"<div {arrayControl.ArrayClassAttribute()}>\r\n" +
            $"<ng-container formArrayName=\"{propertyName}\">\r\n" +
            $"    @for ({itemGroupVariable} of form.controls.{propertyName}.controls; track $index) {{\r\n" +
            $"        <ng-container [formGroupName]=\"$index\">\r\n" +
            innerControls +
            $"            @if ({mutationGuard}) {{\r\n" +
            $"            <button mat-button type=\"button\" class=\"entry-array-remove-button\" (click)=\"remove{methodName}Item($index)\"{arrayControl.RemoveButtonLabel.I18NAttribute(context.EnableI18N)}>{arrayControl.RemoveButtonLabel.Value.EscapeHtmlText()}</button>\r\n" +
            $"            }}\r\n" +
            $"        </ng-container>\r\n" +
            $"    }}\r\n" +
            $"    @if ({mutationGuard}) {{\r\n" +
            $"    <button mat-button type=\"button\" class=\"entry-array-add-button\" (click)=\"add{methodName}Item()\"{arrayControl.AddButtonLabel.I18NAttribute(context.EnableI18N)}>{arrayControl.AddButtonLabel.Value.EscapeHtmlText()}</button>\r\n" +
            $"    }}\r\n" +
            $"</ng-container>\r\n" +
            $"</div>\r\n" +
            $"}}\r\n");
    }

    // The wrapping div carries the array's configured classes and custom control type (an
    // ng-container cannot carry classes). ControlType falls back to the custom type name, which
    // may be empty — hence the explicit base classes instead of FieldClassAttribute.
    private static string ArrayClassAttribute(this ArrayFormControl arrayControl)
    {
        var staticClasses = $"entry-{arrayControl.PropertyName.Kebaberize()}-field entry-array-field";
        if (arrayControl.ControlTypeName.HasContent())
        {
            staticClasses += $" {arrayControl.ControlTypeName}";
        }

        staticClasses = arrayControl.ClassNames.Values
            .Where(classNameEntry => classNameEntry.When == ApplyWhen.Always)
            .Aggregate(staticClasses, (current, classNameEntry) => current + $" {classNameEntry.Value}");

        return $"class=\"{staticClasses}\"{arrayControl.ConditionalClassBindings()}";
    }
}
