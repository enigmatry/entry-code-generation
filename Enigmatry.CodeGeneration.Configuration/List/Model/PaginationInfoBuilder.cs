using System.Collections.Generic;

namespace Enigmatry.CodeGeneration.Configuration.List.Model
{
    public class PaginationInfoBuilder : IBuilder<PaginationInfo>
    {
        private bool _showPaginator = true;
        private bool _showFirstLastPageButtons = true;
        private int _pageSize = 20;
        private IEnumerable<int> _pageSizeOptions = new[] {20, 50, 100};
        private bool _showPageSize = true;

        public PaginationInfoBuilder ShowPaginator(bool value)
        {
            _showPaginator = value;
            return this;
        }

        public PaginationInfoBuilder ShowFirstLastPageButtons(bool value)
        {
            _showFirstLastPageButtons = value;
            return this;
        }

        public PaginationInfoBuilder PageSize(int value)
        {
            _pageSize = value;
            return this;
        }

        public PaginationInfoBuilder ShowPageSize(bool value)
        {
            _showPageSize = value;
            return this;
        }

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
}
