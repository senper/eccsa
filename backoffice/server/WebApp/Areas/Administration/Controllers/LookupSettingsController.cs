using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class LookupSettingsController : BaseController
    {
        private readonly ILogger<LookupSettingsController> _logger;
        private readonly YesNoRepository _repository;
        private readonly ExternalDataRepository _rep;
        public LookupSettingsController(ILogger<LookupSettingsController> logger,
            UserManager<User> userManager,
            YesNoRepository repository,
            ExternalDataRepository rep,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _rep = rep;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = TheUser.FullName;
            LookupSettingsViewModel model;
            model = new LookupSettingsViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = await _repository.LookupTypes()
            };
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> IssueLocations( string countryRegion)
        {
            var userName = TheUser.FullName;
            IssueLocationViewModel model;
            model = new IssueLocationViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = await _repository.IssueLocations(countryRegion),
                LocationTypes = await _repository.IssueLocationTypes(),
                SelectedCountry = countryRegion
            };
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var userName = TheUser.FullName;
            LookupSettingsViewModel model;
            model = new LookupSettingsViewModel
            {
                FullName = userName,
                CurrentOperation = "Detail",
                TheModel = await _repository.LookupTypeById(id)
            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            TempData["title"] = model.CurrentPage;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(LookupType data)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = data.Id });
            }
            try
            {
                
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Lookup Type", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Lookup Type", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (data.Id == Guid.Empty)
                return RedirectToAction("Index");
            else
                return RedirectToAction("Detail", new { id = data.Id });
        }

        public async Task GenerateDirectoryHeader()
        {
            DirectoryInfo root = new DirectoryInfo("F:\\ECCSA Data For 2012-13 Business Directory");
            List<LookupValue> lst = new List<LookupValue>();
            var i = 0;
            GenerateDirectory(root, null, ref i, ref lst);
            await _rep.GenerateDirectory(lst);
        }
        public void GenerateDirectory(DirectoryInfo parent, Guid? parentId, ref int i, ref List<LookupValue> lst)
        {
            Guid lookupTemplate = Guid.Parse("00000000-0000-0000-1000-000000000000");
            Guid tyeId = Guid.Parse("00000000-0000-0000-0000-000000000002");
            foreach (var dInfo in parent.GetDirectories())
            {
                i++;
                var id = FromInt(i, lookupTemplate);
                lst.Add(new LookupValue()
                {
                    Id = id,
                    Name = dInfo.Name,
                    LookupTypeId = tyeId,
                    ParentId = parentId
                });
                var children = dInfo.GetDirectories();
                if (children.Length > 0)
                {
                    GenerateDirectory(dInfo, id, ref i, ref lst);
                }
                var files = dInfo.GetFiles();
                if (files.Length > 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Name.ToLowerInvariant().Contains("category.xlsx"))
                        {
                            try
                            {
                                var dt = ImportExcel(file);
                                foreach (DataRow dr in dt.Rows)
                                {

                                    var name = dr[0].ToString();
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        i++;
                                        var cId = FromInt(i, lookupTemplate);
                                        lst.Add(new LookupValue()
                                        {
                                            Id = cId,
                                            Name = name,
                                            LookupTypeId = tyeId,
                                            ParentId = id
                                        });
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                var msg = ex.Message;
                            }
                        }
                    }

                }
            }

        }
        private Guid FromInt(int val, Guid template)
        {
            var s = template.ToString();
            var hex = val.ToString("X");
            var lenHex = hex.Length;
            s = s[..^lenHex] + hex;
            return Guid.Parse(s);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.Delete(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete LookupType", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete LookupType", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> SaveLookupValue(LookupValue data)
        {
            LogStart(_logger, controllerName, "LookupValue");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveLookupValue");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = data.Id });
            }
           
            try
            {
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Lookup Value", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Lookup Value", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = data.LookupTypeId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLookupValue(Guid id, Guid lookupTypeId)
        {
            try
            {
                await _repository.DeleteLookupValue(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete LookupValue", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete LookupValue", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Detail", new { id = lookupTypeId });
        }

        [HttpPost]
        public async Task<IActionResult> SaveLocation(IssueLocation data)
        {
            LogStart(_logger, controllerName, "SaveLocation");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveLocation");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("IssueLocations", new { country = data.CountryRegion });
            }
            try
            {
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Issue Location", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Issue Location", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("IssueLocations", new { CountryRegion = data.CountryRegion });
        }

        [HttpGet]
        public async Task<IActionResult> IssueTypes()
        {
            var userName = TheUser.FullName;
            IssueTypeViewModel model;
            model = new IssueTypeViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = await _repository.IssueTypes()
            };
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveIssueType(IssueType data)
        {
            if (string.IsNullOrEmpty(data.NameLocal))
                data.NameLocal = string.Empty;
            LogStart(_logger, controllerName, "SaveIssueType");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveIssueType");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("SaveIssueTypes");
            }
            try
            {
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Issue Type", ex, data);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Issue Type", ex, data);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("IssueTypes");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteIssueType(Guid id)
        {
            LogStart(_logger, controllerName, "DeleteIssueType");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteIssueType");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("IssueTypes");
            }
            try
            {
                await _repository.DeleteIssueType(id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete Issue Type", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete Issue Type", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("IssueTypes");
        }
    }
}
