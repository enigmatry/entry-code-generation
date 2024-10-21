using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls.Array;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

/// <summary>
/// Creates a FormControlGroupBuilder instance with the specified name, allowing you to configure a named group of form controls.
/// </summary>
/// <remarks>
/// <para>
/// Use this builder to create UI sections with wrapper elements, add various types of form controls (e.g., input, textarea, select, etc.), and manage form control configuration.
/// </para>
/// <code>
/// var formGroup = builder
///     .FormControlGroup("Group Name")
///     .CreateUiSection("group-type")
///     .WithCustomWrapper("group-wrapper")
///     .WithClassName("group-wrapper-readonly", ApplyWhen.FormIsReadonly);
/// </code>
/// </remarks>
/// <typeparam name="T">The type of the object being configured with the form control group.</typeparam>
[UsedImplicitly]
public class FormControlGroupBuilder<T> : BaseControlBuilder<FormControlGroup, FormControlGroupBuilder<T>>
{
    private string? _wrapperElement;
    private readonly IList<IControlBuilder> _controlBuilders = new List<IControlBuilder>();

    internal FormControlGroupBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
    {
    }

    internal FormControlGroupBuilder(string name) : base(name)
    {
    }

    public FormControlGroupBuilder<T> CreateUiSection(string wrapperElement)
    {
        _wrapperElement = wrapperElement;
        return this;
    }

    public InferredFormControlBuilder FormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new InferredFormControlBuilder(propertyInfo));
    }

    public InputFormControlBuilder InputFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new InputFormControlBuilder(propertyInfo));
    }

    public EmailFormControlBuilder EmailFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new EmailFormControlBuilder(propertyInfo));
    }

    public PasswordFormControlBuilder PasswordFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new PasswordFormControlBuilder(propertyInfo));
    }

    public NumberFormControlBuilder NumberFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new NumberFormControlBuilder(propertyInfo));
    }

    public CheckboxFormControlBuilder CheckboxFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new CheckboxFormControlBuilder(propertyInfo));
    }

    public TextareaFormControlBuilder TextareaFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new TextareaFormControlBuilder(propertyInfo));
    }

    public RichTextInputFormControlBuilder RichTextInputFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new RichTextInputFormControlBuilder(propertyInfo));
    }

    public SelectFormControlBuilder SelectFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new SelectFormControlBuilder(propertyInfo));
    }

    public MultiSelectFormControlBuilder MultiSelectFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new MultiSelectFormControlBuilder(propertyInfo));
    }

    public RadioGroupFormControlBuilder RadioGroupFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new RadioGroupFormControlBuilder(propertyInfo));
    }
        
    public MultiCheckboxFormControlBuilder MultiCheckboxFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new MultiCheckboxFormControlBuilder(propertyInfo));
    }

    public AutocompleteFormControlBuilder AutocompleteFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new AutocompleteFormControlBuilder(propertyInfo));
    }

    public DatepickerFormControlBuilder DatepickerFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new DatepickerFormControlBuilder(propertyInfo));
    }

    public DateTimePickerFormControlBuilder DateTimePickerFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new DateTimePickerFormControlBuilder(propertyInfo));
    }

    public CustomFormControlBuilder CustomFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new CustomFormControlBuilder(propertyInfo));
    }

    public CustomFormControlBuilder CustomFormControl(string propertyName)
    {
        return GetOrCreateBuilder(propertyName, propName => new CustomFormControlBuilder(propName));
    }

    public FormControlGroupBuilder<T> FormControlGroup(string name)
    {
        return GetOrCreateBuilder(name, propertyName => new FormControlGroupBuilder<T>(propertyName));
    }

    public ButtonFormControlBuilder ButtonFormControl(string name)
    {
        return GetOrCreateBuilder(name, propertyName => new ButtonFormControlBuilder(propertyName));
    }

    public ArrayFormControlBuilder<TProperty> ArrayFormControl<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> propertyExpression)
    {
        return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new ArrayFormControlBuilder<TProperty>(propertyInfo));
    }

    public IList<FormControl> BuildFormControls(ComponentInfo componentInfo)
    {
        return _controlBuilders.Select(_ => _.Build(componentInfo)).Where(_ => !_.Ignore).ToList();
    }
        
    public override FormControl Build(ComponentInfo componentInfo)
    {
        var formControlGroup = new FormControlGroup {
            WrapperElement = _wrapperElement,
            FormControls = BuildFormControls(componentInfo)
        };

        return Build(componentInfo, formControlGroup);
    }

    public IControlBuilder? GetControlBuilder(PropertyInfo propertyInfo)
    {
        return _controlBuilders.FirstOrDefault(builder => builder.Has(propertyInfo));
    }

    public bool HasControlBuilder(PropertyInfo propertyInfo)
    {
        return Has(propertyInfo);
    }

    public override bool Has(PropertyInfo propertyInfo)
    {
        return PropertyInfo != null && PropertyInfo == propertyInfo || _controlBuilders.Any(builder => builder.Has(propertyInfo));
    }

    private TBuilder GetOrCreateBuilder<TBuilder>(PropertyInfo propertyInfo, Func<PropertyInfo, TBuilder> creator) where TBuilder : IControlBuilder
    {
        Check.NotNull(propertyInfo, nameof(propertyInfo));

        var builder = _controlBuilders.FirstOrDefault(builder => builder.Has(propertyInfo));
        if (builder != null)
        {
            return (TBuilder)builder;
        }

        builder = creator(propertyInfo);
        _controlBuilders.Add(builder);

        return (TBuilder)builder;
    }

    private TBuilder GetOrCreateBuilder<TBuilder>(string propertyName, Func<string, TBuilder> creator) where TBuilder : IControlBuilder
    {
        Check.NotNull(propertyName, nameof(propertyName));

        var builder = _controlBuilders.FirstOrDefault(builder => builder.Has(propertyName));
        if (builder != null)
        {
            return (TBuilder)builder;
        }

        builder = creator(propertyName);
        _controlBuilders.Add(builder);

        return (TBuilder)builder;
    }
}
