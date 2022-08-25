using AutoMapper;
using CarWashShopAPI.DTO.UserDTOs;
using CarWashShopAPI.Entities;
using CarWashShopAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static CarWashShopAPI.DTO.Enums;

namespace CarWashShopAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class AccountManagementController : ControllerBase
    {
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly UserManager<CustomUser> _userManager;

        public AccountManagementController(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            IConfiguration configuration,
            CarWashDbContext dbContext,
            IMapper mapper,
            IAccountRepository accountRepository
            )
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _userManager = userManager;
        }

        [HttpPost("CreateUser", Name = "createUser")]
        public async Task<ActionResult<UserToken>> CreateUser(UserInfo userInfo, RoleClaim role)
        {
            var user = new CustomUser
            {
                FirstName = userInfo.FirstName.ToUpper(),
                LastName = userInfo.LastName.ToUpper(),
                Address = userInfo.Address.ToUpper(),
                PhoneNumber = userInfo.PhoneNumber,
                Email = userInfo.Email,
                UserName = userInfo.UserName.ToLower(),
                Role = role.ToString()
            };

            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (result.Succeeded)
            {
                var buildToken = _mapper.Map<UserLogin>(userInfo);

                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role.ToString()));
                return await _accountRepository.BuildToken(buildToken);
            }
            else
                return BadRequest(result.Errors);
        }



        [HttpPost("Login", Name = "login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserLogin model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
                return await _accountRepository.BuildToken(model);
            else
                return BadRequest("Invalid login attempt");
            
        }



        [HttpGet("GetUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UserView>>> Get([FromQuery] UserFilter filter)
        {
            var userEntities = await _accountRepository.GetUsers(filter);

            if (userEntities == null || !userEntities.Any())
                return NotFound("No user found..");

            var usersPaginated = await _accountRepository.Pagination(HttpContext, userEntities, filter.RecordsPerPage, filter.Pagination);

            var userView = _mapper.Map<List<UserView>>(usersPaginated);

            return Ok(userView);
        }



        [HttpDelete("DeleteUserByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Get(string userEmail)
        {
            var user = await _dbContext.CustomUsers.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
                return NotFound($"User with '{userEmail}' doesn't exist..");

            if (user.UserName == User.Identity.Name)
                return BadRequest("You cannot delete yourself!");

            if (user.Role == "Owner")
            {
                await _accountRepository.DeleteUserAssets(user);

                _dbContext.CustomUsers.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            return Ok($"You have successfully deleted user with email '{user.Email}' and username '{user.UserName}'.");
        }
    }
}
