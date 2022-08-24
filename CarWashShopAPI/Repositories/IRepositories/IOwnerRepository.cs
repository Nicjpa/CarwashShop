using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Repositories.IRepositories
{
    public interface IOwnerRepository
    {
        public Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO);
        public Task<List<ShopRemovalRequest>> GetShopRequestsToCancel(int id, string userName);
        public Task<List<ShopRemovalRequest>> GetShopRequestsToApprove(int id, string userName);
        public Task<DisbandRequest> GetDisbandRequestToApprove(int id, string userName);
        public Task<Booking> GetBookingByID(BookingStatusSelection status, string userName);
        public Task<CarWashShop> GetShopToDisbandOwner(int id, string userName);
        public Task<DisbandRequest> CreateDisbandRequest(DisbandRequestCreation statement, CarWashShop shop, string userName);
        public Task<List<CarWashShopsOwners>> AssignNewOwnersToTheShop(CarWashShop carWashShop, List<string> approvedOwnersIDs, List<string> CurrentOwnerUserIds);
        public Task<CarWashShop> GetCarWashShopToAssignOwners(int id);
        public Task<List<string>> GetApprovedOwnerIDs(CarWashShopOwnerAdd listOfNewOwners, List<string> currentOwnerList);
        public Task<List<IncomeEntity>> GetIncome(IncomeFilter filter, string userName);
        public Task<List<IncomeViewDays>> IncomeEntityMap2IncomeViewDays(List<IncomeEntity> incomeEntities, IncomeFilter filter);
        public Task<List<IncomeViewOther>> IncomeEntityMap2IncomeViewOther(List<IncomeEntity> incomeEntities, IncomeFilter filter);
        public Task<IQueryable<CarWashShop>> GetShopsForRevenue(string userName, RevenueFilters revenueFilters);
        public Task<List<RevenueReportView>> CalculateRevenue(IQueryable<CarWashShop> carWashShops);
        public Task<IQueryable<ShopRemovalRequest>> GetShopRemovalRequests(string userName, OwnerRequestsFilters filters);
        public Task<IQueryable<DisbandRequest>> GetDisbandRequests(string userName, OwnerRequestsFilters filters);
        public Task<IQueryable<Booking>> GetBookings(string userName, BookingFilters filters);
        public Task<IQueryable<CarWashShop>> GetOwners(string userName, ListOfOwnersPerShopFilters filters);
        
    }
}
