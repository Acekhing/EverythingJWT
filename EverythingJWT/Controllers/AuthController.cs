using AutoMapper;
using EverythingJWT.Constants;
using EverythingJWT.Data;
using EverythingJWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EverythingJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly BookstoreDbContext dbContext;
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthController> logger;

        public AuthController(IMapper mapper, UserManager<ApiUser> userManager, BookstoreDbContext dbContext, IConfiguration configuration, ILogger<AuthController> logger)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.logger = logger;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = mapper.Map<ApiUser>(model);
            var response = await userManager.CreateAsync(user, model.Password);

            if (response.Succeeded)
            {
                logger.LogInformation("{0} created an account", user.Email);
                
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                logger.LogInformation(code);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var registerResponse = new AuthResponse
                {
                    Email = model.Email,
                    Token = code,
                    UserId = user.Id,
                    UserName = user.UserName
                };

                return Accepted(registerResponse);
            }


            foreach (var error in response.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !isValidPassword)
            {
                return Unauthorized(model);
            }

            //Generate Token
            string tokenString = await GenerateToken(user);

            var response = new AuthResponse
            {
                Email = model.Email,
                Token = tokenString,
                UserId = user.Id,
                UserName = user.UserName
            };

            return Accepted(response);
        }


        [HttpGet]
        [Route("get_users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await dbContext.Users.ToListAsync();
            var userModels = mapper.Map<IEnumerable<UserModel>>(users);
            return Ok(userModels);
        }


        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Roles
            var roles = await userManager.GetRolesAsync(user);

            // Map roles to claims
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            // Get user claims
            var userClaims = await userManager.GetClaimsAsync(user);

            // Claims
            var claims = new List<Claim>
            {
                // https://datatracker.ietf.org/doc/html/rfc7519#section-4
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimNames.Uid, user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            // Token
            var tokem = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings:Duration"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokem);
        }

    }
}
