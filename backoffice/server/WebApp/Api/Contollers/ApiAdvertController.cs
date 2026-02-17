using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiAdvertController : BaseController
    {
        private readonly AdvertizementRepository _repository;
        private readonly YesNoRepository _yesNorepository;
        public ApiAdvertController(
           UserManager<User> userManager
            , PublicUserRepository userRepository
            , AdvertizementRepository repository
            , YesNoRepository yesNoRepository
           , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _repository = repository;
            _yesNorepository = yesNoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdImages(AdvertisementSectionEnum section)
        {
            if (section == AdvertisementSectionEnum.None)
                return NotFound();
            var subFolder = Enum.GetName(typeof(AdvertisementSectionEnum), section).ToLower();
            string filePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        subFolder);

            var ads = await _repository.List(section);
            ads = ads.Where(a => a.StartDate <= DateTime.Today && a.EndDate >= DateTime.Today).ToList();

            var list = new List<string>();
            //if (Directory.Exists(filePath))
            //{
            //    list = Directory.GetFiles(filePath).Select(f =>
            //    f.Split(filePath).Length > 1 ? f.Split(filePath)[1].Replace("\\", "")
            //    .Replace("/", "") : "").ToList();
            //}
            foreach (var ad in ads)
            {
                var fileName = Guid2String(ad.Id) + ad.Extension;
                var fileFullPath = Path.Combine(filePath, fileName);
                if (System.IO.File.Exists(fileFullPath))
                {
                    list.Add(fileName);
                }
            }
            return Ok(list);
        }
        [HttpGet]
        public async Task<IActionResult> GetSettingValue(string key)
        {
            var setting = await _yesNorepository.GetSettingByKey(key);
            if (setting?.Value != null)
            {
                return Ok(setting?.Value);
            }
            else
            {
                return BadRequest($"Setting with key '{key}' Not Found");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSettingValues([FromQuery] List<string> keys)
        {
            var settings = await _yesNorepository.GetSettingByKey(keys);
            return Ok(settings);
        }
    }
}






