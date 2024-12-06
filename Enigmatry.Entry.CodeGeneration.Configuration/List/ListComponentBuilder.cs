using System.Linq.Expressions;
using Enigmatry.Entry.CodeGeneration.Configuration.Builder;
using Enigmatry.Entry.CodeGeneration.Configuration.List.Model;
using JetBrains.Annotations;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List;

/// <summary>
/// The ListComponentBuilder class provides a fluent API for configuring a table component
/// so that configuration can be used to generate UI client feature component e.g. Angular module.
/// </summary>
/// <remarks>
/// <para>
/// It provides methods to configure various aspects of the table component, such as component name,
/// feature name, columns, pagination, and row selection.
/// </para>
/// <example>
/// An example of using the ListComponentBuilder to configure a table component:
/// <code>
/// builder.Component()
///     .HasName("UserList")
///     .BelongsToFeature("Users");
///
/// builder.Column(x => x.Username)
///     .WithHeaderName("Email address");
///
/// builder.Row().Selection(RowSelectionType.Single);
///
/// builder.Pagination().ShowFirstLastPageButtons(false);
/// </code>
/// </example>
/// </remarks>
[UsedImplicitly]
public class ListComponentBuilder<T> : BaseComponentBuilder<ListComponentModel>
{
    private readonly IList<ColumnDefinitionBuilder> _columns = new List<ColumnDefinitionBuilder>();
    private readonly PaginationInfoBuilder _paginationInfoBuilder = new();
    private readonly RowInfoBuilder _rowInfoBuilder = new();

    public ListComponentBuilder() : base(typeof(T))
    {
    }

    /// <summary>
    /// Configure a table column based on a property of the generic type T.
    /// </summary>
    /// <param name="propertyExpression">A lambda expression to specify the property.</param>
    /// <returns>An instance of <see cref="ColumnDefinitionBuilder"/> to further configure the column.</returns>
    public ColumnDefinitionBuilder Column<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    {
        Check.NotNull(propertyExpression, nameof(propertyExpression));

        var propertyInfo = propertyExpression.GetPropertyInfo();
        var columnDefinitionBuilder = _columns.FirstOrDefault(builder => builder.HasProperty(propertyInfo));
        return GetOrAddBuilder(columnDefinitionBuilder, () => new ColumnDefinitionBuilder(propertyInfo));
    }

    /// <summary>
    /// Configure a table column based on a property name string.
    /// </summary>
    /// <param name="propertyName">The name of the property as a string.</param>
    /// <returns>An instance of <see cref="ColumnDefinitionBuilder"/> to further configure the column.</returns>
    public ColumnDefinitionBuilder Column(string propertyName)
    {
        Check.NotEmpty(propertyName, nameof(propertyName));

        var columnDefinitionBuilder = _columns.FirstOrDefault(builder => builder.HasProperty(propertyName));
        return GetOrAddBuilder(columnDefinitionBuilder, () => new ColumnDefinitionBuilder(propertyName));
    }

    /// <summary>
    /// Configure pagination options for the table component.
    /// </summary>
    /// <returns>An instance of <see cref="PaginationInfoBuilder"/> to further configure the pagination options.</returns>
    public PaginationInfoBuilder Pagination() { return _paginationInfoBuilder; }

    /// <summary>
    /// Configure row-level options, such as selection type and context menu.
    /// </summary>
    /// <returns>An instance of <see cref="RowInfoBuilder"/> to further configure the row options.</returns>
    public RowInfoBuilder Row() { return _rowInfoBuilder; }

    public override ListComponentModel Build()
    {
        var componentInfo = _componentInfoBuilder.Build();
        var columns = BuildColumnDefinitions(componentInfo);

        return new ListComponentModel(componentInfo, columns)
        {
            Row = _rowInfoBuilder.Build(componentInfo),
            Pagination = _paginationInfoBuilder.Build()
        };
    }

    private IList<ColumnDefinition> BuildColumnDefinitions(ComponentInfo componentInfo)
    {
        var columns = _columns.Select(_ => _.Build(componentInfo)).ToList();

        if (componentInfo.IncludeUnconfiguredProperties)
        {
            var unconfiguredColumns = BuildUnconfiguredColumnDefinitions(componentInfo);
            columns.AddRange(unconfiguredColumns);
        }

        if (componentInfo.OrderByType == OrderByType.Model)
        {
            var properties = _modelType.GetProperties().Select(propertyInfo => propertyInfo.Name.ToUpper()).ToList();
            columns = columns.OrderBy(columnDefinition => properties.IndexOf(columnDefinition.Property.ToUpper())).ToList();
        }
        return columns;
    }

    private ColumnDefinitionBuilder GetOrAddBuilder(ColumnDefinitionBuilder? builder, Func<ColumnDefinitionBuilder> creator)
    {
        if (builder != null) return builder;

        var columnDefinitionBuilder = creator();
        _columns.Add(columnDefinitionBuilder);

        return columnDefinitionBuilder;
    }

    private IEnumerable<ColumnDefinition> BuildUnconfiguredColumnDefinitions(ComponentInfo componentInfo)
    {
        return _modelType.GetProperties()
            .Where(propertyInfo => _columns.FirstOrDefault(builder => builder.HasProperty(propertyInfo)) == null)
            .Select(propertyInfo => GetOrAddBuilder(null, () => new ColumnDefinitionBuilder(propertyInfo)).Build(componentInfo))
            .ToList();
    }
}
