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
        private bool _showPaginator = true;
        private bool _showFirstLastPageButtons = false;
        private bool _hidePageSize = false;
        private IEnumerable<int> _pageSizeOptions = new[] { 10, 50, 100 };
        private bool _enableSingleSelection = false;
        private bool _enableMultiSelection = false;

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

        public ListComponentBuilder<T> ShowPaginator(bool show) { _showPaginator = show; return this; }
        public ListComponentBuilder<T> ShowFirstLastPageButtons(bool show) { _showFirstLastPageButtons = show; return this; }
        public ListComponentBuilder<T> PageSizeOptions(IEnumerable<int> pageSizeOptions) { _pageSizeOptions = pageSizeOptions; return this; }
        public ListComponentBuilder<T> HidePageSize(bool hidePageSize) { _hidePageSize = hidePageSize; return this; }
        public ListComponentBuilder<T> EnableSingleSelection(bool enable) { _enableSingleSelection = enable; return this; }
        public ListComponentBuilder<T> EnableMultiSelection(bool enable) { _enableMultiSelection = enable; return this; }


        public override ListComponentModel Build()
        {
            var componentInfo = _componentInfoBuilder.Build();

            var columns = _columns.Select(_ => _.Build()).ToList();

            return new ListComponentModel(componentInfo, columns)
            {
                ShowPaginator = _showPaginator,
                ShowFirstLastPageButtons = _showFirstLastPageButtons,
                PageSizeOptions = _pageSizeOptions,
                HidePageSize = _hidePageSize,
                EnableSingleSelection = _enableSingleSelection,
                EnableMultiSelection = _enableMultiSelection
            };
        }
    }
}
