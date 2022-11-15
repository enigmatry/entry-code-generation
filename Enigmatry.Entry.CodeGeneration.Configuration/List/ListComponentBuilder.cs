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

            var propertyInfo = propertyExpression.GetPropertyInfo();
            var columnDefinitionBuilder = _columns.FirstOrDefault(builder => builder.HasProperty(propertyInfo));
            return GetOrAddBuilder(columnDefinitionBuilder, () => new ColumnDefinitionBuilder(propertyInfo));
        }

        public ColumnDefinitionBuilder Column(string propertyName)
        {
            Check.NotEmpty(propertyName, nameof(propertyName));

            var columnDefinitionBuilder = _columns.FirstOrDefault(builder => builder.HasProperty(propertyName));
            return GetOrAddBuilder(columnDefinitionBuilder, () => new ColumnDefinitionBuilder(propertyName));
        }

        public PaginationInfoBuilder Pagination() { return _paginationInfoBuilder; }

        public RowInfoBuilder Row() { return _rowInfoBuilder; }

        public override ListComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();
            var columns = _columns.Select(_ => _.Build(componentInfo)).ToList();

            return new ListComponentModel(componentInfo, columns) {Row = _rowInfoBuilder.Build(componentInfo), Pagination = _paginationInfoBuilder.Build()};
        }

        private ColumnDefinitionBuilder GetOrAddBuilder(ColumnDefinitionBuilder? builder, Func<ColumnDefinitionBuilder> creator)
        {
            if (builder != null) return builder;

            var columnDefinitionBuilder = creator();
            _columns.Add(columnDefinitionBuilder);

            return columnDefinitionBuilder;
        }
    }
}
