using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.List.Model;
using JetBrains.Annotations;

namespace Enigmatry.CodeGeneration.Configuration.List
{
    [UsedImplicitly]
    public class ListComponentBuilder<T> : BaseComponentBuilder<ListComponentModel>
    {
        private readonly IList<ColumnDefinitionBuilder> _columns;
        private readonly PaginationInfoBuilder _paginationInfoBuilder = new PaginationInfoBuilder();
        private readonly RowInfoBuilder _rowInfoBuilder = new RowInfoBuilder();

        public ListComponentBuilder() : base(typeof(T))
        {
            _columns = _modelType.GetProperties().Select(propertyInfo => new ColumnDefinitionBuilder(propertyInfo)).ToList();
            _componentInfoBuilder.Routing().WithEmptyRoute();
        }

        public ColumnDefinitionBuilder Column<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));
            return _columns.First(builder => builder.HasProperty(propertyExpression));
        }

        public ColumnDefinitionBuilder Column(string propertyName)
        {
            Check.NotEmpty(propertyName, nameof(propertyName));

            var columnDefinitionBuilder = _columns.FirstOrDefault(builder => builder.HasProperty(propertyName));
            if (columnDefinitionBuilder != null) return columnDefinitionBuilder;

            columnDefinitionBuilder = new ColumnDefinitionBuilder(propertyName);
            _columns.Add(columnDefinitionBuilder);

            return columnDefinitionBuilder;
        }

        public PaginationInfoBuilder Pagination() { return _paginationInfoBuilder; }

        public RowInfoBuilder Row() { return _rowInfoBuilder; }

        public override ListComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();

            var columns = _columns.Select(_ => _.Build()).ToList();

            return new ListComponentModel(componentInfo, columns) {Row = _rowInfoBuilder.Build(), Pagination = _paginationInfoBuilder.Build()};
        }
    }
}
