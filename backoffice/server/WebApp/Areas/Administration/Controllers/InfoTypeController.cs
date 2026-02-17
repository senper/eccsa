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
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.ExternalData;
using ClosedXML.Excel;
using System.IO;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class InfoTypeController : BaseController
    {
        private readonly ILogger<InfoTypeController> _logger;
        private readonly ExternalDataRepository _repository;
        public InfoTypeController(ILogger<InfoTypeController> logger,
            UserManager<User> userManager,
            ExternalDataRepository repository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
        }

       
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Data Source Types ");
            var userName = TheUser.FullName;
            InfoTypeViewModel model;
            if (!TheUser.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
            {
                model = new InfoTypeViewModel
                {
                    FullName = userName,
                    CurrentOperation = "List",
                    TheList = new List<ExternalDataType>(),
                    TheModel = new ExternalDataType()
                    {
                        Id = Guid.Empty,
                       

                    }

                };
                return View(model);
            }
            model = new InfoTypeViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = await _repository.ExternalDataTypes(),
                TheModel = new ExternalDataType()
                {
                    Id = Guid.Empty,
                }

            };

           
            TempData["title"] = model.CurrentPage;
            return View(model);
        }


        public async Task<IActionResult> Detail(Guid id)
        {
            _logger.LogInformation($"Detail Data Source Type {id}");
            var userName = TheUser.FullName;
            InfoTypeViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new InfoTypeViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = new List<ExternalDataType>(),
                TheModel = await _repository.ExternalDataTypeById(id)

            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            TempData["title"] = model.CurrentPage;
            return View(model);
        }

       

        [HttpGet]
        public async Task<FileResult> DownloadTemplate(Guid id)
        {
            var dataType = await _repository.ExternalDataTypeById(id);
           
            var workbook = new XLWorkbook();
            IXLWorksheet sheet = workbook.Worksheets.Add("Sheet 1");
            var i = 1;
            foreach (var column in dataType.MetaData)
            {
                if (string.IsNullOrEmpty(column.ExcelHeaderName)) continue;
                sheet.Cell(1, i).Value = column.ExcelHeaderName;
                i++;
            }
           

            using MemoryStream stream = new();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{dataType.Caption}.xlsx");
        }
    }
}
