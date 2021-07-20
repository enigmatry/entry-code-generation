using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
using JetBrains.Annotations;

namespace Enigmatry.CodeGeneration.Configuration.Form
{
    [UsedImplicitly]
    public class FormComponentBuilder<T> : BaseComponentBuilder<FormComponentModel>
    {
        private readonly IList<FormControlBuilder> _formControls;

        public FormComponentBuilder() : base(typeof(T))
        {
            _formControls = _modelType.GetProperties()
                .Select(propertyInfo => new FormControlBuilder(propertyInfo))
                .ToList();

            _componentInfoBuilder.Routing().WithIdRoute();
        }

        public FormControlBuilder FormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));
            return FormControlBuilder(propertyExpression);
        }

        public override FormComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();
            var formControls = _formControls.Select(_ => _.Build());

            return new FormComponentModel(componentInfo, formControls);
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
