using System;
using System.Collections.Generic;
using System.Reflection;
using Enigmatry.Entry.CodeGeneration.Configuration.Builder;
using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Humanizer;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

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
    /// Set column header
    /// </summary>
    /// <param name="headerName">header name</param>
    /// <returns></returns>
    public ColumnDefinitionBuilder WithHeaderName(string headerName)
    {
        _headerName = headerName;
        return this;
    }

    /// <summary>
    /// Set header name translationId (i18n)
    /// </summary>
    /// <param name="translationId">translationId</param>
    /// <returns></returns>
    public ColumnDefinitionBuilder WithTranslationId(string translationId)
    {
        _translationId = translationId;
        return this;
    }

    /// <summary>
    /// Set if column is sortable
    /// </summary>
    /// <param name="isSortable">isSortable</param>
    /// <returns></returns>
    public ColumnDefinitionBuilder IsSortable(bool isSortable)
    {
        _isSortable = isSortable;
        return this;
    }

    /// <summary>
    /// Set id of the sort header. Defaults to the column's property name when not specified
    /// </summary>
    /// <param name="sortId">sortId</param>
    /// <returns></returns>
    public ColumnDefinitionBuilder WithSortId(string sortId)
    {
        _sortId = sortId;
        return this;
    }

    /// <summary>
    /// Set if column is visible
    /// </summary>
    /// <param name="isVisible">isVisible</param>
    /// <returns></returns>
    public ColumnDefinitionBuilder IsVisible(bool isVisible)
    {
        _isVisible = isVisible;
        return this;
    }

    /// <summary>
    /// Set column type formatter
    /// </summary>
    /// <param name="formatter">formatter</param>
    /// <returns></returns>
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
    /// Set component name used for cell rendering
    /// </summary>
    /// <param name="componentName">componentName</param>
    /// <returns></returns>
    public ColumnDefinitionBuilder WithCustomComponent(string componentName)
    {
        _customCellComponent = componentName;
        return this;
    }

    /// <summary>
    /// Set column css class name
    /// </summary>
    /// <param name="cssClassName">cssClassName</param>
    /// <returns></returns>
    public ColumnDefinitionBuilder WithCustomCssClass(string cssClassName)
    {
        _customCellCssClass = cssClassName;
        return this;
    }

    /// <summary>
    /// Set custom properties that can be used to pass data to custom cell components
    /// </summary>
    /// <param name="dictionary">custom properties dictionary</param>
    /// <returns></returns>
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