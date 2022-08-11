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
            // CAR WASH SHOP MAPPERS
            CreateMap<ServiceCreationView, Service>();

            CreateMap<CarWashShopCreation, Service>();

            CreateMap<CarWashShop, CarWashShopView>()
                .ForMember(x => x.Services, opt => opt.Ignore());

            CreateMap<CarWashShopsOwners, CarWashShopView>()
                .ForMember(x => x.Services, opt => opt.MapFrom(CarWashShopsOwners2CarWashShopView));

            CreateMap<CarWashShopCreation, CarWashShop>()
                .ForMember(x => x.Owners, opt => opt.MapFrom(CarWashCreationOwners2CarWashOwners))
                .ForMember(x => x.CarWashShopsServices, opt => opt.Ignore());

            CreateMap<CarWashShopUpdate, CarWashShop>()
                .ReverseMap();

            CreateMap<CarWashShop, OwnersViewPerShop>()
                .ForMember(x => x.Owners, opt => opt.MapFrom(CarWashShopOwners2OwnerView));
        }

        private List<CarWashShopsOwners> CarWashCreationOwners2CarWashOwners(CarWashShopCreation creation, CarWashShop entity)
        {
            var result = new List<CarWashShopsOwners>();
            creation.CarWashShopsOwners.ForEach(x => result.Add(new CarWashShopsOwners() { OwnerId = x.ToUpper() }));
            return result;
        }

        private List<ServiceCreationView> CarWashShopsOwners2CarWashShopView(CarWashShopsOwners entity, CarWashShopView view)
        {
            var result = new List<ServiceCreationView>();
            var service = new List<Service>();
            entity.CarWashShop.CarWashShopsServices.ForEach(x => service.Add(x.Service));
            service.ForEach(x => result.Add(new ServiceCreationView { Name = x.Name, Description = x.Description, Price = x.Price }));
            return result;
        }

        private List<string> CarWashShopOwners2OwnerView(CarWashShop entity, OwnersViewPerShop view)
        {
            var result = new List<string>();
            entity.Owners.ForEach(x => result.Add(x.OwnerId));
            return result;
        }
        //private CarWashShopView CarWashShopsOwners2CarWashShopView(CarWashShopsOwners entity, CarWashShopView view)
        //{

        //    var service = new List<Service>();
        //    var serviceView = new List<ServiceCreationView>();

        //    entity.CarWashShop.CarWashShopsServices.ForEach(x => service.Add(x.Service));
        //    service.ForEach(x => serviceView.Add(new ServiceCreationView { Name = x.Name, Description = x.Description, Price = x.Price }));

        //    var result = new CarWashShopView()
        //    {
        //        Id = entity.CarWashShop.Id,
        //        Name = entity.CarWashShop.Name,
        //        AdvertisingDescription = entity.CarWashShop.AdvertisingDescription,
        //        OpeningTime = entity.CarWashShop.OpeningTime,
        //        ClosingTime = entity.CarWashShop.ClosingTime,
        //        Services = serviceView
        //    };
        //    return result;
        //}

    }
}
