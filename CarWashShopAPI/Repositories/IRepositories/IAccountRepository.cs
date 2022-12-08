using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.UserDTOs;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        public Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO);
        public Task<UserToken> BuildToken(UserLogin userInfo);
        public Task DeleteUserAssets(CustomUser user);
        public Task<IQueryable<CustomUser>> GetUsers(UserFilter filter);
        public Task<CustomUser> GetUserByEmail(string email); 
        public Task RemoveUser(CustomUser user);
        public Task Commit();
    }
}
