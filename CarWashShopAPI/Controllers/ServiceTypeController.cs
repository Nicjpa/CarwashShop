using AutoMapper;
using CarWashShopAPI.DTO.ServiceTypeDTO;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashShopAPI.Controllers
{
    [Route("api/serviceType")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public ServiceTypeController(CarWashDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("GetAllServiceTypes", Name = "getAllServiceTypes")]
        public async Task<ActionResult<List<ServiceType>>> Get()
        {
            var allServiceTypeEntities = _dbContext.ServiceTypes;
            return Ok(allServiceTypeEntities);
        }

        [HttpPost("AddServiceType", Name = "addServiceType")]
        public async Task<ActionResult> Post([FromBody] ServiceTypeCreation serviceTypeCreation)
        {
            var serviceTypeEntity = _mapper.Map<ServiceType>(serviceTypeCreation);
            _dbContext.ServiceTypes.Add(serviceTypeEntity);
            await _dbContext.SaveChangesAsync();

            return Ok($"You have successfully added '{serviceTypeEntity.Name}' as ServiceType.");
        }

        [HttpPut("{id:int}", Name = "EditServiceType")]
        public async Task<ActionResult<ServiceType>> Put(int id, [FromBody] ServiceTypeCreation serviceTypeCreation)
        {
            var serviceTypeEntity = _dbContext.ServiceTypes.FirstOrDefault(x => x.Id == id);
            if(serviceTypeEntity == null)
            {
                return BadRequest("ServiceType doesn't exists..");
            }

            serviceTypeEntity = _mapper.Map(serviceTypeCreation, serviceTypeEntity);
            _dbContext.Entry(serviceTypeEntity).State = EntityState.Modified;

            return Ok(serviceTypeEntity);
        }

    }
}
