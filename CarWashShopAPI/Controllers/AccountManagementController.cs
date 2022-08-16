﻿using AutoMapper;
using CarWashShopAPI.DTO.UserDTOs;
using CarWashShopAPI.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarWashShopAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class AccountManagementController : ControllerBase
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly CarWashDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountManagementController(
            UserManager<CustomUser> userManager,
            SignInManager<CustomUser> signInManager,
            IConfiguration config,
            CarWashDbContext dbContext,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private async Task<UserToken> BuildToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.UserName.ToLower()),
                new Claim(ClaimTypes.Email, userInfo.UserName),
            };

            var identityUser = await _userManager.FindByEmailAsync(userInfo.UserName);
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


        [HttpPost("RenewToken", Name = "renewToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
        public async Task<ActionResult<UserToken>> Renew()
        {
            var userInfo = new UserInfo() { UserName = User.Identity.Name };
            return await BuildToken(userInfo);
        }



        [HttpPost("CreateOwnerAccount", Name = "createOwnerAcc")]
        public async Task<ActionResult<UserToken>> CreateOwnerAccount([FromBody] UserInfo userInfo)
        {
            return await CreateUser(userInfo, "Owner");
        }


        [HttpPost("CreateConsumerAccount", Name = "createConsumerAcc")]
        public async Task<ActionResult<UserToken>> CreateConsumerAccount([FromBody] UserInfo userInfo)
        {
            return await CreateUser(userInfo, "Consumer");
        }

        [HttpPost("CreateAdminAccount", Name = "createAdminAcc")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminOnly")]
        public async Task<ActionResult<UserToken>> CreateAdminAccount([FromBody] UserInfo userInfo)
        {
            return await CreateUser(userInfo, "Admin");
        }

        private async Task<ActionResult<UserToken>> CreateUser(UserInfo userInfo, string role)
        {
            try
            {
                var user = new CustomUser { UserName = userInfo.UserName.ToLower(), Email = userInfo.UserName.ToLower() };
                var result = await _userManager.CreateAsync(user, userInfo.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                    return await BuildToken(userInfo);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch
            {
                return BadRequest($"Username {userInfo.UserName} already exists..");
            }
        }

        [HttpPost("Login", Name = "login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(model);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }
    }
}