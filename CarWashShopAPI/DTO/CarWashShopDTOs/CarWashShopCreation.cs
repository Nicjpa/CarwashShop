﻿using CarWashShopAPI.DTO.ServiceDTO;
using CarWashShopAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace CarWashShopAPI.DTO.CarWashShopDTOs
{
    public class CarWashShopCreation
    {
        public string Name { get; set; }
        public string? AdvertisingDescription { get; set; }
        public int AmountOfWashingUnits { get; set; }
        public int OpeningTime { get; set; }
        public int ClosingTime { get; set; }
        public List<ServiceCreationView> Services { get; set; }
        public List<string>? CarWashShopsOwners { get; set; } = new List<string>();
    }
}
