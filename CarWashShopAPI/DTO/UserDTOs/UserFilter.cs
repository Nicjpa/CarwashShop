using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.DTO.UserDTOs
{
    public class UserFilter : UserView
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 50;
        public PaginationDTO Pagination { get { return new PaginationDTO() { Page = Page, RecordsPerPage = RecordsPerPage }; } }
    }
}
