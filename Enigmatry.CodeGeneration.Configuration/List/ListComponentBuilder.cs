using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.List.Model;

namespace Enigmatry.CodeGeneration.Configuration.List
{
    public class ListComponentBuilder<T> : BaseComponentBuilder<ListComponentModel>
    {
        private readonly IList<ColumnDefinitionBuilder> _columns;

        public ListComponentBuilder() : base(typeof(T))
        {
            _routingInfoBuilder.WithEmptyRoute();
            _columns = _modelType.GetProperties().Select(propertyInfo => new ColumnDefinitionBuilder(propertyInfo)).ToList();
        }

        public ColumnDefinitionBuilder Column<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));
            return _columns.First(builder => builder.Has(propertyExpression));
        }

        public ColumnDefinitionBuilder Column(string propertyName)
        {
            Check.NotEmpty(propertyName, nameof(propertyName));

            var columnDefinitionBuilder = _columns.FirstOrDefault(builder => builder.Has(propertyName));
            if (columnDefinitionBuilder != null) return columnDefinitionBuilder;

            columnDefinitionBuilder = new ColumnDefinitionBuilder(propertyName);
            _columns.Add(columnDefinitionBuilder);

            return columnDefinitionBuilder;
        }

        public override ListComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();
            var routingInfo = _routingInfoBuilder.Build();
            var apiClientInfo = _apiClientInfoBuilder.Build();

            var columns = _columns.Select(_ => _.Build()).ToList();

            return new ListComponentModel(componentInfo, routingInfo, apiClientInfo, columns);
        }
    }
}
