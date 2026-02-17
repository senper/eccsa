using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class EswController : BaseController
    {
        private readonly ILogger<EswController> _logger;
        private readonly ExternalDataRepository _repository;
        public EswController(ILogger<EswController> logger,
            UserManager<User> userManager,
            ExternalDataRepository repository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
           
        }

        public async Task<IActionResult> Index( DateTime importDate
             
           )
        {
            var userName = TheUser.FullName;
            CertificateViewModel model;
            model = new CertificateViewModel
            {
                TheList = await _repository.CertificateOfOrigins(importDate),
                EswRequests = await _repository.EswRequests()

            };
            
           // model.TheList = await _repository.CertificateOfOrigins(importDate);
            TempData["title"] = model.CurrentPage;
            model.CurrentModule = ECCSAModuleFlag.Administration;
            model.ImportDate = importDate;
            return View(model);
        }


        //public async Task<IActionResult> TeleBirr()
        //{
        //    var userName = TheUser.FullName;
        //    CertificateViewModel model;
        //    model = new CertificateViewModel
        //    {
        //        TheList = await _repository.pi(),
        //        EswRequests = await _repository.EswRequests()

        //    };

        //    // model.TheList = await _repository.CertificateOfOrigins(importDate);
        //    TempData["title"] = model.CurrentPage;
        //    model.CurrentModule = ECCSAModuleFlag.Administration;
        //    model.ImportDate = importDate;
        //    return View(model);
        //}

    }
}
