using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Enigmatry.Entry.CodeGeneration.Configuration.Builder;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using Enigmatry.Entry.CodeGeneration.Validation;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;
using JetBrains.Annotations;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form;

/// <summary>
/// The FormComponentBuilder class provides a fluent API for configuring a form component
/// so that configuration can be used to generate UI client feature component e.g. Angular module.
/// </summary>
/// <remarks>
/// <para>
/// It provides methods to configure various aspects of the form component, such as component name,
/// feature name, etc. as described in <see cref="ComponentInfoBuilder"/>,
/// as well as the properties and behaviors of each form control,
/// such as input fields, checkboxes, and buttons as described in <see cref="BaseControlBuilder{TControl, TBuilder}"/>
/// </para>
/// <example>
/// An example of using the FormComponentBuilder to configure a table component:
/// <code>
/// builder.Component()
///     .HasName("ProductEdit")
///     .BelongsToFeature("Products")
///     .OrderBy(OrderByType.Configuration);
///
/// builder.FormControl(x => x.Price)
///     .WithLabel("Price per unit")
///     .WithLabelTranslationId(ProductTranslationId.Price)
///     .WithPlaceholder("Price per unit")
///     .WithPlaceholderTranslationId(ProductTranslationId.Price)
///     .WithAppearance(FormControlAppearance.Outline)
///     .WithFormat(new CurrencyPropertyFormatter().WithCurrencyCode("EUR").WithDisplay("€"))
///     .WithValidators("priceIsNotNegative");
/// </code>
/// </example>
/// </remarks>
[UsedImplicitly]
public class FormComponentBuilder<T> : BaseComponentBuilder<FormComponentModel>
{
    private readonly FormControlGroupBuilder<T> _formGroup = new FormControlGroupBuilder<T>("form");
    private IEnumerable<IFormlyValidationRule> _validationRules = new List<IFormlyValidationRule>();

    public FormComponentBuilder() : base(typeof(T))
    {
        _componentInfoBuilder.Routing().WithIdRoute();
    }

    public InferredFormControlBuilder FormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.FormControl(propertyExpression);
    }

    public InputFormControlBuilder InputFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.InputFormControl(propertyExpression);
    }
    public EmailFormControlBuilder EmailFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.EmailFormControl(propertyExpression);
    }
    public PasswordFormControlBuilder PasswordFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.PasswordFormControl(propertyExpression);
    }

    public CheckboxFormControlBuilder CheckboxFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.CheckboxFormControl(propertyExpression);
    }

    public TextareaFormControlBuilder TextareaFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.TextareaFormControl(propertyExpression);
    }

    public SelectFormControlBuilder SelectFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.SelectFormControl(propertyExpression);
    }

    public MultiSelectFormControlBuilder MultiSelectFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.MultiSelectFormControl(propertyExpression);
    }

    public RadioGroupFormControlBuilder RadioGroupFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.RadioGroupFormControl(propertyExpression);
    }

    public MultiCheckboxFormControlBuilder MultiCheckboxFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.MultiCheckboxFormControl(propertyExpression);
    }

    public AutocompleteFormControlBuilder AutocompleteFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.AutocompleteFormControl(propertyExpression);
    }

    public DatepickerFormControlBuilder DatepickerFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.DatepickerFormControl(propertyExpression);
    }

    public CustomFormControlBuilder CustomFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return _formGroup.CustomFormControl(propertyExpression);
    }

    public CustomFormControlBuilder CustomFormControl(string propertyName)
    {
        return _formGroup.CustomFormControl(propertyName);
    }

    public FormControlGroupBuilder<T> FormControlGroup(string name)
    {
        return _formGroup.FormControlGroup(name);
    }

    public ButtonFormControlBuilder ButtonFormControl(string name)
    {
        return _formGroup.ButtonFormControl(name);
    }

    public ArrayFormControlBuilder<TProperty> ArrayFormControl<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> propertyExpression)
    {
        return _formGroup.ArrayFormControl(propertyExpression);
    }

    public void WithValidationConfiguration(IHasFormlyValidationRules validationConfiguration)
    {
        _validationRules = validationConfiguration.ValidationRules;
    }

    public override FormComponentModel Build()
    {
        var componentInfo = _componentInfoBuilder.Build();
        var formControls = BuildFormControls(componentInfo).ToList();

        return new FormComponentModel(componentInfo, formControls, _validationRules);
    }

    private IEnumerable<FormControl> BuildFormControls(ComponentInfo componentInfo)
    {
        var formControls = _formGroup.BuildFormControls(componentInfo);

        if (componentInfo.IncludeUnconfiguredProperties)
        {
            var unconfiguredFormControls = BuildUnconfiguredFormControls(componentInfo);
            formControls = formControls.Concat(unconfiguredFormControls).ToList();
        }

        if (formControls.OfType<FormControlGroup>().Any())
        {
            // When using FormControlGroups
            // we assume that configuration will control the order (OrderBy -> Configuration)
            return formControls;
        }

        if (componentInfo.OrderByType == OrderByType.Model)
        {
            var properties = _modelType.GetProperties().Select(propertyInfo => propertyInfo.Name.ToUpper()).ToList();
            return formControls.OrderBy(formControl => properties.IndexOf(formControl.PropertyName.ToUpper()));
        }

        // OrderBy = Configuration
        return formControls;
    }

    private IEnumerable<FormControl> BuildUnconfiguredFormControls(ComponentInfo componentInfo)
    {
        return _modelType.GetProperties()
            .Where(propertyInfo => !_formGroup.HasControlBuilder(propertyInfo))
            .Select(propertyInfo => new InferredFormControlBuilder(propertyInfo).Build(componentInfo))
            .ToList();
    }
}
