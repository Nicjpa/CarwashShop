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
        private readonly ILogger<AccountManagementController> _logger;
        private readonly UserManager<CustomUser> _userManager;

        public AccountManagementController(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            IConfiguration configuration,
            CarWashDbContext dbContext,
            IMapper mapper,
            IAccountRepository accountRepository,
            ILogger<AccountManagementController> logger
            )
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _mapper = mapper;
            _accountRepository = accountRepository;
            _logger = logger;
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

                _logger.LogInformation($" / POST / MethodName: 'CreateUser' " +
                   $"/ user has been created '{userInfo.UserName.ToLower()}' / USER HAS BEEN CREATED - TOKEN GRANTED ");

                return await _accountRepository.BuildToken(buildToken);
            }
            else
            {
                _logger.LogInformation($" / POST / MethodName: 'CreateUser' " +
                   $"/ unable to build token: {result.Errors} / CREATE USER FAILED ");

                return BadRequest(result.Errors);
            }
        }



        [HttpPost("Login", Name = "login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserLogin model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation($" / POST / MethodName: 'Login' " +
                   $"/ user '{model.UserName}' logged in / LOGIN SUCCESS - TOKEN GRANTED ");

                return await _accountRepository.BuildToken(model);
            }
            else
            {
                _logger.LogInformation($" / POST / MethodName: 'Login' " +
                   $"/ user '{model.UserName}' failed to login / FAILED TO LOGIN ");

                return BadRequest("Invalid login attempt");
            }
        }



        [HttpGet("GetUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<List<UserView>>> Get([FromQuery] UserFilter filter)
        {
            var userEntities = await _accountRepository.GetUsers(filter);

            if (userEntities == null || !userEntities.Any())
            {
                _logger.LogInformation($" / GET / MethodName: 'Login' " +
                   $"/ user '{filter.UserName}' not found / FAILED TO FIND USER ");

                return NotFound("No user found..");
            }

            var usersPaginated = await _accountRepository.Pagination(HttpContext, userEntities, filter.RecordsPerPage, filter.Pagination);

            var userView = _mapper.Map<List<UserView>>(usersPaginated);

            _logger.LogInformation($" / GET / MethodName: 'Login' " +
                   $"/ user '{filter.UserName}' found / USER FOUND ");

            return Ok(userView);
        }



        [HttpDelete("DeleteUserByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Get(string userEmail)
        {
            var user = await _dbContext.CustomUsers.FirstOrDefaultAsync(x => x.Email == userEmail);

            if (user == null)
            {
                _logger.LogInformation($" / DELETE / MethodName: 'Login' " +
                   $"/ user with email '{userEmail}' not found / BAD ATTEMPT TO DELETE USER ");

                return NotFound($"User with '{userEmail}' doesn't exist..");
            }

            if (user.UserName == User.Identity.Name)
            {
                _logger.LogInformation($" / DELETE / MethodName: 'Login' " +
                   $"/ admin is tried to delete himself :) / BAD ATTEMPT TO DELETE YOURSELF ");

                return BadRequest("You cannot delete yourself!");
            }
                

            if (user.Role == "Owner")
            {
                await _accountRepository.DeleteUserAssets(user);
                _dbContext.CustomUsers.Remove(user);
                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation($" / DELETE / MethodName: 'Login' " +
                   $"/ user '{user.UserName}' with email '{user.Email}' has been deleted / USER REMOVED ");

            return Ok($"You have successfully deleted user with email '{user.Email}' and username '{user.UserName}'.");
        }
    }
}
