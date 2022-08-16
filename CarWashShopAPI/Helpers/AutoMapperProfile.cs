using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CAR WASH SHOP MAPPERS
            CreateMap<ServiceCreationAndUpdate, Service>()
                .ReverseMap();

            CreateMap<Service, ServiceView>();

            CreateMap<Service, ServiceViewWithShopAssigned>()
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShops.Select(x => x.CarWashShop.Name).FirstOrDefault()));

            CreateMap<CarWashShopCreation, Service>();

            CreateMap<CarWashShop, CarWashShopView>()
                .ForMember(x => x.Services, opt => opt.MapFrom(CarWashShopsOwners2CarWashShopView));

            CreateMap<CarWashShopCreation, CarWashShop>()
                .ForMember(x => x.Owners, opt => opt.Ignore())
                .ForMember(x => x.CarWashShopsServices, opt => opt.Ignore());

            CreateMap<CarWashShopUpdate, CarWashShop>()
                .ReverseMap();

            CreateMap<CarWashShop, OwnersViewPerShop>()
                .ForMember(x => x.Owners, opt => opt.MapFrom(CarWashShopOwners2OwnerView));

            CreateMap<OwnerRemovalRequest, OwnerDisbandRequestView>()
                .ForMember(x => x.RequesterUserName, opt => opt.MapFrom(x => x.Requester.UserName))
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShop.Name));

            CreateMap<CarWashShopRemovalRequest, OwnerShopRemovalRequestView>()
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShop.Name));
        }

        private List<ServiceView> CarWashShopsOwners2CarWashShopView(CarWashShop entity, CarWashShopView view)
        {
            var result = new List<ServiceView>();
            var service = new List<Service>();
            entity.CarWashShopsServices.ForEach(x => service.Add(x.Service));
            service.ForEach(x => result.Add(new ServiceView { Id = x.Id, Name = x.Name, Description = x.Description, Price = x.Price }));
            return result;
        }

        private List<string> CarWashShopOwners2OwnerView(CarWashShop entity, OwnersViewPerShop view)
        {
            var result = new List<string>();
            entity.Owners.ForEach(x => result.Add( x.Owner.UserName));
            return result;
        }
    }
}
