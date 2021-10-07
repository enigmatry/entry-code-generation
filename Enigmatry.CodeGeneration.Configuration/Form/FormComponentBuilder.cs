using Enigmatry.BuildingBlocks.Validation;
using Enigmatry.BuildingBlocks.Validation.ValidationRules;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Enigmatry.CodeGeneration.Configuration.Form
{
    [UsedImplicitly]
    public class FormComponentBuilder<T> : BaseComponentBuilder<FormComponentModel>
    {
        private readonly IList<IControlBuilder> _formControls = new List<IControlBuilder>();
        private IEnumerable<IFormlyValidationRule> _validationRules = new List<IFormlyValidationRule>();

        public FormComponentBuilder() : base(typeof(T))
        {
            _componentInfoBuilder.Routing().WithIdRoute();
        }

        public FormControlBuilder FormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));
            return FormControlBuilder(propertyExpression);
        }

        public FormControlGroupBuilder<T> FormControlGroup(string groupName)
        {
            Check.NotNull(groupName, nameof(groupName));

            var formControlBuilder = _formControls.FirstOrDefault(builder => builder.Has(groupName));
            return (FormControlGroupBuilder<T>)GetOrCreateBuilder(formControlBuilder, () => new FormControlGroupBuilder<T>(groupName));
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
            var buildFormControls = _formControls.Select(_ => _.Build(componentInfo)).ToList();

            if (buildFormControls.OfType<FormControlGroup>().Any())
            {
                // NOTE:
                // support advanced layouts with form control groups
                // use only controls that are added during configuration!?
                return buildFormControls;
            }

            // NOTE:
            // by default controls are created for all properties in the model
            return GetDefaultControlBuildersForAllProperties()
                .Select(defaultPropertyBuilder => _formControls.FirstOrDefault(builder => builder.Has(defaultPropertyBuilder.PropertyInfo!)) ?? defaultPropertyBuilder)
                .Select(controlBuilder => controlBuilder.Build(componentInfo))
                .ToList();
        }

        private FormControlBuilder FormControlBuilder<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyInfo = propertyExpression.GetPropertyInfo();
            var formControlBuilder = _formControls.FirstOrDefault(builder => builder.Has(propertyInfo));
            return (FormControlBuilder)GetOrCreateBuilder(formControlBuilder, () => new FormControlBuilder(propertyInfo));
        }

        private IControlBuilder GetOrCreateBuilder(IControlBuilder? builder, Func<IControlBuilder> creator)
        {
            if (builder != null) return builder;

            var formControlBuilder = creator();
            _formControls.Add(formControlBuilder);

            return formControlBuilder;
        }

        private IEnumerable<IControlBuilder> GetDefaultControlBuildersForAllProperties()
        {
            return _modelType.GetProperties()
                .Select(propertyInfo => (IControlBuilder)new FormControlBuilder(propertyInfo))
                .ToList();
        }

    }
}
