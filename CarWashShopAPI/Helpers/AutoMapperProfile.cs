using AutoMapper;
using CarWashShopAPI.DTO.CarWashShopDTOs;
using CarWashShopAPI.DTO.ServiceTypeDTO;
using CarWashShopAPI.Entities;

namespace CarWashShopAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ServiceTypeCreation, ServiceType>();
            //CreateMap<CarWashShopCreation, CarWashShop>()
            //    .ForMember(x => x.CarWashOwners, opt => opt.MapFrom(CarWashCreation2CarWash));

            //CreateMap<Franchise, FranchiseView>()
            //    .ForMember(x => x.CarWashes, opt => opt.MapFrom(CarWash2CarWashView));

        }


        //private List<CarWashShopsOwners> CarWashCreation2CarWash(CarWashShopCreation creation, CarWashShop entity)
        //{
        //    var result = new List<CarWashShopsOwners>();
        //    foreach (string ownerName in creation.OwnerUserNames)
        //    {
        //        result.Add(new CarWashShopsOwners() { OwnerId = ownerName.ToUpper() });
        //    }
        //    return result;
        //}

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
