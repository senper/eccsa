using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class InfoController : BaseController
    {
        private readonly ILogger<InfoController> _logger;
        private readonly ExternalDataRepository _repository;
        private readonly AssociationSettingRepository _associationSettingRepository;
        private readonly AssociationRepository _associationRepository;
        public InfoController(ILogger<InfoController> logger,
            UserManager<User> userManager,
            ExternalDataRepository repository,
            PublicUserRepository userRepository,
        AssociationSettingRepository associationSettingRepository,
        AssociationRepository associationRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            _associationSettingRepository = associationSettingRepository;
            _associationRepository = associationRepository;
        }

        public async Task<IActionResult> Index(Guid infoTypeId
            , DateTime startDate

            , int page = 1
            , int rows = 100)
        {
            var userName = TheUser.FullName;
            InfoViewModel model;
            model = new InfoViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheData = new DataTable(),
                InfoTypeId = infoTypeId,

            };

            model.ExternalDataType = await _repository.ExternalDataTypeById(infoTypeId);
            var period = model.ExternalDataType.DataPeriods.FirstOrDefault(p => p.StartDate == startDate);
            DateTime endDate = DateTime.Today;
            if (period != null)
                endDate = period.EndDate;
            model.Title = $"Import {model.ExternalDataType.Caption} ";
            model.TheData = _repository.GetData(model.ExternalDataType.SchemaName, model.ExternalDataType.SourceName, startDate, endDate, page - 1, rows);
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> UploadExcel(
            Guid id
            , DateTime startDate
            , DateTime endDate
            , IFormFile formFile)
        {
            if (endDate < startDate)
            {
                var stDate = startDate.Date;
                startDate = endDate.Date;
                endDate = stDate;
            }

            try
            {
                var externalDataType = await _repository.ExternalDataTypeById(id);

                if (externalDataType.PeriodExists(startDate, endDate))
                {
                    return BadRequest("Period already exists");
                }
                var data = ImportExcel(formFile, externalDataType, startDate, endDate);
                externalDataType.DataPeriods.Add(new Server.Model.ExternalData.Period() { StartDate = startDate, EndDate = endDate });
                await _repository.BulkInsert(externalDataType, data);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("UploadExchange", ex);
                return BadRequest("Unkown error");
            }
            return RedirectToAction("Index", new { infoTypeId = id });
        }


        [HttpPost]
        public async Task<ActionResult> DownloadData(
            Guid id
            , DateTime startDate
            , DateTime endDate
            , IFormFile formFile)
        {
            if (endDate < startDate)
            {
                var stDate = startDate.Date;
                startDate = endDate.Date;
                endDate = stDate;
            }

            try
            {
                var externalDataType = await _repository.ExternalDataTypeById(id);

                if (externalDataType.PeriodExists(startDate, endDate))
                {
                    return BadRequest("Period already exists");
                }
                var data = ImportExcel(formFile, externalDataType, startDate, endDate);
                externalDataType.DataPeriods.Add(new Server.Model.ExternalData.Period() { StartDate = startDate, EndDate = endDate });
                await _repository.BulkInsert(externalDataType, data);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("UploadExchange", ex);
                return BadRequest("Unkown error");
            }
            return RedirectToAction("Index", new { infoTypeId = id });
        }

        [HttpGet]
        public async Task<ActionResult> BankAccountApprovalNeeds()
        {
            var userName = TheUser.FullName;

            var model = new BankAccountApprovalNeedsViewModel
            {
                FullName = userName,
                CurrentPage = "Bank Account Changes",
                CurrentOperation = "List",
            };
            try
            {
                model.TheList = await _associationSettingRepository.AssociationSettingsNeedsApproval(TheUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> BankAccountApprove(Guid id)
        {
            try
            {
                await _associationSettingRepository.ApproveSetting(id, TheUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
            }
            return RedirectToAction("BankAccountApprovalNeeds");
        }
        [HttpGet]
        public async Task<ActionResult> FeeApprovalNeeds()
        {
            var userName = TheUser.FullName;

            var model = new FeeApprovalNeedsViewModel
            {
                FullName = userName,
                CurrentPage = "Association Fee Changes",
                CurrentOperation = "List",
            };
            try
            {
                model.TheList = await _associationRepository.AssociationFeeNeedsApproval(TheUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> FeeApprove(Guid id)
        {
            try
            {
                await _associationRepository.ApproveAssociationFee(id, TheUser.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
            }
            return RedirectToAction("FeeApprovalNeeds");
        }
    }
}
