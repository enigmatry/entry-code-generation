using System;
using System.Collections.Generic;
using System.Reflection;
using Enigmatry.Entry.CodeGeneration.Configuration.Builder;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

/// <summary>
/// The ColumnDefinitionBuilder class is used for configuring individual columns in a table component.
/// It provides methods to set various properties of the columns, such as header name, visibility,
/// sorting, formatting, and more. The methods in this class are designed to be chained together,
/// allowing for a fluent and readable syntax when configuring table columns.
/// </summary>
/// <example>
/// An example of using the ColumnDefinitionBuilder to configure a column for a table component:
/// <code>
/// builder.Column(x => x.UserName)
/// .WithHeaderName("E-mail")
/// .IsVisible(true)
/// .IsSortable(true)
/// .WithFormat(new DatePropertyFormatter().WithFormat("yyyy-MM-dd"));
/// </code>
/// </example>
public class ColumnDefinitionBuilder
{
    private readonly PropertyAccessor? _propertyAccessor;
    private readonly string _propertyName;
    private string _headerName;
    private string? _translationId;
    private bool _isVisible;
    private bool _isSortable;
    private IPropertyFormatter _formatter;
    private string? _customCellComponent;
    private string? _customCellCssClass;
    private IDictionary<string, string> _customProperties = new Dictionary<string, string>();
    private string? _sortId;

    public ColumnDefinitionBuilder(PropertyInfo propertyInfo)
    {
        _propertyAccessor = new PropertyAccessor(propertyInfo);
        _propertyName = _propertyAccessor.Name.Camelize();
        _headerName = _propertyAccessor.DisplayName;
        _isVisible = !_propertyAccessor.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
        _isSortable = true;
        _formatter = _propertyAccessor.GetDefaultPropertyFormatter();
    }

    public ColumnDefinitionBuilder(string propertyName)
    {
        _propertyName = propertyName.Camelize();
        _headerName = propertyName.Humanize();
        _isVisible = true;
        _isSortable = true;
        _formatter = new NoFormattingPropertyFormatter();
    }

    public ColumnDefinition Build(ComponentInfo componentInfo)
    {
        var translationId = _translationId ?? $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}";

        return new ColumnDefinition
        {
            ComponentInfo = componentInfo,
            Property = _propertyName,
            HeaderName = _headerName,
            TranslationId = translationId,
            IsSortable = _isSortable,
            IsVisible = _isVisible,
            Formatter = _formatter,
            CustomCellComponent = _customCellComponent,
            CustomCellCssClass = _customCellCssClass,
            CustomProperties = _customProperties,
            SortId = _sortId
        };
    }

    /// <summary>
    /// Set the column header.
    /// </summary>
    /// <param name="headerName">The name of the header.</param>
    /// <returns>The ColumnDefinitionBuilder instance for method chaining.</returns>
    public ColumnDefinitionBuilder WithHeaderName(string headerName)
    {
        _headerName = headerName;
        return this;
    }

    /// <summary>
    /// Set the header name translation ID (i18n).
    /// </summary>
    /// <param name="translationId">The translation ID for the header name.</param>
    /// <returns>The ColumnDefinitionBuilder instance for method chaining.</returns>
    public ColumnDefinitionBuilder WithTranslationId(string translationId)
    {
        _translationId = translationId;
        return this;
    }

    /// <summary>
    /// Set whether the column is sortable.
    /// </summary>
    /// <param name="isSortable">Set to true to make the column sortable, false otherwise.</param>
    /// <returns>The ColumnDefinitionBuilder instance for method chaining.</returns>
    public ColumnDefinitionBuilder IsSortable(bool isSortable)
    {
        _isSortable = isSortable;
        return this;
    }

    /// <summary>
    /// Set the ID of the sort header. Defaults to the column's property name when not specified.
    /// </summary>
    /// <param name="sortId">The sort ID for the column.</param>
    /// <returns>The <see cref="ColumnDefinitionBuilder"/> instance for method chaining.</returns>
    public ColumnDefinitionBuilder WithSortId(string sortId)
    {
        _sortId = sortId;
        return this;
    }

    /// <summary>
    /// Set whether the column is visible. The default is true, unless the property name in lower case is equal to 'id'.
    /// </summary>
    /// <param name="isVisible">Set to true to make the column visible, false otherwise.</param>
    /// <returns>The <see cref="ColumnDefinitionBuilder"/> instance for method chaining.</returns>
    public ColumnDefinitionBuilder IsVisible(bool isVisible)
    {
        _isVisible = isVisible;
        return this;
    }

    /// <summary>
    /// Set the column type formatter. If not set will default to the formatter that supports the property's type.
    /// The formatter given should support the type of the property.
    /// </summary>
    /// <param name="formatter">The formatter to format the column's value.</param>
    /// <returns>The <see cref="ColumnDefinitionBuilder"/> instance for method chaining.</returns>
    public ColumnDefinitionBuilder WithFormat(IPropertyFormatter formatter)
    {
        if (_propertyAccessor != null)
        {
            formatter.ValidateInputType(_propertyAccessor.PropertyType);
        }
        _formatter = formatter;
        return this;
    }

    /// <summary>
    /// Set the component name used for cell rendering.
    /// </summary>
    /// <param name="componentName">The name of the custom component for cell rendering.</param>
    /// <returns>The <see cref="ColumnDefinitionBuilder"/> instance for method chaining.</returns>
    public ColumnDefinitionBuilder WithCustomComponent(string componentName)
    {
        _customCellComponent = componentName;
        return this;
    }

    /// <summary>
    /// Set the column CSS class name.
    /// </summary>
    /// <param name="cssClassName">The CSS class name for the column.</param>
    /// <returns>The <see cref="ColumnDefinitionBuilder"/> instance for method chaining.</returns>
    public ColumnDefinitionBuilder WithCustomCssClass(string cssClassName)
    {
        _customCellCssClass = cssClassName;
        return this;
    }

    /// <summary>
    /// Set custom properties that can be used to pass data to custom cell components.
    /// </summary>
    /// <param name="dictionary">A dictionary containing custom properties.</param>
    /// <returns>The <see cref="ColumnDefinitionBuilder"/> instance for method chaining.</returns>
    public ColumnDefinitionBuilder WithCustomProperties(IDictionary<string, string> dictionary)
    {
        _customProperties = new Dictionary<string, string>(dictionary);
        return this;
    }

    internal bool HasProperty(PropertyInfo propertyInfo)
    {
        return _propertyAccessor != null && _propertyAccessor.PropertyInfo == propertyInfo;
    }

    internal bool HasProperty(string propertyName)
    {
        return _propertyName == propertyName;
    }
}
