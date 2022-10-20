namespace CRUDNewsApi.Helpers.Pagination
{
    public abstract class PaginationParameters
    {
        const int maxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        public string? Search { get; set; }
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > maxPageSize ? maxPageSize : value;
            }
        }
    }
}
