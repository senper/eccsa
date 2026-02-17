using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Controllers
{
    [Area("Membership")]
    public class AssociationController : BaseController
    {

        private readonly ILogger<AssociationController> _logger;
        private readonly AssociationRepository _associationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly StaffRepository _staffRepository;
        private readonly PositionTypeRepository _positionTypeRepository;
        private readonly ContactRepository _contactRepository;
        private readonly LegalDocumentTypeRepository _legalDocumentTypeRepository;
        private readonly MembershipRequirementRepository _requirementRepository;
        private readonly AssociationSettingRepository _settingRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IndividualRepository _individualRepoistory;
        private readonly BusinessProfileRepository _profileRepository;
        public AssociationController(
            ILogger<AssociationController> logger,
            UserManager<User> userManager,
            LocationRepository locationRepository,
            StaffRepository staffRepository,
            ContactRepository contactRepository,
            PositionTypeRepository positionTypeRepository,
            LegalDocumentTypeRepository legalDocumentTypeRepository,
            MembershipRequirementRepository requirementRepository,
        AssociationSettingRepository settingRepository,
            AssociationRepository associationRepository,
            IWebHostEnvironment hostingEnvironment,
            IndividualRepository individualRepoistory,
            BusinessProfileRepository profileRepository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _associationRepository = associationRepository;
            _locationRepository = locationRepository;
            _individualRepoistory = individualRepoistory;
            _staffRepository = staffRepository;
            _contactRepository = contactRepository;
            _positionTypeRepository = positionTypeRepository;
            _legalDocumentTypeRepository = legalDocumentTypeRepository;
            _requirementRepository = requirementRepository;
            _settingRepository = settingRepository;
            _hostingEnvironment = hostingEnvironment;
            _profileRepository = profileRepository;
            controllerName = "Association";
        }

        [HttpGet]
        public async Task<IActionResult> Print(Guid id)
        {
            //try
            //{
            FastReport.Report report = new();
            var mainDir = _hostingEnvironment.WebRootPath; //Directory.GetCurrentDirectory();
            var templateName = "texttemplate.frx";
            report.Load(Path.Combine(mainDir, "ReportTemplates", templateName));

            var data = new List<PrintModel>();
            var association = await _associationRepository.AssociationById(id, TheUser.Id);
            association.Employees = _staffRepository.Employees(id, TheUser.Id).ToList();
            data.Add(new PrintModel
            {
                Assocation = association.Name,
                Members = association.Employees.Select(s => new PrintDetailModel
                {
                    Position = s.PositionType.Name,
                    MemberName = s.NameEng
                }).ToList()
            });
            report.RegisterData(data, "MembersReport");

            report.Prepare();

            var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);
            var datestr = DateTime.Now.ToFileTime().ToString();
            var fileName = $"Reps_{datestr}.pdf";
            var dir = Path.Combine(outFolder, fileName);

            PDFSimpleExport pdfExport = new();

            pdfExport.Export(report, dir);

            report.Dispose();

            Response.Headers.Add("content-disposition", "inline;filename=" + fileName + ".pdf");
            var stream = new FileStream(dir, FileMode.Open);
            return File(stream, "application/pdf");
            //}
            //catch (Exception ex)
            //{
            //    TempData["CommandStatus"] = ex.Message;
            //}

            //return RedirectToAction("Index");
        }



        public async Task<IActionResult> Index()
        {
            var userName = TheUser.FullName;

            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "List",


            };
            try
            {
                model.TheList = await _associationRepository.List(TheUser.Id);
                if (model.TheList.Count == 1)
                {
                    var ass = model.TheList[0];
                    if (ass.Id == TheUser.AssociationId)
                    {
                        if (ass.AssociationLevel == LevelEnum.Woreda)
                        {
                            model.ShowNew = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.Association);

            return View(model);
        }

        public async Task<IActionResult> Detail(Guid id, int activeTabIndex = 0)
        {
            var userName = TheUser.FullName;
            LogStart(_logger, controllerName, "Detail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Detail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Detail",
                TheList = new List<Association>(),
                ProfileImages = GetProfileAttachedImages(id),
            };
            try
            {
                model.TheModel = await _associationRepository.AssociationById(id, TheUser.Id);
                model.legalDocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();
                model.TheModel.Employees = _staffRepository.Employees(id, TheUser.Id).ToList();
                model.TheModel = await _associationRepository.AssociationById(id, TheUser.Id);
                if (model.TheModel.ParentId != Guid.Empty)
                    model.TheModel.Parent = await _associationRepository.AssociationById(model.TheModel.ParentId, TheUser.Id);
                else
                    model.TheModel.Parent = Association.NULL;

                model.BoardMembers = model.TheModel.Employees.Where(s => s.PositionType.ForBoard).ToList();
                model.KeyStaffs = model.TheModel.Employees.Where(s => s.PositionType.ForStaff).ToList();
                model.Associations = await _associationRepository.ChildList(id, TheUser.Id);
                model.Individuals = await _individualRepoistory.List(TheUser.Id, id);
                model.LookupTypes = await _associationRepository.LookupTypes(model.CurrentModule);
                model.BoardChairman = model.TheModel.Employees.FirstOrDefault(l =>
                l.PositionTypeId == Constants.MembershipBoardChairmanId &&
                l.Status == StatusEnum.Active &&
                l.EndDate >= DateTime.Today);

                model.CEO = model.TheModel.Employees.FirstOrDefault(l =>
               l.PositionTypeId == Constants.MembershipCEOId &&
               l.Status == StatusEnum.Active &&
               l.EndDate >= DateTime.Today);
                if (model.BoardChairman == null)
                {
                    model.BoardChairman = new Staff()
                    {
                        Id = Guid.Empty,
                        NameEng = "",
                        NameLocal = "",
                        Mobile = "",
                        Email = "",
                        PositionType = new PositionType()
                        {
                            NameEng = "",
                            NameLocal = ""
                        }
                    };
                }
                model.CEO = model.KeyStaffs.FirstOrDefault(l => l.PositionTypeId == Constants.MembershipCEOId);
                if (model.CEO == null)
                {
                    model.CEO = new Staff()
                    {
                        Id = Guid.Empty,
                        NameEng = "",
                        NameLocal = "",
                        Mobile = "",
                        Email = "",
                        PositionType = new PositionType()
                        {
                            NameEng = "",
                            NameLocal = ""
                        }
                    };
                }
                model.BoardDirectorPath = ProcessImage(
                    model.BoardChairman.Id,
                    model.BoardChairman.Extension,
                    MembershipPhotoRelativePath,
                    MembershipPhotoAbsolutePath,
                   MembershipNoPersonUrl);
                model.SecretaryGeneralPath = ProcessImage(
                    model.CEO.Id,
                    model.CEO.Extension,
                    MembershipPhotoRelativePath,
                    MembershipPhotoAbsolutePath,
                    MembershipNoPersonUrl);

                model.LogoPath = ProcessImage(
                    model.TheModel.Id,
                    model.TheModel.Extension,
                    MembershipPhotoRelativePath,
                    MembershipPhotoAbsolutePath,
                   MembershipNoLogoUrl);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown Error");
            }

            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.ChangeActive(MenuItemEnum.Association);
            model.ActiveTabIndex = activeTabIndex;
            model.RoleLevel = roleLevel;
            return View(model);
        }
        private List<string> GetProfileAttachedImages(Guid associationId)
        {
            var id = Guid2String(associationId);
            string filePath = Path.Combine(ProfileImagePath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{ProfileRelativePath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProfileImage(Guid id, IFormFile image)
        {
            try
            {
                var idstr = Guid2String(id);
                string filePath = Path.Combine(ProfileImagePath, idstr);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                await SaveFile(filePath, image.FileName, image);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveProfileImage(Guid id, string fileName)
        {
            var idstr = Guid2String(id);
            string filePath = Path.Combine(ProfileImagePath, idstr);
            var splitedFileName = fileName.Split("/");
            if (splitedFileName.Length > 0)
            {
                var filteredFileName = splitedFileName[^1];
                System.IO.File.Delete(Path.Combine(filePath, filteredFileName));
            }
            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(Guid id, Guid associationId)
        {
            var userName = TheUser.FullName;
            var profiles = await _profileRepository.AssociationProfileByAssociationId(associationId, TheUser.Id);
            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Members",
                CurrentOperation = "Detail",
                TheModel = new Association
                {
                    Id = associationId
                }
            };
            model.ProfileAttribures = await _profileRepository.Attributes();
            model.ProfileAttribures = model.ProfileAttribures.Where(p => p.ForAssociation).ToList();
            if (id == Guid.Empty)
            {
                model.BusinessProfile = new AssociationProfile
                {
                    Id = Guid.Empty,
                    AssociationId = associationId,
                };
                foreach (var p in profiles)
                {
                    var x = model.ProfileAttribures.FirstOrDefault(a => a.Id == p.ProfileAttribureId);
                    model.ProfileAttribures.Remove(x);
                }
            }
            else
            {
                model.BusinessProfile = await _profileRepository.AssociationProfileById(id, TheUser.Id);
                model.ProfileAttribures = model.ProfileAttribures.Where(x => x.Id == model.BusinessProfile.ProfileAttribureId).ToList();
            }


            model.ChangeActive(MenuItemEnum.Association);
            return PartialView(model);

        }


        [HttpPost]
        public async Task<IActionResult> SaveProfile(AssociationViewModel model)
        {
            var m = model.BusinessProfile;
            try
            {
                await _profileRepository.SaveAssociationProfile(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id });
        }


        public async Task<IActionResult> Edit(Guid id)
        {
            var userName = TheUser.FullName;
            var lst = await _associationRepository.List(TheUser.Id);
            lst = lst.Where(l =>
                    (l.AssociationLevel == LevelEnum.Regional || l.AssociationLevel == LevelEnum.National) &&
                    (l.AssociationType == AssociationTypeEnum.Administrative)).ToList();
            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Edit",
            };
            model.TheList = lst;
            model.Locations = _locationRepository.Locations(TheUser.AssociationId).ToList();

            if (id != Guid.Empty)
            {
                model.TheModel = await _associationRepository.AssociationById(id, TheUser.Id);
            }
            else
            {
                model.TheModel = new Association();
            }

            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AssociationViewModel model)
        {
            try
            {
                var m = model.TheModel;
                var logo = ProcessImage(model.Photo);
                CreateAssociationInput theModel = new(
                    m.NameEng,
                    m.NameLocal,
                    m.ShortName,
                    m.ShortName_Local,
                    m.MemberSince,
                    m.AssociationType,
                    m.AssociationLevel,
                    m.ParentId,
                    m.LocationId,
                    logo.DataFiles,
                    logo.FileType,
                    m.Stakeholder.Status,
                    m.RegistrationFee,
                    m.RegistrationFee
                    );
                if (model.Photo != null)
                {
                    CheckFileExtension(model.Photo.FileName);
                }
                var result = await _associationRepository.Create(new GenericInput<CreateAssociationInput>(theModel, TheUser.Id));
                var fileName = $"{Guid2String(result.PayLoad.Id)}{result.PayLoad.Extension}";
                await SaveFile(MembershipPhotoAbsolutePath, fileName, model.Photo);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(AssociationViewModel model)
        {

            try
            {
                var logo = ProcessImage(model.Photo);
                var m = model.TheModel;
                UpdateAssociationInput theModel = new(
                    m.Id,
                    m.NameEng,
                    m.NameLocal,
                    m.ShortName,
                    m.ShortName_Local,
                    m.MemberSince,
                    m.AssociationType,
                    m.AssociationLevel,
                    m.ParentId,
                    m.LocationId,
                    logo.DataFiles,
                    logo.FileType,
                    m.Stakeholder.Status,
                     m.RegistrationFee,
                    m.RenewalFee
                    );
                if (model.Photo != null)
                {
                    CheckFileExtension(model.Photo.FileName);
                }
                var result = await _associationRepository.Update(new GenericInput<UpdateAssociationInput>(theModel, TheUser.Id));
                var fileName = $"{Guid2String(result.PayLoad.Id)}{result.PayLoad.Extension}";
                await SaveFile(MembershipPhotoAbsolutePath, fileName, model.Photo);
                CommandStatus(CommandStatusEnum.Updated);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("Detail", new { id = model.TheModel.Id });
        }

        [HttpPost]
        public async Task<IActionResult> SetProfile(Guid id, String profile)
        {
            var idstr = Guid2String(id);
            _ = Path.Combine(MembershipPhotoAbsolutePath, idstr);
            //var listofImageTags = profile.Where(s => )
            //get img 

            //check if img scr is d/f from filepath

            //if d/f remove img
            _ = await _associationRepository.UpdateProfile(id, profile, TheUser.Id);
            CommandStatus(CommandStatusEnum.Saved);
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> SetProfileLocal(Guid id, String profile)
        {
            var idstr = Guid2String(id);
            _ = Path.Combine(MembershipPhotoAbsolutePath, idstr);
            //var listofImageTags = profile.Where(s => )
            //get img 

            //check if img scr is d/f from filepath

            //if d/f remove img
            _ = await _associationRepository.UpdateProfile_Local(id, profile, TheUser.Id);
            CommandStatus(CommandStatusEnum.Saved);
            return RedirectToAction("Detail", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> SaveRequirement(MembershipRequirement model)
        {
            LogStart(_logger, controllerName, "SaveRequirement");

            try
            {
                var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
                if (roleLevel != RoleLevelEnum.Admin)
                {
                    _logger.LogError("SaveRequirement", "Insufficient Privilege");
                    CommandStatus("Insufficient Privilege");
                    return RedirectToAction("Detail", new { id = model.AssociationId });
                }
                await _associationRepository.Save(model, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);

            }
            catch (Exception ex)
            {
                _logger.LogError("SaveRequirement", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            return RedirectToAction("Detail", new { id = model.AssociationId, activeTabIndex = 1 });
        }
        [HttpGet]
        public async Task<IActionResult> EditStaff(Guid associationId, Guid id)
        {
            var userName = TheUser.FullName;
            _ = await _staffRepository.StaffById(id, TheUser.Id);

            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Edit",
            };

            model.PositionTypes = _positionTypeRepository.PositionTypes(Guid.Empty).ToList();

            if (id != Guid.Empty)
            {
                model.Staff = await _staffRepository.StaffById(id, Guid.Empty);
            }
            else
            {
                model.Staff = new Staff()
                {
                    Id = Guid.Empty,
                    AssociationId = associationId,

                };
            }
            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> StaffDetail(Guid associationId, Guid id)
        {
            var userName = TheUser.FullName;


            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Detail",
            };

            model.PositionTypes = _positionTypeRepository.PositionTypes(Guid.Empty).ToList();

            if (id != Guid.Empty)
            {
                model.Staff = await _staffRepository.StaffById(id, Guid.Empty);
            }
            else
            {
                return RedirectToAction("Detail", new { id = associationId });
            }
            var s = model.Staff;
            model.PhotoPath = ProcessImage(s.Id, s.Extension, MembershipPhotoRelativePath, MembershipPhotoAbsolutePath, MembershipNoPersonUrl);

            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveStaff(AssociationViewModel model)
        {
            var s = model.Staff;
            var photo = ProcessImage(model.Photo);
            var usi = new UpdateStaffInput(
                s.Id,
                s.Title,
                s.NameEng,
                s.NameLocal,
                s.PositionTypeId,
                s.AssociationId,
                DateTime.Today,
                DateTime.Today.AddYears(20),
                s.Email,
                s.Mobile,
                s.Status,
                photo.DataFiles,
                photo.FileType);
            var csi = new CreateStaffInput(s.Title,
                     s.NameEng,
                     s.NameLocal,
                     s.PositionTypeId,
                     s.AssociationId,
                     s.StartDate,
                     s.EndDate,
                     s.Email,
                     s.Mobile,
                     Server.Model.Common.StatusEnum.Active,
                     photo.DataFiles,
                     photo.FileType);
            try
            {
                StaffPayload result;
                if (model.Photo != null)
                {
                    CheckFileExtension(model.Photo.FileName);
                }
                if (s.Id == Guid.Empty)
                {
                    result = await _staffRepository.Create(new GenericInput<CreateStaffInput>(csi, TheUser.Id));
                }
                else
                {
                    result = await _staffRepository.Update(new GenericInput<UpdateStaffInput>(usi, TheUser.Id));
                }
                var fileName = $"{Guid2String(result.PayLoad.Id)}{result.PayLoad.Extension}";
                await SaveFile(MembershipPhotoAbsolutePath, fileName, model.Photo);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, s.Name, s.Email, s.Mobile, TheUser.FullName);
                CommandStatus(CommandStatusEnum.ValidationError);
            }

            return RedirectToAction("Detail", new { id = s.AssociationId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStaff(Guid associationId, Guid id)

        {
            await _staffRepository.Delete(id);

            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id = associationId });
        }

        [HttpGet]
        public async Task<IActionResult> EditContact(Guid associationId, Guid id)
        {
            var userName = TheUser.FullName;
            var lst = await _contactRepository.Contacts(associationId, TheUser.Id);

            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Edit",
            };

            model.Contact = lst.FirstOrDefault(c => c.Id == id);

            if (model.Contact == null)
            {
                model.Contact = new Contact()
                {
                    Id = Guid.Empty,
                    MemberId = associationId,
                    Priority = 5
                };
            }
            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveContact(AssociationViewModel model)

        {
            //   AssociationViewModel model = new AssociationViewModel();
            var c = model.Contact;
            UpdateContactInput theModel = new(c.Id, c.MemberId, c.ContactType, c.Address, c.Priority);
            try
            {
                if (theModel.Id == Guid.Empty)
                {
                    var csm = new CreateContactInput(theModel.MemberId,
                        theModel.ContactType, theModel.Address, theModel.Priority);
                    await _contactRepository.Create(new GenericInput<CreateContactInput>(csm, TheUser.Id));
                }
                else
                {
                    await _contactRepository.Update(new GenericInput<UpdateContactInput>(theModel, TheUser.Id));
                }
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, c.ContactType, c.Address, TheUser.FullName);
                CommandStatus(CommandStatusEnum.ValidationError);
            }

            return RedirectToAction("Detail", new { id = theModel.MemberId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(Guid memberId, Guid id)
        {
            //   AssociationViewModel model = new AssociationViewModel();
            await _contactRepository.Delete(id);
            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id = memberId });
        }

        [HttpGet]
        public async Task<IActionResult> EditDocument(Guid associationId, Guid id)
        {
            var userName = TheUser.FullName;
            _ = await _staffRepository.StaffById(id, TheUser.Id);

            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Edit",
            };

            model.legalDocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();

            if (id != Guid.Empty)
            {
                model.MembershipRequirement = await _requirementRepository.MembershipRequirementById(id, Guid.Empty);
            }
            else
            {
                model.MembershipRequirement = new MembershipRequirement()
                {
                    Id = Guid.Empty,
                    AssociationId = associationId,

                };
            }
            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DocumentDetail(Guid associationId, Guid id)
        {
            var userName = TheUser.FullName;


            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Detail",
            };

            model.legalDocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();

            if (id != Guid.Empty)
            {
                model.MembershipRequirement = await _requirementRepository.MembershipRequirementById(id, Guid.Empty);
            }
            else
            {
                return RedirectToAction("Detail", new { id = associationId });
            }


            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }
        [HttpPost]
        public IActionResult SaveDocument(AssociationViewModel model)
        {
            var s = model.MembershipRequirement;
            //var usi = new UpdateMembershipRequirementInput(
            //    s.Id,
            //    s.AssociationId,
            //    s.DocumentTypeId,
            //    s.ApplicationRequirementLevel,
            //    s.RenewalRequirementLevel);
            //var csi = new CreateMembershipRequirementInput(s.AssociationId,
            //    s.DocumentTypeId,
            //    s.ApplicationRequirementLevel,
            //    s.RenewalRequirementLevel);
            //try
            //{
            //    if (s.Id == Guid.Empty)
            //    {
            //        await _requirementRepository.Create(new GenericInput<CreateMembershipRequirementInput>(csi, TheUser.Id));
            //    }
            //    else
            //    {
            //        await _requirementRepository.Update(new GenericInput<UpdateMembershipRequirementInput>(usi, TheUser.Id));
            //    }
            //    CommandStatus(CommandStatusEnum.Saved);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message, s, TheUser.FullName);
            //    CommandStatus(CommandStatusEnum.ValidationError);
            //}

            return RedirectToAction("Detail", new { id = s.AssociationId, activeTabIndex = 1 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument(Guid associationId, Guid id)

        {
            await _staffRepository.Delete(id);

            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id = associationId, activeTabIndex = 1 });
        }



        [HttpGet]
        public async Task<IActionResult> EditSetting(Guid associationId, Guid id)
        {
            var userName = TheUser.FullName;
            _ = await _staffRepository.StaffById(id, TheUser.Id);

            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Edit",
            };


            if (id != Guid.Empty)
            {
                model.Setting = await _settingRepository.AssociationSettingById(id, Guid.Empty);
            }
            else
            {
                model.Setting = new AssociationSetting()
                {
                    Id = Guid.Empty,
                    AssociationId = associationId,

                };
            }
            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> SettingDetail(Guid associationId, Guid id)
        {
            var userName = TheUser.FullName;


            var model = new AssociationViewModel
            {
                FullName = userName,
                CurrentPage = "Associations",
                CurrentOperation = "Detail",
            };


            if (id != Guid.Empty)
            {
                model.Setting = await _settingRepository.AssociationSettingById(id, Guid.Empty);
            }
            else
            {
                return RedirectToAction("Detail", new { id = associationId });
            }

            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveSetting(AssociationViewModel model)
        {
            var s = model.Setting;
            var usi = new UpdateAssociationSettingInput(
                s.Id,
                s.AssociationId,
                s.BankName,
                s.AccountNumber,
                ECCSAModuleFlag.Membership);
            var csi = new CreateAssociationSettingInput(s.AssociationId,
               s.BankName,
                s.AccountNumber,
                ECCSAModuleFlag.Membership);
            try
            {
                if (s.Id == Guid.Empty)
                {
                    await _settingRepository.Create(new GenericInput<CreateAssociationSettingInput>(csi, TheUser.Id));
                }
                else
                {
                    await _settingRepository.Update(new GenericInput<UpdateAssociationSettingInput>(usi, TheUser.Id));
                }
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, s, TheUser.FullName);
                CommandStatus(CommandStatusEnum.ValidationError);
            }

            return RedirectToAction("Detail", new { id = s.AssociationId, activeTabIndex = 2 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSetting(Guid associationId, Guid id)

        {
            var st = await _settingRepository.AssociationSettingById(id, Guid.Empty);
            if (st != null)
            {
                await _settingRepository.Delete(st, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            return RedirectToAction("Detail", new { id = associationId, activeTabIndex = 2 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRequirement(Guid requirementId, Guid associationId)

        {
            await _associationRepository.Delete(new MembershipRequirement() { Id = requirementId, AssociationId = associationId }, TheUser.Id);
            CommandStatus(CommandStatusEnum.Deleted);

            return RedirectToAction("Detail", new { id = associationId, activeTabIndex = 1 });
        }


    }
}
