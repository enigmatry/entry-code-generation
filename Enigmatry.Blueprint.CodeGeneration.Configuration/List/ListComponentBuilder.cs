using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Enigmatry.Blueprint.CodeGeneration.Configuration.Builder;
using Enigmatry.Blueprint.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.List
{
    public class ListComponentBuilder<T> : BaseComponentBuilder<ListComponentModel>
    {
        private readonly IList<ColumnPropertyBuilder> _columns;

        public ListComponentBuilder() : base(typeof(T))
        {
            _columns = _modelType
                .GetProperties().Select(propertyInfo => new ColumnPropertyBuilder(propertyInfo)).ToList();
        }

        public ColumnPropertyBuilder Column<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));

            var propertyInfo = propertyExpression.GetPropertyAccess();
            return _columns.First(builder => builder.PropertyInfo == propertyInfo);
        }

        public override ListComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();
            componentInfo.Routing = RoutingInfo.WithEmptyRoute();

            var columns = _columns.Select(_ => _.Build()).ToList();

            return new ListComponentModel(componentInfo, columns);
        }
    }
}
