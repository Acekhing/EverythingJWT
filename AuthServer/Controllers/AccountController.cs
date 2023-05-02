using AuthServer.Data;
using AuthServer.Infrastructure;
using AuthServer.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        // Account Endpoints
        // 1. Register
        // 2. Login
        // 3. Password Reset
        // 4. Email Confirmation
        // 5. Delete Account
        // 6. Remove Account

        // Account Manage Endpoints
        // 1. Assign Role
        // 2. Assign Claim
        // 3. Roles
        // 4. Claims
        // 5. Logins
        // 6. Users

        private readonly UserManager<AccountUser> userManager;
        private readonly IMapper mapper;
        private readonly ILogger<AccountController> logger;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<AccountUser> userManager, IMapper mapper, ILogger<AccountController> logger, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.logger = logger;
            this.emailSender = emailSender;
        }


        [HttpPost]
        [Route("Register")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            logger.LogInformation("Registeration started...");

            // Check if user has not registered already

            var existingUser = await userManager.FindByNameAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), $"An account exist with email: {model.Email}");
                return BadRequest(ModelState);
            }

            var user = mapper.Map<AccountUser>(model);

            var response = await userManager.CreateAsync(user, model.Password);

            if (response.Succeeded == false)
            {
                foreach (var error in response.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return BadRequest(ModelState);
            }

            if (existingUser == null && response.Succeeded)
            {
                logger.LogInformation("Generating email confirmation code...");

                // Generate email confirmation code 
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                // Encode code
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


                try
                {
                    // Send email
                    var callbackUrl = Url.Action(
                        "ConfirmEmail", 
                        "Account", 
                        new { email = model.Email, token = code },
                        Request.Scheme
                    );

                    await emailSender.SendEmailAsync(model.Email, "Penpab Email Confirmation", callbackUrl);

                    return Accepted(new RegistrationResponse { UserId = user.Id, EmailConfirmationCode = callbackUrl });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(ex.Source, ex.Message);
                    return BadRequest(ModelState);
                }
                
            }

            // Return badrequest
            foreach (var error in response.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            logger.LogInformation("Login started...");

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(nameof(model.Email), $"No account associated with email: {model.Email}");
                return BadRequest(ModelState);
            }

            logger.LogInformation("Generating token...");

            return Accepted();
        }


        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword()
        {
            return Accepted();
        }

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            return Accepted();
        }

        [HttpPut]
        [Route("manage/delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            return NoContent();
        }

        [HttpPut]
        [Route("manage/assign-role")]
        public async Task<IActionResult> AssignRole()
        {
            return NoContent();
        }

        [HttpPut]
        [Route("manage/assign-claim")]
        public async Task<IActionResult> AssignClaim()
        {
            return NoContent();
        }

        [HttpDelete]
        [Route("manage/remove-account")]
        public async Task<IActionResult> RemoveAccount()
        {
            return NoContent();
        }

        [HttpGet]
        [Route("manage/users")]
        [ProducesResponseType(typeof(UserModel), 200)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userManager.Users.ToListAsync();
            var userModels = mapper.Map<List<UserModel>>(users);
            return Ok(userModels);
        }

        [HttpGet]
        [Route("manage/roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok();
        }

        [HttpGet]
        [Route("manage/claims")]
        public async Task<IActionResult> GetClaims()
        {
            return Ok();
        }

        [HttpGet]
        [Route("manage/logins")]
        public async Task<IActionResult> GetExternalLogins()
        {
            return Ok();
        }

    }
}
