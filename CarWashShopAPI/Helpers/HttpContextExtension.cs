using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Helpers
{
    public static class HttpContextExtension
    {
        public async static Task InsertPagination<T>(this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double count = await queryable.CountAsync();
            double totalAmountOfPages = Math.Ceiling(count / recordsPerPage);
            httpContext.Response.Headers.Add("NumberOfPages", totalAmountOfPages.ToString());
            httpContext.Response.Headers.Add("TotalAmountOfItems", count.ToString());
        }
    }
}
