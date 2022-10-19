using CRUDNewsApi.Entities;

namespace CRUDNewsApi.Helpers.Pagination
{
    public class UserPaginationParams : PaginationParameters
    {
        public ERoles? Roles { get; set; }
        public EStatus? Status { get; set; }
    }
}
