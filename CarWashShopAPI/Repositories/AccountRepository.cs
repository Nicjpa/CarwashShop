using CarWashShopAPI.DTO;
using CarWashShopAPI.DTO.UserDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Helpers;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarWashShopAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CarWashDbContext _dbContext;
        private readonly UserManager<CustomUser> _userManager;
        private readonly IConfiguration _config;

        public AccountRepository(CarWashDbContext dbContext, UserManager<CustomUser> userManager, IConfiguration config)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _config = config;
        }
        public async Task<List<T>> Pagination<T>(HttpContext httpContext, IQueryable<T> genericList, int recPerPage, PaginationDTO paginationDTO)
        {
            return await CustomStaticFunctions.GenericPagination(httpContext, genericList, recPerPage, paginationDTO);
        }

        public async Task<UserToken> BuildToken(UserLogin userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.UserName.ToLower())
            };

            var identityUser = await _userManager.FindByNameAsync(userInfo.UserName);
            var claimsDB = await _userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public async Task DeleteUserAssets(CustomUser user)
        {
            var allCarWashShopsOfUser = await _dbContext.CarWashsShops
                        .Include(x => x.Owners)
                        .ThenInclude(x => x.Owner)
                        .Include(x => x.Bookings)
                        .Include(x => x.CarWashShopsServices)
                        .ThenInclude(x => x.Service)
                        .Where(x => x.Owners.Any(x => x.Owner.UserName == user.UserName)).ToListAsync();

            var services = new List<Service>();

            var carWashShops = allCarWashShopsOfUser.Where(x => x.Owners.Count == 1).ToList();

            if (carWashShops.Any())
            {
                carWashShops.ForEach(x => x.CarWashShopsServices.ForEach(x => services.Add(x.Service)));
                _dbContext.Services.RemoveRange(services);
                _dbContext.CarWashsShops.RemoveRange(carWashShops);
            }
        }

        public async Task<IQueryable<CustomUser>> GetUsers(UserFilter filter)
        {
            var entities = _dbContext.CustomUsers
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Id))
                entities = entities.Where(x => x.Id == filter.Id);
            else if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
                entities = entities.Where(x => x.PhoneNumber == filter.PhoneNumber);
            else
            {
                if (filter.Role != null)
                    entities = entities.Where(x => x.Role == filter.Role.ToString());

                if (!string.IsNullOrWhiteSpace(filter.Email))
                    entities = entities.Where(x => x.Email.Contains(filter.Email));

                if (!string.IsNullOrWhiteSpace(filter.UserName))
                    entities = entities.Where(x => x.UserName.Contains(filter.UserName));

                if (!string.IsNullOrWhiteSpace(filter.FirstName))
                    entities = entities.Where(x => x.FirstName.Contains(filter.FirstName));

                if (!string.IsNullOrWhiteSpace(filter.LastName))
                    entities = entities.Where(x => x.LastName.Contains(filter.LastName));

                if (!string.IsNullOrWhiteSpace(filter.Address))
                    entities = entities.Where(x => x.Address.Contains(filter.Address));
            }
            return entities;
        }
    }
    
}
