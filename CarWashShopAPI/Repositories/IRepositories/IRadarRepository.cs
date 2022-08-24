namespace CarWashShopAPI.Repositories.IRepositories
{
    public interface IRadarRepository
    {
        public void DoBookingWork(object? state);
        public void DoShopWork(object? state);
        public void DoOwnerWork(object? state);

    }
}
