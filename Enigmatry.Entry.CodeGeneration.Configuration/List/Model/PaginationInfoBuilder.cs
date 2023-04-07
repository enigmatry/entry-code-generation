using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

/// <summary>
/// The PaginationInfoBuilder class provides a fluent API for configuring pagination settings for a table component.
/// </summary>
/// <remarks>
/// <para>
/// It offers methods to set various properties related to pagination, such as showing or hiding
/// the paginator, configuring page size options, and displaying controls for navigating to the first
/// and last pages.
/// </para>
/// <example>
/// An example of using the PaginationInfoBuilder to configure pagination for a table component:
/// <code>
/// builder.Pagination()
///     .ShowPaginator(true)
///     .ShowFirstLastPageButtons(false)
///     .PageSize(10)
///     .ShowPageSize(true)
///     .PageSizeOptions(new int[] { 10, 50, 100 });
/// </code>
/// </example>
/// </remarks>
public class PaginationInfoBuilder : IBuilder<PaginationInfo>
{
    private bool _showPaginator = true;
    private bool _showFirstLastPageButtons = true;
    private int _pageSize = 20;
    private IEnumerable<int> _pageSizeOptions = new[] {20, 50, 100};
    private bool _showPageSize = true;

    /// <summary>
    /// Show or hide the paginator.
    /// </summary>
    /// <param name="value">Set to true to show the paginator, false to hide it.</param>
    /// <returns>The <see cref="PaginationInfoBuilder"/> instance for method chaining.</returns>
    public PaginationInfoBuilder ShowPaginator(bool value)
    {
        _showPaginator = value;
        return this;
    }

    /// <summary>
    /// Show or hide buttons in the paginator to go to the first or last page.
    /// </summary>
    /// <param name="value">Set to true to show the buttons, false to hide them.</param>
    /// <returns>The <see cref="PaginationInfoBuilder"/> instance for method chaining.</returns>
    public PaginationInfoBuilder ShowFirstLastPageButtons(bool value)
    {
        _showFirstLastPageButtons = value;
        return this;
    }

    /// <summary>
    /// Set the maximum number of items displayed per page.
    /// </summary>
    /// <param name="value">The maximum number of items per page.</param>
    /// <returns>The <see cref="PaginationInfoBuilder"/> instance for method chaining.</returns>
    public PaginationInfoBuilder PageSize(int value)
    {
        _pageSize = value;
        return this;
    }

    /// <summary>
    /// Show or hide the page size select control in the pager.
    /// </summary>
    /// <param name="value">Set to true to show the page size control, false to hide it.</param>
    /// <returns>The <see cref="PaginationInfoBuilder"/> instance for method chaining.</returns>
    public PaginationInfoBuilder ShowPageSize(bool value)
    {
        _showPageSize = value;
        return this;
    }

    /// <summary>
    /// Set the available page size options.
    /// </summary>
    /// <param name="options">An IEnumerable of integers representing the available page size options.</param>
    /// <returns>The <see cref="PaginationInfoBuilder"/> instance for method chaining.</returns>
    public PaginationInfoBuilder PageSizeOptions(IEnumerable<int> options)
    {
        _pageSizeOptions = options;
        return this;
    }

    public PaginationInfo Build()
    {
        return new PaginationInfo
        {
            PageSize = _pageSize,
            ShowPageSize = _showPageSize,
            ShowFirstLastPageButtons = _showFirstLastPageButtons,
            PageSizeOptions = _pageSizeOptions,
            ShowPaginator = _showPaginator
        };
    }
}
