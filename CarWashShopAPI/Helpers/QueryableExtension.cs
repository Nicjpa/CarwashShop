using CarWashShopAPI.DTO;

namespace CarWashShopAPI.Helpers
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pages)
        {
            return queryable
                .Skip((pages.Page - 1) * pages.RecordsPerPage)
                .Take(pages.RecordsPerPage);
        }
    }
}
