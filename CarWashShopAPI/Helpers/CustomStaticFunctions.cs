using CarWashShopAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Helpers
{
    public static class CustomStaticFunctions
    {
        public async static Task<List<T>> GenericPagination<T>(this HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO)
        {
            await httpContext.InsertPagination(genericList, recPerPage);
            return await genericList.Paginate(paginationDTO).ToListAsync();
        }
    }
}
