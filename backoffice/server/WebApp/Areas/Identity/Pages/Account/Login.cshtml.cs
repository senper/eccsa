using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [AutoValidateAntiforgeryToken]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserAdministrationRepository _adminRepository;
        private readonly EccsaContext _context;
        private readonly UserManager<User> _userManager;
        private bool _launched;
        public LoginModel(SignInManager<User> signInManager
            , UserManager<User> userManager
            , ILogger<LoginModel> logger
            , IConfiguration configuration,
            UserAdministrationRepository adminRepository,
             EccsaContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _adminRepository = adminRepository;

        }
        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            //[Required]
            //[EmailAddress]
            //public string Email { get; set; }

            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var userManager = _signInManager.UserManager;
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        var user = await userManager.FindByNameAsync(Input.UserName);

                        int passwordDisabledTime = 1;
                        try
                        {
                            passwordDisabledTime = _configuration.GetValue<int>("PasswordDisabledTime");
                            passwordDisabledTime = Math.Max(Math.Min(passwordDisabledTime, 48), 1);
                        }
                        catch { }
                        if (user != null && user.Disabled && user.PasswordDisabledTime != null && user.PasswordDisabledTime <= DateTime.UtcNow.AddHours(-passwordDisabledTime))
                        {
                            user.Disabled = false;
                            user.PasswordDisabledTime = null;
                            user.PasswordTryCount = 0;
                            await _adminRepository.UpdateUserLoginStatus(user);
                        }
                        if (user == null || user.Disabled || user.AssociationId == Guid.Empty)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }
                        _logger.LogInformation("User logged in.");

                        if (user.DefaultModule == 0 || user.AssignedModules == 0)
                        {
                            _logger.LogWarning("No default module assigned Or No Assigned Modules.");
                            ModelState.AddModelError(string.Empty, "User has no default module.");
                            return Page();
                        }

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

                        var _launched = await _adminRepository.Launched();
                        if (!_launched)
                        {
                            return LocalRedirect("~/Launch/Index");
                        }
                        string redirectUrl = GetDefaultRedirectModuleUrl(user);
                        return LocalRedirect(redirectUrl);
                    }
                    else
                    {
                        var user = await userManager.FindByNameAsync(Input.UserName);
                        if (user != null && !user.Disabled)
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
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                } 
                catch (Exception ex)
                {

                }
            }

            return Page();
        }

        private static string GetDefaultRedirectModuleUrl(User user)
        {
            var redirectUrl = "";
            switch (user.DefaultModule)
            {
                case Model.Common.ECCSAModuleFlag.Academy:
                    redirectUrl = "~/Academy?module=Academy";
                    break;
                case Model.Common.ECCSAModuleFlag.Administration:
                    redirectUrl = "~/Administration?module=Administration";
                    break;
                case Model.Common.ECCSAModuleFlag.Arbitration:
                    if (user.IsArbitrator)
                        redirectUrl = "~/Service/Arbitrator/Index?module=Arbitration";
                    else
                        redirectUrl = "~/Service/ServiceRequest/Index?module=Arbitration";
                    break;
                case Model.Common.ECCSAModuleFlag.Communication:
                    redirectUrl = "~/Communication/Dashboard/Index?module=Communication";
                    break;
                case Model.Common.ECCSAModuleFlag.Membership:

                    redirectUrl = "~/Membership?module=Membership";

                    break;
                case Model.Common.ECCSAModuleFlag.Research:
                    redirectUrl = "~/Advocacy/Dashboard/Index?module=Research";
                    break;
                case Model.Common.ECCSAModuleFlag.TradePromotion:
                    redirectUrl = "~/Service/ServiceRequest/Index?module=TradePromotion";
                    break;
                case Model.Common.ECCSAModuleFlag.Investment:
                    redirectUrl = "~/Service/ServiceRequest/Index?module=Investment";
                    break;
                case Model.Common.ECCSAModuleFlag.MandE:
                    redirectUrl = "~/mande?module=MandE";
                    break;
                case Model.Common.ECCSAModuleFlag.Support:
                    redirectUrl = "~/Support?module=Support";
                    break;
                case Model.Common.ECCSAModuleFlag.Bmo:
                    redirectUrl = "~/Bmo/Home/Index?module=Support";
                    break;
            }

            return redirectUrl;
        }
    }
}
