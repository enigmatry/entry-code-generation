namespace Enigmatry.Entry.CodeGeneration.Configuration.List.Model;

public class PaginationInfo
{
    public bool ShowPaginator { get; set; } = true;
    public bool ShowFirstLastPageButtons { get; set; } = true;
    public int PageSize { get; set; } = 20;
    public IEnumerable<int> PageSizeOptions { get; set; } = new[] { 20, 50, 100 };
    public bool ShowPageSize { get; set; } = true;
}
