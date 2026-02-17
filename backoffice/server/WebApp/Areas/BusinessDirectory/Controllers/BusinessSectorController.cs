using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.BusinessDirectory.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.BusinessDirectory.Controllers
{
    [Area("BusinessDirectory")]
    public class BusinessSectorController : BaseController
    {
        private readonly ILogger<BusinessSectorController> _logger;
        private readonly BusinessRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public BusinessSectorController(
            UserManager<User> userManager,
             IWebHostEnvironment hostingEnvironment
            , ILogger<BusinessSectorController> logger
            , BusinessRepository repository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "BusinessSector";
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var model = new BusinessSectorViewModel();
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.RootBusinessSectors = await _repository.RootSectors();
                model.Name = String.Empty;
                if (id == Guid.Empty)
                    model.TheList = model.RootBusinessSectors;
                else
                {
                    model.Name = model.RootBusinessSectors.Find(b => b.Id == id).Name;
                    model.TheList = await _repository.ChildSectors(id);
                }
                model.SectorId = id;
            }
            catch (Exception ex)
            {
                model.TheList = new List<BusinessSector>();
                _logger.LogError(ex, "Business Sector Exception");
            }
            model.CurrentPage = "Business Directory";
            model.CurrentOperation = "Index";
            model.RoleLevel = await GetRoleLevel(Server.Model.Common.ECCSAModuleFlag.Membership);
            model.ChangeActive(MenuItemEnum.BusinessSector);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id, AutoMapBy mapBy = AutoMapBy.Mobile)
        {
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var model = new BusinessSectorViewModel();
            model.Name = String.Empty;
            if (id == Guid.Empty)
                return RedirectToAction("Index");
            var companies = new List<Company>();
            try
            {
                model.RootBusinessSectors = await _repository.RootSectors();
                companies = await _repository.Companies();
                model.TheModel = await _repository.SectorById(id);
                if (model.TheModel == null)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Business Sector Exception {id}");
            }

            model.SectorId = id; // model.TheModel.ParentId.Value;
            model.Name = model.RootBusinessSectors.Find(r => r.Id == model.TheModel.ParentId.Value).Name;
            model.TempCompanies = model.TheModel.TempCompanies.ToList();

            foreach (var tmp in model.TempCompanies)
            {
                List<Company> lst = new();
                switch (mapBy)
                {
                    case AutoMapBy.Mobile:
                        if (tmp.Mobile.Length > 1)
                        {
                            lst = companies.Where(t => t.Mobile.Contains(tmp.Mobile)).ToList();
                        }
                        break;
                    case AutoMapBy.Telephone:
                        if (tmp.Telephone.Length > 1)
                        {
                            lst = companies.Where(t => t.Mobile.Contains(tmp.Telephone)).ToList();
                        }
                        break;
                }


                tmp.TempList = lst;
            }
            model.CurrentPage = "Business Directory";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.BusinessSector);
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }

        public async Task<IActionResult> CompanyDetail(Guid id, Guid? sectorid)
        {
            var model = new CompanyViewModel();
            LogStart(_logger, controllerName, "CompanyDetail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            if (id == Guid.Empty)
                return RedirectToAction("Index");
            model.Name = String.Empty;
            try
            {

                model.BusinessSectors = await _repository.RootSectors();
                model.TheModel = await _repository.Company(id);
                model.SectorId = sectorid.HasValue ? sectorid.Value : Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Business Sector Company Exception {id}");
            }

            var tmp = model.BusinessSectors.OrderBy(x => x.Name).ToList();
            var glist = new List<SelectListGroup>();
            model.SelectSource = new List<SelectListItem>();
            foreach (var t in tmp)
            {
                var gItem = new SelectListGroup();
                gItem.Name = t.Name;
                foreach (var i in t.SubSectors)
                {
                    SelectListItem item = new SelectListItem(i.Name, i.Id.ToString());
                    item.Group = gItem;
                    model.SelectSource.Add(item);
                }


            }
            if (!string.IsNullOrEmpty(model.TheModel.Extension))
            {
                var fileName = Guid2String(model.TheModel.Id) + model.TheModel.Extension;
                model.LogoPath = Path.Combine(DirectoryRelativePath, fileName);
            }

            model.CurrentPage = "Business Directory";
            model.CurrentOperation = "CompanyDetail";
            model.ChangeActive(MenuItemEnum.BusinessSector);
            LogEnd(_logger, controllerName, "CompanyDetail");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ImportCompanies(Guid sectorId, IFormFile file)
        {
            LogStart(_logger, controllerName, "ImportCompanies");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "ImportCompanies");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = sectorId });
            }
            List<ImportTemplate> columns;
            columns = new List<ImportTemplate>
            {
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Name"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Telephone"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Mobile"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Fax"
                },
                 new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Email"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "PoBox"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Web"
                }
            };
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file format");
            }

            var message = string.Empty;
            var hasError = false;
            var lst = new List<CompanyImportModel>();
            try
            {
                lst = ImportExcel<CompanyImportModel>(file, columns);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                hasError = true;
            }
            if (lst.Count > 0)
                await _repository.ClearTempList(sectorId);
            foreach (var item in lst)
            {
                var m = new TempCompany
                {
                    Id = item.Id,
                    Name = item.Name,
                    Telephone = item.Telephone,
                    Mobile = item.Mobile,
                    Email = item.Email,
                    Fax = item.Fax,
                    PoBox = item.PoBox,
                    Web = item.Web,
                    SectorId = sectorId,
                    Extension = String.Empty

                };


                try
                {
                    await _repository.Save(m, TheUser.Id);
                }
                catch (Exception ex)
                {
                    hasError = true;
                    _logger.LogError("ImportCompanies", ex, item);
                    message += ex.Message;
                }
            }
            if (!hasError)
            {
                CommandStatus(CommandStatusEnum.Saved);
            }
            else
            {
                TempData["CommandStatus"] = message;
            }
            return RedirectToAction("Detail", new { id = sectorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveTempCompany(IFormCollection collection)
        {
            LogStart(_logger, controllerName, "MoveTempCompany");
            var sectorId = Guid.Parse(collection["sectorId"]);
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "MoveTempCompany");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = sectorId });
            }
            var model = new BusinessSectorViewModel();
            var keys = collection.Keys;

            var chBoxStarter = "CK";
            var selStarter = "S";
            var hasError = false;
            var message = string.Empty;
            var tempList = await _repository.GetTempList(sectorId);
            foreach (var tmp in tempList)
            {
                var idS = model.Guid2String(tmp.Id);
                var key = $"{chBoxStarter}{idS}";
                if (!keys.Contains(key)) continue;
                var sel = $"{selStarter}{idS}";
                var newId = tmp.Id;
                if (keys.Contains(sel))
                {
                    var xx = Guid.Parse(collection[sel]);
                    if (xx != Guid.Empty)
                        newId = xx;
                }
                try
                {
                    await _repository.MoveTempList(sectorId, tmp.Id, newId);
                }
                catch (Exception ex)
                {
                    hasError = true;
                    _logger.LogError("MoveTempCompany", ex, tmp);
                    message += ex.Message;
                }
            }
            if (!hasError)
            {
                CommandStatus(CommandStatusEnum.Saved);
            }
            else
            {
                TempData["CommandStatus"] = message;
            }
            return RedirectToAction("Detail", new { id = sectorId });
        }

        //public async Task<IActionResult> Import(Guid id, AutoMapBy mapBy = AutoMapBy.Mobile)
        //{
        //    var model = new BusinessSectorViewModel();
        //    model.RootBusinessSectors = await _repository.RootSectors();
        //    model.Name = String.Empty;
        //    if (id == Guid.Empty)
        //        return RedirectToAction("Index");

        //    model.TheModel = await _repository.SectorById(id);

        //    model.SectorId = model.TheModel.ParentId.Value;
        //    model.Name = model.RootBusinessSectors.Find(r => r.Id == model.SectorId).Name;
        //    model.TempCompanies = model.TheModel.TempCompanies.ToList();
        //    var companies = await _repository.Companies();
        //    foreach (var tmp in model.TempCompanies)
        //    {
        //        var lst = mapBy == AutoMapBy.Mobile ?
        //                companies.Where(t => t.Mobile.Contains(tmp.Mobile)).ToList() :
        //                companies.Where(t => t.Telephone.Contains(tmp.Telephone)).ToList();
        //        tmp.TempList = lst;
        //    }
        //    model.SectorId = id;
        //    return View(model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCompany(CompanyImportModel model, Guid sectorId, IFormFile logo)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveCompany");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = sectorId });
            }
            try
            {
                var m = model.ToCompany();
                if (m.Id == Guid.Empty)
                {
                    m.Id = Guid.NewGuid();
                    m.BusinessSectors = new List<BusinessSectorCompany>();
                    m.BusinessSectors.Add(new BusinessSectorCompany()
                    {
                        CompanyId = m.Id,
                        SectorId = sectorId

                    });
                }
                if (logo != null && logo.Length > 0)
                {
                    var lindex = logo.FileName.LastIndexOf(".");
                    if (lindex > 0)
                    {
                        var ext = logo.FileName.Substring(lindex);
                        var fileName = $"{Guid2String(m.Id)}{ext}";
                        var tmp = await SaveFile(DirectoryAbsolutePath, fileName, logo);
                        m.Extension = ext;
                    }
                }


                await _repository.Save(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveCompany", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (sectorId != Guid.Empty && model.Id != Guid.Empty)
            {
                return RedirectToAction("CompanyDetail", new { id = model.Id, sectorid = sectorId });
            }
            return RedirectToAction("Detail", new { id = sectorId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSector(BusinessSector model)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveSector");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index", new { id = model.ParentId.HasValue ? model.ParentId.Value : Guid.Empty });
            }
            try
            {
                await _repository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index", new { id = model.ParentId.HasValue ? model.ParentId.Value : Guid.Empty });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMembership(Guid companyId, Guid sectorId, bool fromCompany)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteMembership");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = sectorId });
            }
            try
            {
                await _repository.DeleteCompanySector(companyId, sectorId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, new { companyId, sectorId });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (fromCompany)
            {
                return RedirectToAction("CompanyDetail", new { id = companyId });
            }
            return RedirectToAction("Detail", new { id = sectorId });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMembership(Guid companyId, Guid sectorId)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "AddMembership");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = sectorId });
            }
            try
            {
                var data = new BusinessSectorCompany() { Id = Guid.NewGuid(), SectorId = sectorId, CompanyId = companyId };
                await _repository.Save(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, new { companyId, sectorId });
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("CompanyDetail", new { id = companyId });


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSector(Guid sectorId)
        {
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteSector");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index", new { id = Guid.Empty });
            }
            try
            {
                await _repository.DeleteSector(sectorId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, sectorId);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("Index", new { id = Guid.Empty });

        }

        [HttpGet]
        public async Task<FileResult> DownLoadCompanyImportTemplate()
        {
            var fileName = "Company Import Template.xlsx";
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "ReportTemplates", fileName);
            return await DownLoadFile(path, fileName);
        }
    }
}
