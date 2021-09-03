using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Enigmatry.BuildingBlocks.Validation;
using Enigmatry.BuildingBlocks.Validation.ValidationRules;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
using JetBrains.Annotations;

namespace Enigmatry.CodeGeneration.Configuration.Form
{
    [UsedImplicitly]
    public class FormComponentBuilder<T> : BaseComponentBuilder<FormComponentModel>
    {
        private readonly IList<FormControlBuilder> _formControls;
        private IList<IValidationRule> _validationRules;

        public FormComponentBuilder() : base(typeof(T))
        {
            _formControls = _modelType.GetProperties()
                .Select(propertyInfo => new FormControlBuilder(propertyInfo))
                .ToList();
            _validationRules = new List<IValidationRule>();

            _componentInfoBuilder.Routing().WithIdRoute();
        }

        public FormControlBuilder FormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));
            return FormControlBuilder(propertyExpression);
        }

        public void WithValidationConfiguration(IHasValidationRules validationConfirguration)
        {
            _validationRules = validationConfirguration.ValidationRules.ToList();
        }

        public override FormComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();
            var formControls = _formControls.Select(_ => _.Build(componentInfo));

            return new FormComponentModel(componentInfo, formControls, _validationRules);
        }

        private FormControlBuilder FormControlBuilder<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyInfo = propertyExpression.GetPropertyInfo();
            var formControlBuilder = _formControls.FirstOrDefault(builder => builder.PropertyInfo == propertyInfo);
            return GetOrAddBuilder(formControlBuilder, () => new FormControlBuilder(propertyInfo));
        }

        private FormControlBuilder GetOrAddBuilder(FormControlBuilder? builder, Func<FormControlBuilder> creator)
        {
            if (builder != null) return builder;

            var formControlBuilder = creator();
            _formControls.Add(formControlBuilder);

            return formControlBuilder;
        }
    }
}
