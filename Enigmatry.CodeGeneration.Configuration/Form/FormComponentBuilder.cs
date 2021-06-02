using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Form.Model;

namespace Enigmatry.CodeGeneration.Configuration.Form
{
    public class FormComponentBuilder<T> : BaseComponentBuilder<FormComponentModel>
    {
        private readonly IList<FormControlBuilder> _formControls;
        private string _createOrUpdateCommandTypeName = String.Empty;

        public FormComponentBuilder() : base(typeof(T))
        {
            _formControls = _modelType
                .GetProperties()
                .Select(propertyInfo => new FormControlBuilder(propertyInfo))
                .ToList();

            _componentInfoBuilder.Routing().WithIdRoute();
        }

        public FormControlBuilder FormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));

            var propertyInfo = propertyExpression.GetPropertyInfo();
            return _formControls.First(builder => builder.PropertyInfo == propertyInfo);
        }

        public override FormComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();
            var formControls = _formControls.Select(_ => _.Build());

            return new FormComponentModel(componentInfo, formControls, _createOrUpdateCommandTypeName);
        }

        public FormComponentBuilder<T> HasCreateOrUpdateCommandOfType<TCreateOrUpdateCommand>()
        {
            _createOrUpdateCommandTypeName = typeof(TCreateOrUpdateCommand).GetDeclaringName();
            return this;
        }

        public FormComponentBuilder<T> HasCreateOrUpdateCommandWithName(string createOrUpdateCommandTypeName)
        {
            _createOrUpdateCommandTypeName = createOrUpdateCommandTypeName;
            return this;
        }
    }
}
