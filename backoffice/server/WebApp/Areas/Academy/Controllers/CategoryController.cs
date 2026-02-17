using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Academy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Academy.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Academy.Controllers
{
    [Area("Academy")]
    public class CategoryController : BaseController
    {
        private readonly ILogger<CategoryController> _logger;

        private readonly CategoryRepository _categoryRepository;
        public CategoryController(ILogger<CategoryController> logger,
            UserManager<User> userManager,
            CategoryRepository categoryRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new CategoryViewModel
            {
                FullName = userName,
                CurrentPage = "Category",
                CurrentOperation = "List",
                TheList = _categoryRepository.Categories(TheUser.Id).ToList(),

            };
            model.ChangeActive(MenuItemEnum.AcademyTopicCategory);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new CategoryViewModel
            {
                FullName = userName,
                CurrentPage = "Category",
                CurrentOperation = "Detail",
                TheList = new List<Category>(),
                TheModel = await _categoryRepository.CategoryById(id, TheUser.Id),

            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }

            model.ChangeActive(MenuItemEnum.AcademyTopicCategory);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userName = TheUser.FullName;

            var model = new CategoryViewModel
            {
                FullName = userName,
                CurrentPage = "Categorizes",
                TheList = _categoryRepository.Categories(TheUser.Id).ToList(),
                TheModel = (await _categoryRepository.CategoryById(id, TheUser.Id)) ?? new Category(),

            };
            model.ChangeActive(MenuItemEnum.AcademyTopicCategory);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryInput theModel)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            try
            {
                await _categoryRepository.Create(new GenericInput<CreateCategoryInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "CreateCategory", theModel);
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryInput theModel)
        {
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Academy);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = theModel.Id });
            }
            try
            {
                var payload = await _categoryRepository.Update(new GenericInput<UpdateCategoryInput>(theModel, TheUser.Id));
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "CreateCategory", theModel);
            }

            return RedirectToAction("Detail", new { id = theModel.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            LogStart(_logger, controllerName, "DeleteCategory");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.User)
            {
                LogSecurityError(_logger, controllerName, "DeleteCategory");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Index");
            }
            var category = (await _categoryRepository.CategoryById(id, TheUser.Id));
            if (category != null)
            {
                await _categoryRepository.Delete(category);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            else
            {
                TempData["CommandStatus"] = "Invalid Category";
            }
            return RedirectToAction("Index");
        }
    }
}
