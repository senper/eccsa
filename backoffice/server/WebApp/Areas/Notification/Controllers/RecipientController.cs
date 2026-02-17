using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Notification.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace ECCSA.BackOffice.Server.WebApp.Areas.Notification.Controllers
{
    [Area("Notification")]
    public class RecipientController : BaseController
    {
        private readonly ILogger<RecipientController> _logger;
        private readonly NotificationRepository _repository;
        private readonly YesNoRepository _lookupRepository;
        public RecipientController(ILogger<RecipientController> logger, UserManager<User> userManager, NotificationRepository repository, YesNoRepository lookupRepository, PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _lookupRepository = lookupRepository;
            controllerName = "Recipient";
        }

        private static string GetCurrentPage(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Academy => "Chamber Academy Recipients",
                ECCSAModuleFlag.Arbitration => "Arbitration Recipients",
                ECCSAModuleFlag.Communication => "Communication Recipients",
                ECCSAModuleFlag.Investment => "Trade Promotion Recipients",
                ECCSAModuleFlag.Membership => "Membership Recipients",
                ECCSAModuleFlag.Reporting => "Reporting Recipients",
                ECCSAModuleFlag.Research => "Research & Advocacy Recipients",
                ECCSAModuleFlag.TradePromotion => "Trade Facilitation Recipients",
                ECCSAModuleFlag.Administration => "Administration",
                _ => string.Empty,
            };
        }
        [HttpGet]
        public async Task<IActionResult> Index(ECCSAModuleFlag module, StakeholderTypeEnum stakeholderType = StakeholderTypeEnum.Association)
        {
            var userName = TheUser.FullName;
            RecipientViewModel model;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }


            model = new RecipientViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheModel = new RecipientSource()
                {
                    Id = Guid.Empty,
                },
                Owner = module
            };
            model.TheList = await _repository.Recipients(null, Guid.Empty, stakeholderType);
            var lst = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId);
            model.Sectors = lst.LookupValues.ToList();
            foreach (var r in model.TheList)
            {
                if (!r.SectorId.HasValue) continue;
                r.Sector = model.Sectors.FirstOrDefault(l => l.Id == r.SectorId.Value).Name;
            }
            model.StakeholderType = stakeholderType;
            model.CurrentModule = module;
            model.ChangeActive(MenuItemEnum.RecipientSource);
            TempData["title"] = GetCurrentPage(module);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(RecipientSource vmodel)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(vmodel.Owner);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index", new { module = vmodel.Owner });
            }
            if (vmodel.StakeholderType == StakeholderTypeEnum.Association ||
                vmodel.StakeholderType == StakeholderTypeEnum.Authenticated ||
                vmodel.StakeholderType == StakeholderTypeEnum.Individual ||
                vmodel.StakeholderType == StakeholderTypeEnum.Public)
            {
                vmodel.SectorId = vmodel.SectorId;
            }
            else
            {
                vmodel.SectorId = null;
            }
            try
            {
                await _repository.Save(vmodel, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Save Recipient", ex, vmodel);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save Recipient", ex, vmodel);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (vmodel.Id == Guid.Empty)
                return RedirectToAction("Index", new { module = vmodel.Owner });
            else
                return RedirectToAction("Detail", new { id = vmodel.Id });
        }
        [HttpPost]
        public async Task<IActionResult> ImportAssociationContacts(ECCSAModuleFlag module)
        {
            if (module != ECCSAModuleFlag.Membership)
            {
                TempData["CommandStatus"] = "Import Association Contacts if only available for Membership !!";
                return RedirectToAction("Index", new { module });
            }
            try
            {
                await _repository.ImportAssociations();
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Import Association Contacts", ex);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Import Association Contacts", ex);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module, stakeholderType = StakeholderTypeEnum.Association });
        }
        [HttpPost]
        public async Task<IActionResult> ImportMemberContacts(ECCSAModuleFlag module)
        {
            if (module != ECCSAModuleFlag.Membership)
            {
                TempData["CommandStatus"] = "Import Member Contacts if only available for Membership !!";
                return RedirectToAction("Index", new { module });
            }
            try
            {
                await _repository.ImportMembers();
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Import Member Contacts", ex);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Import Member Contacts", ex);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module, stakeholderType = StakeholderTypeEnum.Individual });
        }
        [HttpPost]
        public async Task<IActionResult> ImportStaffContacts(ECCSAModuleFlag module)
        {
        //    if (module != ECCSAModuleFlag.Membership)
        //    {
        //        TempData["CommandStatus"] = "Import Staff Contacts is only available for Membership !!";
        //        return RedirectToAction("Index", new { module, stakeholderType = StakeholderTypeEnum.Management });
        //    }
            try
            {
                await _repository.ImportEccsaStaff(module);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Import Staff Contacts", ex);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Import Staff Contacts", ex);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module, stakeholderType = StakeholderTypeEnum.Management });
        }

        [HttpPost]
        public async Task<IActionResult> ImportPublicContacts(ECCSAModuleFlag module)
        {
            if (module != ECCSAModuleFlag.Membership)
            {
                TempData["CommandStatus"] = "Import Staff Contacts if only available for Membership !!";
                return RedirectToAction("Index", new { module });
            }
            try
            {
                await _repository.ImportPublicContacts(module);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Import Staff Contacts", ex);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Import Staff Contacts", ex);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module, stakeholderType = StakeholderTypeEnum.Public });
        }
        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var userName = TheUser.FullName;
            RecipientViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new RecipientViewModel
            {
                FullName = userName,
                CurrentOperation = "Detail",
                TheList = new List<RecipientSource>(),
                TheModel = await _repository.RecipientById(id)

            };

            var roleLevel = await GetRoleLevel(model.TheModel.Owner);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var lst = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId);
            model.Sectors = lst.LookupValues.ToList();
            model.TheModel.Sector = model.TheModel.SectorId != null ?
                model.Sectors.FirstOrDefault(s => s.Id == model.TheModel.SectorId)?.Name ?? "" : "";
            model.CurrentModule = model.TheModel.Owner;
            model.ChangeActive(MenuItemEnum.RecipientSource);
            model.CurrentPage = GetCurrentPage(model.CurrentModule);
            TempData["title"] = model.CurrentPage;
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, ECCSAModuleFlag module)
        {
            RecipientSource request = new()
            {
                Id = id,
                CreatedBy = TheUser.Id
            };
            try
            {
                await _repository.Delete(request);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Delete Recipient", ex, request);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete Recipient", ex, request);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Index", new { module });
        }
        [HttpGet]
        public async Task<IActionResult> ImportContacts(ECCSAModuleFlag module)
        {
            var userName = TheUser.FullName;
            RecipientViewModel model;
            LogStart(_logger, controllerName, "ImportContacts");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            model = new RecipientViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheModel = new RecipientSource()
                {
                    Id = Guid.Empty,
                },
                Owner = module
            };
            model.TempList = await _repository.GetTempRecipient(module);
            var lst = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId);
            model.Sectors = lst.LookupValues.ToList();
            foreach (var r in model.TempList)
            {
                if (!r.SectorId.HasValue) continue;
                r.Sector = model.Sectors.FirstOrDefault(l => l.Id == r.SectorId.Value).Name;
            }
            model.CurrentModule = module;
            model.ChangeActive(MenuItemEnum.RecipientSource);
            TempData["title"] = GetCurrentPage(module);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "ImportContacts");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ImportFile(ECCSAModuleFlag module, IFormFile file)
        {
            var userName = TheUser.FullName;
            RecipientViewModel model;
            LogStart(_logger, controllerName, "ImportContacts");
            var roleLevel = await GetRoleLevel(module);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            model = new RecipientViewModel
            {
                FullName = userName,
                CurrentPage = GetCurrentPage(module),
                CurrentOperation = "List",
                TheModel = new RecipientSource()
                {
                    Id = Guid.Empty,
                },
                Owner = module
            };
            model.TheList = new List<RecipientSource>();
            var lst = await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId);
            model.Sectors = lst.LookupValues.ToList();
            model.CurrentModule = module;
            try
            {
                model.TempList = ImportExcel<TempRecipientSource>(file, new List<ImportTemplate>
                {
                    new ImportTemplate{ Name = "" },
                    new ImportTemplate{ Name = "CompanyName", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "Name", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "Position", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "ImportSectorName", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "ImportSubsectorName", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "BusinessInterest", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "OfficeTel", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "MobileNo", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "Email", ColumnType = DataTypeEnum.String },
                    new ImportTemplate{ Name = "Website", ColumnType = DataTypeEnum.String },

                });
                var sectorMaps = await _repository.GetSectorMaps();
                foreach (var item in model.TempList)
                {
                    item.Owner = module;
                    if (model.Sectors.Any(s => s.Name.ToLower() == item.ImportSubsectorName.ToLower()))
                    {
                        var sector = model.Sectors.FirstOrDefault(s => s.Name.ToLower() == item.ImportSubsectorName.ToLower());
                        item.SectorId = sector.Id;
                        item.Sector = sector.Name;
                    }
                    else if (item.ImportSubsectorName.Trim() != string.Empty && sectorMaps.Any(m => m.ImportSectorName.ToLower() == item.ImportSubsectorName.ToLower()))
                    {
                        var sector = sectorMaps.FirstOrDefault(s => s.ImportSectorName.ToLower() == item.ImportSubsectorName.ToLower());
                        item.SectorId = sector.SectorId;
                        var sectorll = model.Sectors.FirstOrDefault(s => s.Id == sector.SectorId);
                        item.Sector = sectorll.Name;
                    }
                }
                await _repository.SaveTempReceipients(model.TempList, TheUser.Id);
            }
            catch (InvalidDataException ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("Import Company Contacts from file", ex);
                TempData["CommandStatus"] = ex.Message;
            }
            model.ChangeActive(MenuItemEnum.RecipientSource);
            TempData["title"] = GetCurrentPage(module);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "ImportContacts");
            return View("ImportContacts", model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveImport(ECCSAModuleFlag module, IFormCollection formCollection)
        {
            try
            {
                var list = await _repository.Recipients(module, Guid.Empty, StakeholderTypeEnum.Individual);
                var tempList = await _repository.GetTempRecipient(module);
                var listofMap = new List<ImportRecipientSectorMap>();
                var recipientList = new List<RecipientSource>();
                foreach (var item in tempList)
                {
                    if (item.SectorId == null && formCollection[item.Id.ToString()] != string.Empty &&
                        Guid.TryParse(formCollection[item.Id.ToString()], out Guid sectorId))
                    {
                        item.SectorId = sectorId;
                        listofMap.Add(new ImportRecipientSectorMap { SectorId = sectorId, ImportSectorName = item.ImportSubsectorName });
                    }

                    var id = list.FirstOrDefault(s => (!string.IsNullOrWhiteSpace(item.Email) && s.Email == item.Email) || (!string.IsNullOrWhiteSpace(item.MobileNo) && s.MobileNo == item.MobileNo))?.Id ??
                     tempList.FirstOrDefault(s => (!string.IsNullOrWhiteSpace(item.Email) && s.Email == item.Email) || (!string.IsNullOrWhiteSpace(item.MobileNo) && s.MobileNo == item.MobileNo))?.Id ?? Guid.NewGuid();

                    recipientList.Add(new RecipientSource
                    {
                        Id = id,
                        Name = string.IsNullOrWhiteSpace(item.Name) && !string.IsNullOrWhiteSpace(item.Email) ? item.Email : item.Name,
                        CompanyName = item.CompanyName,
                        MobileNo = item.MobileNo,
                        Email = item.Email,
                        Owner = module,
                        BusinessInterest = item.BusinessInterest,
                        Position = item.Position,
                        StakeholderType = StakeholderTypeEnum.Individual,
                        SectorId = item.SectorId,
                        ImportSectorName = item.ImportSectorName,
                        ImportSubsectorName = item.ImportSubsectorName,
                        Website = item.Website,
                        OfficeTel = item.OfficeTel,
                        CreatedBy = TheUser.Id,
                        CreateStamp = DateTime.UtcNow,
                        UpdatedBy = TheUser.Id,
                        UpdateStamp = DateTime.UtcNow
                    });
                }

                await _repository.SaveSectorMap(listofMap, TheUser.Id);
                await _repository.Save(recipientList, TheUser.Id);
                await _repository.SaveTempReceipients(new List<TempRecipientSource>(), TheUser.Id);

                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("Import Company Contacts from file", ex);
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("ImportContacts", new { module });
        }
        [HttpGet]
        public async Task<FileResult> DownLoadImportTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot", "membership");
            return await DownLoadFile(Path.Combine(filePath, "Companiesaddtemplet.xls"), "Company Address Import Template.xls");
        }
    }
}
