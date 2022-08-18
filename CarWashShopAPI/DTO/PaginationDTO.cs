namespace CarWashShopAPI.DTO
{
    public class PaginationDTO
    {
        
        private int _recordsPerPage = 10;
        private readonly int _maxRecordsPerPage = 20;

        public int Page { get; set; } = 1;
        public int RecordsPerPage
        {
            get { return _recordsPerPage; }
            set { _recordsPerPage = (value > _maxRecordsPerPage) ? _maxRecordsPerPage : value; }
        }
    }
}
