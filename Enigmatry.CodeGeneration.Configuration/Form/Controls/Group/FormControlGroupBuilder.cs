using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
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

        public CheckboxFormControlBuilder CheckboxFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new CheckboxFormControlBuilder(propertyInfo));
        }

        public TextareaFormControlBuilder TextareaFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new TextareaFormControlBuilder(propertyInfo));
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

        public AutocompleteFormControlBuilder AutocompleteFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new AutocompleteFormControlBuilder(propertyInfo));
        }

        public DatepickerFormControlBuilder DatepickerFormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return GetOrCreateBuilder(propertyExpression.GetPropertyInfo(), propertyInfo => new DatepickerFormControlBuilder(propertyInfo));
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

        public IList<FormControl> BuildFormControls(ComponentInfo componentInfo)
        {
            return _controlBuilders.Select(_ => _.Build(componentInfo)).Where(_ => !_.Ignore).ToList();
        }
        
        public override FormControl Build(ComponentInfo componentInfo)
        {
            var formControlGroup = new FormControlGroup { WrapperElement = _wrapperElement, FormControls = BuildFormControls(componentInfo) };

            return Build(componentInfo, formControlGroup);
        }

        public IControlBuilder? GetControlBuilder(PropertyInfo propertyInfo)
        {
            return _controlBuilders.FirstOrDefault(builder => builder.Has(propertyInfo));
        }

        public bool HasControlBuilder(PropertyInfo propertyInfo)
        {
            return _controlBuilders.Any(builder => builder.Has(propertyInfo));
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
}
