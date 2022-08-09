using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ServiceCreation, Service>();
            CreateMap<CarWashShopCreation, CarWashShop>()
                .ForMember(x => x.Owners, opt => opt.MapFrom(CarWashCreationOwners2CarWashOwners))
                .ForMember(x => x.Services, opt => opt.MapFrom(CarWashCreationService2CarWashService))
                .ForMember(x => x.CarWashShopsServices, opt => opt.Ignore());

            //CreateMap<Franchise, FranchiseView>()
            //    .ForMember(x => x.CarWashes, opt => opt.MapFrom(CarWash2CarWashView));

        }

        private List<CarWashShopsOwners> CarWashCreationOwners2CarWashOwners(CarWashShopCreation creation, CarWashShop entity)
        {
            var result = new List<CarWashShopsOwners>();

            creation.CarWashShopsOwners.ForEach(x => result.Add(new CarWashShopsOwners() { OwnerId = x.ToUpper() }));
            return result;
        }

        private List<Service> CarWashCreationService2CarWashService(CarWashShopCreation creation, CarWashShop entity)
        {
            var result = new List<Service>();
            creation.Services.ForEach(x => result.Add(new Service() { Name = x.Name, Description = x.Description, Price = x.Price}));
            return result;
        }
        //private List<FranchiseCarWashView> CarWash2CarWashView(Franchise from, FranchiseView to)
        //{
        //    var result = new List<FranchiseCarWashView>();
        //    foreach (CarWash carWash in from.CarWashes)
        //    {
        //        result.Add(new FranchiseCarWashView()
        //        {
        //            Id = carWash.Id,
        //            Name = carWash.Name,
        //            AmountOfWashingBoxes = carWash.AmountOfWashingBoxes,
        //            FranchiseName = from.Name,
        //            CarWashOwners = carWash.CarWashOwners
        //        });
        //    }
        //    return result;
        //}
    }
}
