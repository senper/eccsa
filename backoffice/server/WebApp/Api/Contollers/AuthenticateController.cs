using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthenticateController : BaseController
    {
        protected readonly UserManager<User> userManager;
        private readonly SignInManager<User> _signInManager;
        protected ILogger<AuthenticateController> _logger;
        private readonly UserAdministrationRepository _adminRepository;
        private readonly IConfiguration _configuration;
        public AuthenticateController(ILogger<AuthenticateController> logger,
            UserManager<User> userManager,
            IConfiguration configuration,
            UserAdministrationRepository adminRepository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository,
            SignInManager<User> signInManager) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            this.userManager = userManager;
            _signInManager = signInManager;
            _adminRepository = adminRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Launched")]
        public async Task<IActionResult> Launched()
        {
            bool launched = false;
            try
            {

                launched = await _adminRepository.Launched();

            }
            catch
            {

            }
            return Ok(new
            {
                launched
            });
        }

        [HttpPost]
        [Route("Launch")]
        public async Task<IActionResult> Launch([FromBody] LaunchModel model)
        {
            bool launched = false;
            try
            {
                launched = await _adminRepository.Launch(model.Code);
            }
            catch
            {

            }
            return Ok(new
            {
                launched
            });
        }
        // GET: api/<controller>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] InputModel model)
        {
            try
            {

                var user = await userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return BadRequest("Invalid login attempt.");
                }

                int passwordDisabledTime = 1;
                try
                {
                    passwordDisabledTime = _configuration.GetValue<int>("PasswordDisabledTime");
                    passwordDisabledTime = Math.Max(Math.Min(passwordDisabledTime, 48), 1);
                }
                catch { }
                if (user.Disabled && user.PasswordDisabledTime != null && user.PasswordDisabledTime <= DateTime.UtcNow.AddHours(-passwordDisabledTime))
                {
                    user.Disabled = false;
                    user.PasswordDisabledTime = null;
                    user.PasswordTryCount = 0;
                    await _adminRepository.UpdateUserLoginStatus(user);
                }
                if (user.Disabled)
                {
                    return BadRequest("Invalid login attempt.");
                }
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    if (!user.EmailConfirmed || !user.MobileNoConfirmed)
                    {
                        return Ok(new
                        {
                            success = false,
                            message = "Email and/or mobile no not confirmed",
                            isNew = user.OldEmail == null && user.OldMobileNo == null
                        });
                    }

                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }


                    var jwtSecret = "abcdefghijklmnopqrstuvwxyz1234567890"; //_configuration["JWT:Secret"]
                    var jwtValidIssuer = "http://localhost:44365"; //_configuration["JWT:ValidIssuer"]
                    var jwtValidAudience = "graphql"; //_configuration["JWT:ValidAudience"]

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

                    var token = new JwtSecurityToken(
                        issuer: jwtValidIssuer,
                        audience: jwtValidAudience,
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );


                    await _adminRepository.SaveUserHistory(new UserLoginHistory
                    {
                        UserId = user.Id,
                        Date = DateTime.UtcNow.Date,
                        Time = DateTime.UtcNow,
                        IPAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    });

                    user.Disabled = false;
                    user.PasswordDisabledTime = null;
                    user.PasswordTryCount = 0;
                    await _adminRepository.UpdateUserLoginStatus(user);

                    return Ok(new
                    {
                        success = true,
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                {
                    int maxTryCount = 5;
                    try
                    {
                        maxTryCount = _configuration.GetValue<int>("MaxPasswordTryCount");
                        maxTryCount = Math.Max(Math.Min(maxTryCount, 10), 2);
                    }
                    catch { }
                    if (user.PasswordTryCount >= maxTryCount)
                    {
                        user.Disabled = true;
                        user.PasswordDisabledTime = DateTime.UtcNow;
                    }
                    else
                    {
                        user.PasswordTryCount++;
                    }
                    await _adminRepository.UpdateUserLoginStatus(user);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] RegisterInputModel model, IFormCollection formCollection)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var errorMsg = string.Empty;
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                AssignedModules = 0,
                DefaultModule = 0,
                PhoneNumber = model.PhoneNo,
                FullName = model.FullName,
                TIN = model.TIN,
                Address = model.Address,
                CompanyName = model.CompanyName,
                MobileNo = model.MobileNo,
                BusinessAccount = model.ForBusiness,
            };
            var emailUser = await userManager.FindByEmailAsync(user.Email);
            if (emailUser != null)
            {
                return Ok(new
                {
                    errorMsg = "Email Already user by other user."
                });
            }
            var file = formCollection.Files.FirstOrDefault(f => f.Name == "logo");
            if (file != null)
            {
                var kbyteSize = file.Length / 1000;
                if (kbyteSize > 65)
                {
                    return Ok(new
                    {
                        errorMsg = "Invalid Image size for logo! Size must be below 65kb"
                    });
                }
                List<string> imageExts = new() { /*"pjp", "jfif", "jpg", "jpeg", "pjpeg", "bmp", "gif",*/ "png" };
                string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                if (!imageExts.Contains(extension.Replace(".", "").ToLower()))
                {
                    return Ok(new
                    {
                        errorMsg = "Invalid Image for Log! Only PNG Allowed"
                    });
                }
                user.LogoExtension = extension;
            }
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (file != null)
                {
                    var fileName = file.FileName;
                    await SaveLogo(user.Id, file);
                }
                _logger.LogInformation("User created a new account with password.");

                //var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { userId = user.Id, code = code },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new
                {
                    success = true
                });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }
            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                errorMsg
            });
        }

    }
    public class LaunchModel
    {
        public string Code { get; set; }
    }
    public class InputModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class RegisterInputModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string TIN { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string CompanyName { get; set; }
        public bool ForBusiness { get; set; }
    }
    public class GetEmailMobileConfirmationModel
    {
        public string Email { get; set; }
        public string ConfirmationUrl { get; set; }
    }
    public class VerifyEmailMobileModel
    {
        public string EmailCode { get; set; }
        public string MobileNoCode { get; set; }
    }
    public class UpdateProfileModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        //public string Tin { get; set; }
        public string PhoneNo { get; set; }
    }
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordModel
    {
        public string ResetCode { get; set; }
        public string NewPassword { get; set; }
    }

    public enum Redirects
    {
        Login = 1,
        Lokout = 2,
        Registration = 3
    }
}
