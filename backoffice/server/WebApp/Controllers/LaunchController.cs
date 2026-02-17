using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    [Authorize]
    public class LaunchController : Controller
    {
        private readonly UserAdministrationRepository _adminRepository;
        public async Task< IActionResult> Index()
        {
           var  _launched = await _adminRepository.Launched();
            return View();
        }
        public LaunchController(UserAdministrationRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Launch()
        {
       
            try
            {
                 await _adminRepository.Launch(String.Empty);

                return RedirectToAction("Index", "Dashboard", new {@Area="Membership" });
            }
            catch 
            {
                return RedirectToAction("Index", "Dashboard", new { @Area = "Membership" });
            }

            
        }
    }
}
