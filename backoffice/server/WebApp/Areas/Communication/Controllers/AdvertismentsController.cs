using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class AdvertismentsController : BaseController
    {
        private readonly ILogger<AdvertismentsController> _logger;
        private readonly AdvertizementRepository _repository;
        public AdvertismentsController(
            UserManager<User> userManager
            , ILogger<AdvertismentsController> logger
            , AdvertizementRepository repository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Advertisments";
        }
        public async Task<IActionResult> Index()
        {
            var model = new AdvertViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.List();
            }
            catch (Exception ex)
            {
                model.TheList = new List<Advertizement>();

                _logger.LogError(ex, "Index");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Advertizement";
            model.CurrentOperation = "Index";
            model.ChangeActive(MenuItemEnum.CommunicationAdvertisment);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            var model = new AdvertViewModel();
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheModel = await _repository.ById(id);
                var r = model.TheModel;
                string fileName = $"{r.CompanyName}{r.Extension}";
                var fname = $"{Guid2String(id)}{r.Extension}";
                model.TheModel.Url = Path.Combine(RelativePath(r.Section), fname);
            }
            catch (Exception ex)
            {
                model.TheList = new List<Advertizement>();
                _logger.LogError(ex, "Detail");
            }
            model.CurrentPage = "Advertizement";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.CommunicationAdvertisment);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Advertizement model, IFormFile upload)
        {
            model.Id = model.Id == Guid.Empty ? Guid.NewGuid() : model.Id;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                if (upload != null && upload.Length > 0)
                {
                    model.Extension = await SaveDocument(model.Id, model.Section, upload);
                }
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (model.Id == Guid.Empty)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Detail", new { id = model.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Renew(Advertizement model)
        {
            LogStart(_logger, controllerName, "Renew");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var data = await _repository.ById(model.Id);
                if (data == null)
                {
                    CommandStatus("Invalid Id");
                    return RedirectToAction("Index");
                }
                data.EndDate = model.EndDate;
                await _repository.Save(data, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            LogStart(_logger, controllerName, "Delete");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Delete");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                var res = await _repository.ById(id);
                await _repository.Delete(id);
                if (!string.IsNullOrEmpty(res.Extension))
                {
                    var fname = Guid2String(id) + res.Extension;
                    var path = Path.Combine(AbsolutePath(res.Section), fname);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id)
        {
            var r = await _repository.ById(id);
            string fileName = $"{r.CompanyName}{r.Extension}";
            var fname = $"{Guid2String(id)}{r.Extension}";
            return await DownLoadFile(Path.Combine(AbsolutePath(r.Section), fname), fileName);
        }
    }
}
