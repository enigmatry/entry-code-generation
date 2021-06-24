using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class PaginationInfo
    {
        public bool ShowPaginator { get; set; } = true;
        public bool ShowFirstLastPageButtons { get; set; } = true;
        public IEnumerable<int> PageSizeOptions { get; set; } = new[] {10, 50, 100};
        public bool ShowPageSize { get; set; } = true;
    }
}
