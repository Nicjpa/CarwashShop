using AutoMapper;
using CarWashShopAPI.DTO.BookingDTO;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.OwnerDTO;
using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.DTO.UserDTOs;
using CarWashShopAPI.Entities;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ServiceCreationAndUpdate, Service>()
                .ReverseMap();

            CreateMap<Service, ServiceView>();

            CreateMap<Service, ServiceViewWithShopAssigned>()
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShops.Select(x => x.CarWashShop.Name).FirstOrDefault()))
                .ForMember(x => x.CarWashShopId, opt => opt.MapFrom(x => x.CarWashShops.Select(x => x.CarWashShop.Id).FirstOrDefault()));

            CreateMap<CarWashShopCreation, Service>();

            CreateMap<CarWashShop, CarWashShopView>()
                .ForMember(x => x.Services, opt => opt.MapFrom(CarWashShopsOwners2CarWashShopView));

            CreateMap<CarWashShopCreation, CarWashShop>()
                .ForMember(x => x.Owners, opt => opt.Ignore())
                .ForMember(x => x.CarWashShopsServices, opt => opt.Ignore());

            CreateMap<CarWashShopUpdate, CarWashShop>()
                .ReverseMap();

            CreateMap<CarWashShop, ListOfOwnersPerShopView>()
                .ForMember(x => x.Owners, opt => opt.MapFrom(CarWashShopOwners2OwnerView));

            CreateMap<DisbandRequest, DisbandRequestView>()
                .ForMember(x => x.RequesterUserName, opt => opt.MapFrom(x => x.Requester.UserName))
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShop.Name));

            CreateMap<ShopRemovalRequest, ShopRemovalRequestView>()
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShop.Name));

            CreateMap<BookingCreation, Booking>();

            CreateMap<Booking, BookingViewConsumerSide>()
                .ForMember(x => x.ScheduledDate, opt => opt.MapFrom(x => x.ScheduledDateTime.Date.ToString("ddd, dd MMM yyyy")))
                .ForMember(x => x.ScheduledTime, opt => opt.MapFrom(x => x.ScheduledDateTime.Hour.ToString() + ":00"))
                .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Service.Price))
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShop.Name))
                .ForMember(x => x.ServiceName, opt => opt.MapFrom(x => x.Service.Name));
                

            CreateMap<Booking, BookingViewOwnerSide>()
                .ForMember(x => x.ScheduledDate, opt => opt.MapFrom(x => x.ScheduledDateTime.Date.ToString("ddd, dd MMM yyyy")))
                .ForMember(x => x.ScheduledTime, opt => opt.MapFrom(x => x.ScheduledDateTime.Hour.ToString() + ":00"))
                .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Service.Price))
                .ForMember(x => x.CarWashShopName, opt => opt.MapFrom(x => x.CarWashShop.Name))
                .ForMember(x => x.ServiceName, opt => opt.MapFrom(x => x.Service.Name))
                .ForMember(x => x.ConsumerUsername, opt => opt.MapFrom(x => x.Consumer.UserName))
                .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Consumer.Email))
                .ForMember(x => x.ContactPhone, opt => opt.MapFrom(x => x.Consumer.PhoneNumber))
                .ForMember(x => x.DateCreated, opt => opt.MapFrom(x => x.DateCreated.ToString("ddd, dd MMM yyyy HH':'mm")));

            CreateMap<CustomUser, UserView>();
            CreateMap<UserInfo, UserLogin>();

        }

        private List<ServiceView> CarWashShopsOwners2CarWashShopView(CarWashShop entity, CarWashShopView view)
        {
            var result = new List<ServiceView>();
            var service = new List<Service>();
            entity.CarWashShopsServices.ForEach(x => service.Add(x.Service));
            service.ForEach(x => result.Add(new ServiceView { Id = x.Id, Name = x.Name, Description = x.Description, Price = x.Price }));
            return result;
        }

        private List<string> CarWashShopOwners2OwnerView(CarWashShop entity, ListOfOwnersPerShopView view)
        {
            var result = new List<string>();
            entity.Owners.ForEach(x => result.Add( x.Owner.UserName));
            return result;
        }
    }
}
