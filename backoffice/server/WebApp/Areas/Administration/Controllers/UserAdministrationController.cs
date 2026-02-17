using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration;
using ECCSA.BackOffice.Server.WebApp.Areas.Administration.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class UserAdministrationController : BaseController
    {
        private readonly ILogger<UserAdministrationController> _logger;
        private readonly UserAdministrationRepository _repository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly YesNoRepository _lookupRepository;

        private readonly List<Role> MembershipRoles = new List<Role>()
        {
            new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                    Name = "Membership Admin",
                    NormalizedName = "MEA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Membership
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-00000000000a"),
                    Name = "Membership User",
                    NormalizedName = "MEU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Membership
                }
        };
        public UserAdministrationController(ILogger<UserAdministrationController> logger,
            UserManager<User> userManager,
            UserAdministrationRepository repository,
             IWebHostEnvironment hostingEnvironment,
            PublicUserRepository userRepository,
        YesNoRepository lookupRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            controllerName = "UserAdministration";
            _lookupRepository = lookupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            LogStart(_logger, controllerName, "Index");
            var userName = TheUser.FullName;
            UserAdministrationViewModel model;

            if (!TheUser.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
            {
                _logger.LogError("UserAdministration",
                                 "Index",
                                 "Insufficient Privilege",
                                 TheUser,
                                 DateTime.UtcNow);
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var supportingUnits = (await _lookupRepository.LookupTypeById(new Guid("424063b8-7cef-4ff0-8a13-c88334a100a5")))?.LookupValues.ToList()
    ?? new List<LookupValue>();

            model = new UserAdministrationViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = await _repository.BackOfficeUsers(),
                TheModel = new User()
                {
                    Id = Guid.Empty,
                },
                SupportingUnits = supportingUnits.Where(s => s.Value != "0").ToList()
            };
            foreach (var mod in Enum.GetValues<ECCSAModuleFlag>())
            {
                model.AllModules.Add(new Server.Model.Membership.SimpleLookup()
                {
                    Id = ((int)mod),
                    Name = mod.GetDisplayName()
                });

            }
            model.RoleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            TempData["title"] = model.CurrentPage;
            LogEnd(_logger, controllerName, "Index");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Members()
        {
            LogStart(_logger, controllerName, "Members");
            var userName = TheUser.FullName;
            MemberUserAdministrationViewModel model;

            if (!TheUser.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
            {
                _logger.LogError("UserAdministration",
                                 "Members",
                                 "Insufficient Privilege",
                                 TheUser,
                                 DateTime.UtcNow);
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            model = new MemberUserAdministrationViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = await _repository.MemberUsers(),
                Associations = await _repository.Associations(),
                TheModel = new User()
                {
                    Id = Guid.Empty,
                }

            };
            if(!TheUser.IsHeadOfficeUser)
            {
                model.TheList = model.TheList.Where(m => m.AssociationId == TheUser.AssociationId || m.AssociationParentId == TheUser.AssociationId).ToList();
                model.Associations = model.Associations.Where(a => a.Id == TheUser.AssociationId || a.ParentId == TheUser.AssociationId).ToList();
            }
            foreach (var mod in Enum.GetValues<ECCSAModuleFlag>())
            {
                model.AllModules.Add(new Server.Model.Membership.SimpleLookup()
                {
                    Id = ((int)mod),
                    Name = mod.GetDisplayName()
                });

            }
            model.RoleLevel = await GetRoleLevel(ECCSAModuleFlag.Administration);
            TempData["title"] = model.CurrentPage;
            
            LogEnd(_logger, controllerName, "Members");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            LogStart(_logger, controllerName, "Detail");
            var userName = TheUser.FullName;
            UserAdministrationViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            var supportingUnits = (await _lookupRepository.LookupTypeById(new Guid("424063b8-7cef-4ff0-8a13-c88334a100a5")))?.LookupValues.ToList()
    ?? new List<LookupValue>();
            model = new UserAdministrationViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = new List<User>(),
                
                TheModel = await _repository.BackOfficeUser(id),
                UserLoginHistories = await _repository.UserLoginHistory(id),
                SupportingUnits = supportingUnits.Where(s => s.Value != "0").ToList()
            };
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            foreach (var mod in Enum.GetValues<ECCSAModuleFlag>())
            {
                model.AllModules.Add(new Server.Model.Membership.SimpleLookup()
                {
                    Id = ((int)mod),
                    Name = mod.GetDisplayName()
                });
                if (model.TheModel.AssignedModules == 0) continue;
                if (model.TheModel.AssignedModules.HasFlag(mod))
                    model.AssignedModules.Add(mod);
            }
            model.Roles = await _repository.GetRoles();
            TempData["title"] = model.CurrentPage;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> MemberDetail(Guid id)
        {
            LogStart(_logger, controllerName, "MemberDetail");
            var userName = TheUser.FullName;
            MemberUserAdministrationViewModel model;
            if (Guid.Empty == id)
            {
                return NotFound();
            }
            model = new MemberUserAdministrationViewModel
            {
                FullName = userName,
                CurrentOperation = "List",
                TheList = new List<User>(),
                TheModel = await _repository.MemberUser(id),
                Associations = await _repository.Associations(),
                UserLoginHistories = await _repository.UserLoginHistory(id)
            };
            model.AssignedModules.Clear();
            model.AssignedModules.Add(ECCSAModuleFlag.Membership);
            
            
            model.Roles = MembershipRoles;
            var allRoles = await _repository.GetRoles();
            model.Roles.AddRange(allRoles.Where(r => r.Module == ECCSAModuleFlag.Administration).ToList());

            TempData["title"] = model.CurrentPage;
            return View(model);
        }
        [HttpGet]
        public async Task< JsonResult> GetAssociations(Guid? parentId)
        {
            parentId = parentId ?? TheUser.AssociationId;
            var tmp = await _repository.Associations();
            Guid pId;
            if (parentId.HasValue && parentId.Value != Guid.Empty)
            {
                pId = parentId.Value;
            }
            else
            {
                pId = Constants.ECCSA;
            }
            tmp = tmp.Where(l => l.ParentId == pId
                  || l.Id == pId).ToList();

            tmp.Insert(0, new Server.Model.Membership.Association
            {
                Id = Guid.Empty,
                NameEng = string.Empty,
                NameLocal = string.Empty,
               
            });
            var lst = new List<Membership.Model.LookupView>();
            foreach (var t in tmp)
            {
                lst.Add(new Membership.Model.LookupView
                {
                    Id = t.Id,
                    Name = t.Name,
                    ParentId = t.ParentId
                });
            }
            return Json(lst);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            LogStart(_logger, controllerName, "Create");
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            bool succeeded = false;
            if (user.AssociationId == Guid.Empty && user.DefaultModule > 0)
            { user.AssociationId = Constants.ECCSA; }
            user.AssignedModules = user.DefaultModule;
            IEnumerable<IdentityError> errors = null;
            try
            {
                var result = await _userManager.CreateAsync(user, user.Password);

                succeeded = result.Succeeded;
                if (succeeded)
                {
                    user.AssignedModules = user.DefaultModule;
                    await _repository.UpdateUser(user);
                }
                errors = result.Errors;
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "Create", user);
                TempData["CommandStatus"] = ex.Message;
            }
            if (succeeded)
            {
                CommandStatus("User Created Successfully");
                return RedirectToAction("Detail", new { id = user.Id });
            }
            else if (errors != null)
            {
                TempData["CommandStatus"] = errors.Select(a => a.Description).Aggregate((a, b) => a + "; " + b);
            }
            else
            {
                CommandStatus("Cannot create User");
            }
            LogEnd(_logger, controllerName, "Create");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(User user)
        {
            LogStart(_logger, controllerName, "CreateMember");
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            bool succeeded = false;
            if (TheUser.IsHeadOfficeUser && user.AssociationId == Guid.Empty )
            { user.AssociationId = Constants.ECCSA; }
            else if(!TheUser.IsHeadOfficeUser && user.AssociationId == Guid.Empty)
            {
                user.AssociationId = TheUser.AssociationId;
            }
            user.AssignedModules = ECCSAModuleFlag.Membership;
            IEnumerable<IdentityError> errors = null;
            try
            {
                var result = await _userManager.CreateAsync(user, user.Password);

                succeeded = result.Succeeded;
                if (succeeded)
                {
                    user.AssignedModules = ECCSAModuleFlag.Membership;
                    await _repository.ChangeUserSubsystems(user.Id, new List<int>() { (int)ECCSAModuleFlag.Membership});
                    List<Guid> roles = new()
                    {
                        MembershipRoles[0].Id,
                        MembershipRoles[1].Id
                    };
                    await _repository.ChangeUserRole(user.Id, roles);
                    await _repository.UpdateMemberUser(user);
                }
                errors = result.Errors;
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "Create", user);
                TempData["CommandStatus"] = ex.Message;
            }
            if (succeeded)
            {
                CommandStatus("User Created Successfully");
                return RedirectToAction("MemberDetail", new { id = user.Id });
            }
            else if (errors != null)
            {
                TempData["CommandStatus"] = errors.Select(a => a.Description).Aggregate((a, b) => a + "; " + b);
            }
            else
            {
                CommandStatus("Cannot create User");
            }
            LogEnd(_logger, controllerName, "Create");
            return RedirectToAction("Members");
        }
       
        [HttpPost]
        public async Task<IActionResult> SaveUserProfile(User user)
        {
            LogStart(_logger, controllerName, "SaveUserProfile");
            try
            {
                await _repository.UpdateProfile(user);
                CommandStatus("User Profile Updated Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveUserProfile", user);
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "DisableUser");
            return RedirectToAction("Detail", new { id = user.Id });
        }
      
        [HttpPost]
        public async Task<IActionResult> DisableUser(Guid id)
        {
            LogStart(_logger, controllerName, "DisableUser");
            try
            {
                await _repository.DisableUser(id);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "DisableUser", id);
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "DisableUser");
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            LogStart(_logger, controllerName, "RemoveUser");
            try
            {
                await _repository.RemoveUser(id);
                CommandStatus("User removed Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RemoveUser", id);
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "RemoveUser");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> EnableUser(Guid id)
        {
            LogStart(_logger, controllerName, "EnableUser");
            try
            {
                await _repository.EnableUser(id);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "EnableUser", id);
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "EnableUser");
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> AddUserRole(Guid userId, Guid roleId)
        {
            LogStart(_logger, controllerName, "AddUserRole");
            try
            {
                await _repository.AddUserRole(userId, roleId);
                CommandStatus("Role saved Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "AddUserRole", new { userId, roleId });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "AddUserRole");
            return RedirectToAction("Detail", new { id = userId });
        }
      
        [HttpPost]
        public async Task<IActionResult> SaveUserRole(Guid userId, List<Guid> roles, bool Members = false)
        {
            LogStart(_logger, controllerName, "SaveUserRoles");
            try
            {
                await _repository.ChangeUserRole(userId, roles, Members);
                CommandStatus("Roles saved Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ChangeUserRole", new { userId, roles });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "ChangeUserRole");
            return RedirectToAction(!Members ? "Detail" : "MemberDetail", new { id = userId });
        }
       
        [HttpPost]
        public async Task<IActionResult> SaveUserSubsystems(Guid userId, List<int> subsystems)
        {
            LogStart(_logger, controllerName, "SaveUserSubsystems");
            try
            {
                await _repository.ChangeUserSubsystems(userId, subsystems);
                CommandStatus("Subsystems saved Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SaveUserSubsystems", new { userId, subsystems });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "SaveUserSubsystems");
            return RedirectToAction("Detail", new { id = userId });
        }
      
        [HttpPost]
        public async Task RenameUserName(Guid userId, string userName)
        {
            LogStart(_logger, controllerName, "RemoveUserRole");
            try
            {
                await _repository.RenameUserName(userId, userName);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RemoveUserRole", new { userId, userName });
            }
            LogEnd(_logger, controllerName, "RemoveUserRole");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserRole(Guid userId, Guid roleId)
        {
            LogStart(_logger, controllerName, "RemoveUserRole");
            try
            {
                await _repository.RemoveUserRole(userId, roleId);
                CommandStatus("Role removed Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RemoveUserRole", new { userId, roleId });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "RemoveUserRole");
            return RedirectToAction("Detail", new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserSubsystem(Guid userId, int subsystem)
        {
            LogStart(_logger, controllerName, "RemoveUserSubsystem");
            try
            {
                await _repository.RemoveUserSubsystem(userId, subsystem);
                CommandStatus("Subsystem removed Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "RemoveUserSubsystem", new { userId, subsystem });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "RemoveUserSubsystem");
            return RedirectToAction("Detail", new { id = userId });
        }
        [HttpPost]
        public async Task<IActionResult> SetDefaultSubsystem(Guid userId, int subsystem)
        {
            LogStart(_logger, controllerName, "SetDefaultSubsystem");
            try
            {
                await _repository.SetDefaultSubsystem(userId, subsystem);
                CommandStatus("Default subsystem set Successfully");
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "SetDefaultSubsystem", new { userId, subsystem });
                TempData["CommandStatus"] = ex.Message;
            }
            LogEnd(_logger, controllerName, "SetDefaultSubsystem");
            return RedirectToAction("Detail", new { id = userId });
        }
        [HttpPost]
        public async Task ChangeSystemAccess(Guid userId, ECCSAModuleFlag assignedModules, ECCSAModuleFlag defaultModule)
        {
            LogStart(_logger, controllerName, "ChangeSystemAccess");
            try
            {
                await _repository.UpdateUserModuleAccess(userId, assignedModules, defaultModule);
            }
            catch (Exception ex)
            {
                LogError(_logger, ex, controllerName, "ChangeSystemAccess", new { userId, assignedModules, defaultModule });
            }
            LogEnd(_logger, controllerName, "ChangeSystemAccess");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordChange(ChangePasswordModel model, Guid id, ECCSAModuleFlag module)
        {
            var errorMsg = string.Empty;
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["CommandStatus"] = "User not found.";
                return RedirectToAction("Detail", new { id });
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset.", user);
                TempData["CommandStatus"] = "Password reset Successfully";
                return RedirectToAction("Detail", new { id });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }

            TempData["CommandStatus"] = errorMsg;
            return RedirectToAction("Detail", new { id });
        }
        [HttpGet]
        public IActionResult ChangePassword(ECCSAModuleFlag module)
        {
            var model = new Models.BaseViewModel
            {
                CurrentModule = module,
                FullName = TheUser.FullName
            };
            return View(model);
        }

        [HttpGet]
        public async  Task<IActionResult> Profile(ECCSAModuleFlag module)
        {
            var model = new UserAdministrationViewModel
            {
                CurrentModule = module,
                FullName = TheUser.FullName,
                TheModel = await _userManager.FindByNameAsync(User.Identity.Name)
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SavePasswordChange(ChangePasswordModel model, ECCSAModuleFlag module)
        {
            var errorMsg = string.Empty;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password updated.");
                TempData["CommandStatus"] = "Password Updated";
                return RedirectToAction("ChangePassword", new { module });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }

            TempData["CommandStatus"] = errorMsg;
            return RedirectToAction("ChangePassword", new { module });
        }
        [HttpPost]
        public async Task<IActionResult> SaveSelfUserProfile(User model, ECCSAModuleFlag module)
        {
            var errorMsg = string.Empty;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Email != model.Email && user.MobileNo != model.MobileNo)
            {
                TempData["CommandStatus"] = "Email & Mobile no change at the same time is not allowed";
                return RedirectToAction("Profile", new { module });
            }
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.MobileNo;
            user.MobileNo = model.MobileNo;
            user.FullName = model.FullName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("Profile updated.");
                TempData["CommandStatus"] = "Profile Updated";
                return RedirectToAction("Profile", new { module });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }

            TempData["CommandStatus"] = errorMsg;
            return RedirectToAction("Profile", new { module });
        }
        [HttpGet]
        public async Task<IActionResult> LoginHistory(ECCSAModuleFlag module)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new UserAdministrationViewModel
            {
                CurrentModule = module,
                FullName = TheUser.FullName,
                TheModel = user,
                UserLoginHistories = await _repository.UserLoginHistory(user.Id)
            };
            return View(model);
        }

    }
}
