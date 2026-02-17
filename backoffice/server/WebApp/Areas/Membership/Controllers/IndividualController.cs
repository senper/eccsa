using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
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
    public class IndividualController : BaseController
    {
        private readonly ILogger<IndividualController> _logger;
        private readonly AssociationRepository _associationRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly IndividualRepository _individualRepoistory;
        private readonly BusinessProfileRepository _profileRepository;
        private readonly LegalDocumentRepository _legalDocumentRepository;
        private readonly PositionTypeRepository _positionTypeRepository;
        private readonly ContactRepository _contactRepository;
        private readonly LegalDocumentTypeRepository _legalDocumentTypeRepository;
        private readonly LocationRepository _locationRepository;
        private readonly MembershipEnrollmentRepository _membershipRepository;
        private readonly YesNoRepository _lookupRepository;
        private readonly StaffRepository _staffRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public IndividualController(ILogger<IndividualController> logger, UserManager<User> userManager, StaffRepository staffRepository, AssociationRepository associationRepository, IndividualRepository individualRepoistory, BusinessProfileRepository profileRepository, LegalDocumentRepository legalDocumentRepository, PositionTypeRepository positionTypeRepository, ContactRepository contactRepository, LegalDocumentTypeRepository legalDocumentTypeRepository, LocationRepository locationRepository, MembershipEnrollmentRepository membershipRepository, InvoiceRepository invoiceRepository, YesNoRepository lookupRepository, IWebHostEnvironment hostingEnvironment, PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {

            _logger = logger;
            _staffRepository = staffRepository;
            _associationRepository = associationRepository;
            _individualRepoistory = individualRepoistory;
            _profileRepository = profileRepository;
            _legalDocumentRepository = legalDocumentRepository;
            _positionTypeRepository = positionTypeRepository;
            _hostingEnvironment = hostingEnvironment;
            _locationRepository = locationRepository;

            _contactRepository = contactRepository;

            _legalDocumentTypeRepository = legalDocumentTypeRepository;
            _membershipRepository = membershipRepository;

            _invoiceRepository = invoiceRepository;
            _lookupRepository = lookupRepository;
            controllerName = "Individual";
        }

        [HttpGet]
        public async Task<JsonResult> GetTree()
        {
            var associations = await _associationRepository.List(TheUser.Id);
            var tview = TreeView.BuildTree(associations, TheUser.AssociationId);
            return Json(tview);
        }

        [HttpGet]
        public JsonResult GetLocation(Guid? parentId)
        {
            var locations = _locationRepository.Locations(TheUser.Id).ToList();
            Guid pId;
            if (parentId.HasValue && parentId.Value != Guid.Empty)
            {
                pId = parentId.Value;
            }
            else
            {
                pId = Constants.EthiopiaId;
            }
            var tmp = locations.Where(l => l.ParentId == pId
                  || l.Id == pId).ToList();

            tmp.Insert(0, new Location
            {
                Id = Guid.Empty,
                NameEng = string.Empty,
                NameLocal = string.Empty,
                Code = string.Empty
            });
            var lst = new List<LookupView>();
            foreach (var t in tmp)
            {
                lst.Add(new LookupView
                {
                    Id = t.Id,
                    Name = t.Name,
                    Code = t.Code,
                    ParentId = t.ParentId
                });
            }
            return Json(lst);
        }

        [HttpGet]
        public JsonResult PriceRss()
        {
            return Json(Rss());
        }

        public async Task<IActionResult> Index(Guid? associationId)
        {
            LogStart(_logger, controllerName, "Index");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "Index");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var userName = TheUser.FullName;
            var currentAssociationId = associationId.HasValue && associationId.Value != Guid.Empty ? associationId.Value : TheUser.AssociationId;
            //var associations = await _associationRepository.List(TheUser.Id);
            //var tview = AssociationTreeView.BuildTree(associations, TheUser.AssociationId);
            //var json = tview.ToString();
            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Members",
                CurrentOperation = "List",
                TheList = await _membershipRepository.List(currentAssociationId, TheUser.Id),
                Associations = await _associationRepository.ChildList(currentAssociationId, TheUser.Id),
                AssociationId = currentAssociationId

            };
            if (model.Associations.Count == 1)
            {
                if (model.Associations[0].AssociationLevel == LevelEnum.Woreda)
                {
                    model.ShowAssociations = false;
                    model.AssociationId = currentAssociationId;
                }
            }
            model.Associations.Insert(0, new Association
            {
                Id = Guid.Empty,
                NameEng = "",
                NameLocal = "",
                ShortName = "",
                ShortName_Local = "",
            });
            model.TheModel = new MembershipEnrollment()
            {
                Id = Guid.Empty,
                Stakeholder = new Stakeholder()
                {
                    Id = Guid.Empty,
                    Individual = new Individual
                    {
                        Id = Guid.Empty,
                        LocationId = Guid.Empty
                    }
                },
                ParentAssociationId = currentAssociationId
            };
            model.TheMember = new Individual
            {
                Id = Guid.Empty,
                Stakeholder = new Stakeholder
                {
                    Id = Guid.Empty
                }
            };
            model.Sectors = (await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId)).LookupValues.ToList();
            model.ChangeActive(MenuItemEnum.Individual);
            foreach (var s in model.TheList)
            {
                if (s.Member == null) continue;
                if (!s.Member.SectorId.HasValue) continue;
                var sectorId = s.Member.SectorId.Value;

                s.Member.Sector = model.Sectors.Find(s => s.Id == sectorId)?.Name ?? "";
            }
            TempData["title"] = "Members";
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "Index");
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
            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Members",
                CurrentOperation = "Detail",

                TheModel = await _membershipRepository.MembershipEnrollmentById(id, TheUser.Id),

            };
            model.TheMember = await _individualRepoistory.IndividualById(model.TheModel.MemberId, TheUser.Id);
            model.LegalDocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();
            model.Locations = _locationRepository.Locations(Guid.Empty).ToList();
            model.PositionTypes = _positionTypeRepository.PositionTypes(Guid.Empty).ToList();
            model.Staffs = _staffRepository.IndividualEmployees(id, TheUser.Id).ToList();
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            if (model.TheModel.RenewalStatus == MembershipWorkflowEnum.Accepted || model.TheModel.RenewalStatus == MembershipWorkflowEnum.NotSet)
            {
                model.Invoice = new Invoice();
                model.Payment = new Settlment();
            }
            else
            {
                model.Invoice = await _invoiceRepository.InvoiceByNo(model.TheModel.InvoiceNo, model.TheModel.LastRenewalId);
                if (model.Invoice == null)
                {
                    model.Invoice = new Invoice();
                    model.Payment = new Settlment();

                }
                else
                {
                    model.Payment = new Settlment
                    {
                        Id = Guid.Empty,
                        InvoiceId = model.Invoice.Id,

                    };
                }
            }
            if (model.TheModel.Member?.LegalDocuments != null)
            {
                foreach (var res in model.TheModel.Member.LegalDocuments)
                {
                    if (res.Requirement.InputMethod == ServiceInputTypeEnum.Upload)
                    {
                        var fileName = Guid2String(res.Id);
                        var filePath = AbsolutePath(ECCSAModuleFlag.Membership);
                        var extension = GetExtension(filePath, fileName);
                        res.RelativePath = Path.Combine(RelativePath(ECCSAModuleFlag.Membership), $"{fileName}{extension}");
                    }
                    else if (res.Requirement.InputMethod == ServiceInputTypeEnum.LegalDocument)
                    {
                        var userPath = Guid2String(model.TheModel.Member.AssociatedUserId);
                        var filePath = Path.Combine(UserLegalProfileDocumentsAbsolutePath, userPath);
                        var fileName = Guid2String(res.LookupValueId.Value);
                        var extension = GetExtension(filePath, fileName);
                        res.RelativePath = Path.Combine(UserLegalProfileDocumentsRelativePath, userPath, $"{fileName}{extension}");
                    }
                }
            }
            model.Sectors = (await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId)).LookupValues.ToList();
            if (model.TheModel.Member?.SectorId.HasValue == true)
            {
                var sectorId = model.TheModel.Member?.SectorId.Value;
                var sector = model.Sectors.Find(s => s.Id == sectorId);
                if (sector != null)
                {
                    model.TheModel.Member.Subsector = sector.ParentId != null ? sector?.Name ?? "" : "";
                    model.TheModel.Member.Sector = sector.ParentId != null ? model.Sectors.Find(s => s.Id == sector.ParentId)?.Name ?? "" : sector?.Name ?? "";
                }
            }
            model.ChangeActive(MenuItemEnum.Individual);
            model.RoleLevel = roleLevel;
            model.ActiveTabIndex = activeTabIndex;
            LogEnd(_logger, controllerName, "Detail");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditStaff(Guid individualId, Guid id)
        {
            var userName = TheUser.FullName;

            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Individuals",
                CurrentOperation = "Edit",
            };

            model.PositionTypes = _positionTypeRepository.PositionTypes(Guid.Empty).ToList();

            if (id != Guid.Empty)
            {
                model.Staff = await _staffRepository.IndividualStaffById(id, Guid.Empty);
            }
            else
            {
                model.Staff = new IndividualStaff()
                {
                    Id = Guid.Empty,
                    IndividualId = individualId,

                };
            }
            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> StaffDetail(Guid individualId, Guid id)
        {
            var userName = TheUser.FullName;


            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Individuals",
                CurrentOperation = "Detail",
            };

            model.PositionTypes = _positionTypeRepository.PositionTypes(Guid.Empty).ToList();

            if (id != Guid.Empty)
            {
                model.Staff = await _staffRepository.IndividualStaffById(id, Guid.Empty);
            }
            else
            {
                return RedirectToAction("Detail", new { id = individualId });
            }
            var s = model.Staff;
            model.PhotoPath = ProcessImage(s.Id, s.Extension, MembershipPhotoRelativePath, MembershipPhotoAbsolutePath, MembershipNoPersonUrl);

            model.ChangeActive(MenuItemEnum.Association);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveStaff(IndividualViewModel model)
        {
            var s = model.Staff;
            var photo = ProcessImage(model.Photo);
            var usi = new UpdateIndividualStaffInput(
                s.Id,
                s.Title,
                s.NameEng,
                s.NameLocal,
                s.PositionTypeId,
                s.IndividualId,
                DateTime.Today,
                DateTime.Today.AddYears(20),
                s.Email,
                s.Mobile,
                s.Status,
                photo.DataFiles,
                photo.FileType);
            var csi = new CreateIndividualStaffInput(s.Title,
                     s.NameEng,
                     s.NameLocal,
                     s.PositionTypeId,
                     s.IndividualId,
                     s.StartDate,
                     s.EndDate,
                     s.Email,
                     s.Mobile,
                     Server.Model.Common.StatusEnum.Active,
                     photo.DataFiles,
                     photo.FileType);
            try
            {
                IndividualStaffPayload result;
                if (model.Photo != null)
                {
                    CheckFileExtension(model.Photo.FileName);
                }
                if (s.Id == Guid.Empty)
                {
                    result = await _staffRepository.CreateIndividualStaff(new GenericInput<CreateIndividualStaffInput>(csi, TheUser.Id));
                }
                else
                {
                    result = await _staffRepository.UpdateIndividualStaff(new GenericInput<UpdateIndividualStaffInput>(usi, TheUser.Id));
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

            return RedirectToAction("Detail", new { id = s.IndividualId, activeTabIndex = 7 });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStaff(Guid individualId, Guid id)

        {
            await _staffRepository.DeleteIndividualStaff(id);

            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id = individualId, activeTabIndex = 7 });
        }

        public async Task<IActionResult> Edit(Guid id, Guid associationId)
        {
            var userName = TheUser.FullName;

            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Members",
                CurrentOperation = "Detail",
                TheList = new List<MembershipEnrollment>(),
            };
            model.Locations = _locationRepository.Locations(TheUser.Id).ToList();
            if (id == Guid.Empty)
            {
                model.TheModel = new MembershipEnrollment()
                {
                    Id = Guid.Empty,
                    Stakeholder = new Stakeholder()
                    {
                        Id = Guid.Empty,
                        Individual = new Individual
                        {
                            Id = Guid.Empty,
                            LocationId = Guid.Empty
                        }
                    },
                    ParentAssociationId = associationId
                };
                model.TheMember = new Individual
                {
                    Id = Guid.Empty,
                    Stakeholder = new Stakeholder
                    {
                        Id = Guid.Empty
                    }
                };
            }
            else
            {
                model.TheModel = await _membershipRepository.MembershipEnrollmentById(id, TheUser.Id);
                model.TheMember = await _individualRepoistory.IndividualById(model.TheModel.MemberId, TheUser.Id);

            }
            model.Sectors = (await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId)).LookupValues.ToList();

            model.ChangeActive(MenuItemEnum.Individual);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(IndividualViewModel model)
        {
            var m = model.TheModel;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = m.Id });
            }

            m.DateOfEnrollment = model.TheMember.MemberSince;
            try
            {
                m.Stakeholder = new Stakeholder
                {
                    Id = model.TheMember.Id,
                    Individual = new Individual
                    {
                        Id = model.TheMember.Id,
                        NameEng = model.TheMember.NameEng,
                        NameLocal = model.TheMember.NameLocal,
                        TaxIdNumber = model.TheMember.TaxIdNumber,
                        LocationId = model.LocationId,
                        SectorId = model.TheMember.SectorId,
                    }
                };
                await _membershipRepository.Save(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                _logger.LogError("Save", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
            }
            if (m.Id == Guid.Empty)
                return RedirectToAction("Index", new { associationId = m.ParentAssociationId });
            return RedirectToAction("Detail", new { id = m.Id, activeTabIndex = model.ActiveTabIndex });
        }

        [HttpPost]
        public async Task<IActionResult> Update(AssociationViewModel model)
        {
            var logo = ProcessImage(model.Photo);
            var m = model.TheModel;
            LogStart(_logger, controllerName, "Update");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "Save");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = m.Id });
            }
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
            _ = await _associationRepository.Update(new GenericInput<UpdateAssociationInput>(theModel, TheUser.Id));
            CommandStatus(CommandStatusEnum.Updated);
            return RedirectToAction("Detail", new { id = theModel.Id });
        }

        [HttpGet]
        public async Task<FileResult> DownLoadFile(Guid id, FolderTypeEnum type, string extension)
        {
            var filePath = type == FolderTypeEnum.Document ? MembershipDocumentAbsolutePath : MembershipPhotoAbsolutePath;
            var fileName = String.Concat(Guid2String(id), extension);
            return await DownLoadFile(filePath, fileName);
        }


        [HttpGet]
        public async Task<IActionResult> EditContact(Guid individualId, Guid id)
        {
            var userName = TheUser.FullName;
            var lst = await _contactRepository.Contacts(individualId, TheUser.Id);

            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Individual",
                CurrentOperation = "Edit",
            };

            model.Contact = lst.FirstOrDefault(c => c.Id == id);

            if (model.Contact == null)
            {
                model.Contact = new Contact()
                {
                    Id = Guid.Empty,
                    MemberId = individualId,
                    Priority = 5
                };
            }
            model.ChangeActive(MenuItemEnum.Individual);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SaveContact(IndividualViewModel model)

        {
            //   AssociationViewModel model = new AssociationViewModel();
            var c = model.Contact;
            UpdateContactInput theModel = new(c.Id, c.MemberId, c.ContactType, c.Address, c.Priority);

            LogStart(_logger, controllerName, "SaveContact");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Membership);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveContact");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("Detail", new { id = c.MemberId });
            }
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
                _logger.LogError(ex.Message, c, TheUser.FullName);
                CommandStatus(CommandStatusEnum.ValidationError);
            }

            return RedirectToAction("Detail", new { id = theModel.MemberId, activeTabIndex = model.ActiveTabIndex });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(Guid memberId, Guid id)
        {
            //   AssociationViewModel model = new AssociationViewModel();
            await _contactRepository.Delete(id);
            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("Detail", new { id = memberId, activeTabIndex = 0 });
        }

        //[HttpGet]
        //public async Task<IActionResult> EditLegalDocument(Guid id, Guid individualId, Guid enrollmentId)
        //{
        //    var userName = TheUser.FullName;

        //    var model = new IndividualViewModel
        //    {
        //        FullName = userName,
        //        CurrentPage = "Members",
        //        CurrentOperation = "Detail",
        //        TheModel = new MembershipEnrollment
        //        {
        //            Id = enrollmentId,
        //            MemberId = individualId
        //        }
        //    };
        //    if (id == Guid.Empty)
        //    {
        //        model.LegalDocument = new LegalDocument
        //        {
        //            Id = Guid.Empty,
        //            IndividualId = individualId,
        //        };
        //    }
        //    else
        //    {
        //        model.LegalDocument = await _legalDocumentRepository.LegalDocumentById(id, TheUser.Id);
        //    }
        //    model.LegalDocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();
        //    model.ChangeActive(MenuItemEnum.Individual);
        //    return View(model);

        //}


        [HttpPost]
        public IActionResult SaveLegalDocument(IndividualViewModel model)
        {
            var m = model.LegalDocument;
            string extension = string.Empty;
            if (model.Photo.Length > 0)
            {
                extension = Path.GetExtension(Path.GetFileName(model.Photo.FileName));
            }
            try
            {
                //TODO: update legal documents
                //LegalDocumentPayload payload;
                //if (m.Id == Guid.Empty)
                //{
                //    cldi = new CreateLegalDocumentInput(m.IndividualId, m.LegalDocumentTypeId, m.Document,  extension);
                //    payload = await _legalDocumentRepository.Create(new GenericInput<CreateLegalDocumentInput>(cldi, TheUser.Id));
                //}
                //else
                //{
                //    uldi = new UpdateLegalDocumentInput(m.Id, m.IndividualId, m.LegalDocumentTypeId, m.Document, extension, m.Status, m.Remark);
                //    payload = await _legalDocumentRepository.Update(new GenericInput<UpdateLegalDocumentInput>(uldi, TheUser.Id));
                //}
                //if (model.Photo.Length > 0)
                //{
                //    var newFileName = String.Concat(Guid2String(payload.PayLoad.Id), extension);
                //    await SaveFile(MembershipDocumentAbsolutePath, newFileName, model.Photo);
                //}
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }



        //[HttpGet]
        //public async Task<IActionResult> EditDocumentStatus(Guid id, Guid individualId, Guid enrollmentId)
        //{
        //    var userName = TheUser.FullName;

        //    var model = new IndividualViewModel
        //    {
        //        FullName = userName,
        //        CurrentPage = "Members",
        //        CurrentOperation = "Detail",
        //        TheModel = new MembershipEnrollment
        //        {
        //            Id = enrollmentId,
        //            MemberId = individualId
        //        }
        //    };
        //    if (id == Guid.Empty)
        //    {
        //        model.LegalDocument = new LegalDocument
        //        {
        //            Id = Guid.Empty,
        //            IndividualId = individualId,
        //        };
        //    }
        //    else
        //    {
        //        model.LegalDocument = await _legalDocumentRepository.LegalDocumentById(id, TheUser.Id);
        //    }
        //    model.LegalDocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();
        //    model.ChangeActive(MenuItemEnum.Individual);
        //    return View(model);

        //}


        [HttpPost]
        public IActionResult UpdateDocumentStatus(IndividualViewModel model)
        {
            var m = model.LegalDocument;
            //TODO: update legal documents
            //if (model.Photo.Length > 0)
            //{
            //    _ = Path.GetExtension(Path.GetFileName(model.Photo.FileName));
            //}
            //try
            //{
            //    LegalDocumentPayload payload;
            //    if (m.Id == Guid.Empty)
            //    {
            //        CommandStatus(CommandStatusEnum.ValidationError);
            //        return RedirectToAction("Detail", new { id = model.TheModel.Id });
            //    }
            //    else
            //    {
            //       var uldi = new UpdateLegalDocumentStatusInput(m.Id, model.TheModel.Id, m.IndividualId,  m.Status, m.Remark);
            //        payload = await _legalDocumentRepository.Update(new GenericInput<UpdateLegalDocumentStatusInput>(uldi, TheUser.Id));
            //    }

            //    CommandStatus(CommandStatusEnum.Saved);
            //}
            //catch (Exception ex)
            //{
            //    TempData["CommandStatus"] = ex.Message;
            //}

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile(Guid id, Guid individualId, Guid memberId)
        {
            var userName = TheUser.FullName;
            var profiles = await _profileRepository.BusinessProfileByIndividualId(individualId, TheUser.Id);
            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Members",
                CurrentOperation = "Detail",
                TheModel = new MembershipEnrollment
                {
                    Id = memberId,
                    MemberId = individualId
                }
            };
            model.ProfileAttribures = await _profileRepository.Attributes();
            model.ProfileAttribures = model.ProfileAttribures.Where(p => p.ForIndividual).ToList();
            if (id == Guid.Empty)
            {
                model.BusinessProfile = new BusinessProfile
                {
                    Id = Guid.Empty,
                    IndividualId = individualId,
                };
                foreach (var p in profiles)
                {
                    var x = model.ProfileAttribures.FirstOrDefault(a => a.Id == p.ProfileAttribureId);
                    model.ProfileAttribures.Remove(x);
                }
            }
            else
            {
                model.BusinessProfile = await _profileRepository.BusinessProfileById(id, TheUser.Id);
                model.ProfileAttribures = model.ProfileAttribures.Where(x => x.Id == model.BusinessProfile.ProfileAttribureId).ToList();
            }

            model.ChangeActive(MenuItemEnum.Individual);
            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> SaveProfile(IndividualViewModel model)
        {
            var m = model.BusinessProfile;
            try
            {
                await _profileRepository.SaveIndividualProfile(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }



        [HttpGet]
        public async Task<IActionResult> RenewMembership(Guid id, Guid membershipId)
        {
            var userName = TheUser.FullName;

            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Members",
                CurrentOperation = "Detail",
            };

            if (id == Guid.Empty)
            {
                model.RenewalHistory = new RenewalHistory
                {
                    Id = Guid.Empty,
                    EnrollmentId = membershipId,
                };
            }
            else
            {
                model.TheModel = await _membershipRepository.MembershipEnrollmentById(membershipId, TheUser.Id);
                model.RenewalHistory = model.TheModel.RenewalHistory.FirstOrDefault(r => r.Id == id);
            }

            model.ChangeActive(MenuItemEnum.Individual);
            return View(model);

        }






        [HttpGet]
        public async Task<IActionResult> Payment(Guid id, Guid individualId, Guid invoiceId)
        {
            IndividualViewModel model = new()
            {

            };
            try
            {

                model.TheModel = await _membershipRepository.MembershipEnrollmentById(id, TheUser.Id);
                model.Invoice = await _invoiceRepository.InvoiceByNo(model.TheModel.InvoiceNo, model.TheModel.LastRenewalId);
                model.Payment = new Server.Model.Payment.Settlment
                {
                    Id = Guid.Empty,
                    InvoiceId = model.Invoice.Id,

                };
                CommandStatus(CommandStatusEnum.PaymentMade);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> ApproveRejectRenewal(IndividualViewModel model)
        {
            var m = model.TheModel;
            try
            {
                var uldi = new ApproveRejectRenewalInput(m.Id, m.RenewalStatus, m.Remark);
                await _membershipRepository.ApproveRejectRenewal(m, TheUser.Id);
                if (m.RenewalStatus == MembershipWorkflowEnum.Accepted)
                    CommandStatus(CommandStatusEnum.Approved);
                else
                    CommandStatus(CommandStatusEnum.Rejected);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }

        [HttpGet]
        public async Task<IActionResult> EditTradeInfo(Guid id)
        {
            var userName = TheUser.FullName;

            var model = new IndividualViewModel
            {
                FullName = userName,
                CurrentPage = "Members",
                CurrentOperation = "Detail",
            };
            if (id == Guid.Empty)
            {
                model.LegalDocument = new LegalDocument
                {
                    Id = Guid.Empty,
                };
            }
            else
            {
                model.TheMember = await _individualRepoistory.IndividualById(id, TheUser.Id);
            }

            model.ChangeActive(MenuItemEnum.Individual);
            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> ImportIndividuals(Guid associationId, IFormFile file)
        {
            List<ImportTemplate> columns;
            columns = new List<ImportTemplate>
            {
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "CompanyName"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "TIN"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Mobile"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "MemberCode"
                },
                new ImportTemplate()
                {
                    ColumnType = DataTypeEnum.String,
                    Name = "Sector"
                }
            };
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file format");
            }
            var association = await _associationRepository.AssociationById(associationId, TheUser.Id);
            var locationId = association.LocationId;
            var lst = ImportExcel<IndividualImportModel>(file, columns);
            var sectors = (await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId)).LookupValues.ToList();

            foreach (var item in lst)
            {
                var m = new MembershipEnrollment
                {
                    Id = item.Id,
                    MemberId = item.Id,
                    LastRenewalDate = DateTime.Today,
                    DateOfEnrollment = DateTime.Today,
                    ExpiryDate = DateTime.Today,
                    Code = item.MemberCode,
                    ParentAssociationId = associationId,
                    Stakeholder = new Stakeholder
                    {
                        Id = item.Id,
                        Individual = new Individual
                        {
                            Id = item.Id,
                            NameEng = item.CompanyName,
                            NameLocal = String.Empty,
                            TaxIdNumber = item.TIN,
                            LocationId = locationId,
                            Sector = item.Sector,
                            SectorId = sectors.FirstOrDefault(s => s.Name.ToLower() == item.Sector.ToLower())?.Id,
                        }
                    }
                };


                try
                {
                    await _membershipRepository.Import(m, TheUser.Id);
                }
                catch (Exception ex)
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            return RedirectToAction("Index", new { associationId });
        }

        [HttpPost]
        public async Task<IActionResult> ImportIndividualsNew(Guid associationId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file format");
            }
            var association = await _associationRepository.AssociationById(associationId, TheUser.Id);
            var locationId = association.LocationId;
            var lst = IndividualImportColumnMap.ImportExcel(file, TempAbsolutePath);
            var sectors = (await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId)).LookupValues.ToList();

            foreach (var item in lst)
            {
                var sectorId = sectors.FirstOrDefault(s => s.Name.Trim().ToLower() == item.Subsector.Trim().ToLower())?.Id;
                item.SectorId = sectorId == null ? sectors.FirstOrDefault(s => s.Name.Trim().ToLower() == item.Sector.Trim().ToLower())?.Id : sectorId;
                item.LocationId = locationId;
                var m = new MembershipEnrollment
                {
                    Id = item.Id,
                    MemberId = item.Id,
                    LastRenewalDate = DateTime.Today,
                    DateOfEnrollment = DateTime.Today,
                    ExpiryDate = DateTime.Today,
                    Code = item.No,

                    ParentAssociationId = associationId,
                    Stakeholder = new Stakeholder
                    {
                        Id = item.Id,
                        Individual = new Individual
                        {
                            Id = item.Id,
                            NameEng = item.NameEng,
                            NameLocal = item.NameLocal,
                            SectorId = item.SectorId,
                            TradeRegistrationNumber = item.TradeRegistrationNumber ?? string.Empty,
                            Address = item.Address ?? string.Empty,
                            LocationId = item.LocationId,
                            Capital = item.Capital ?? string.Empty,
                            NumberofEmployees = item.NumberofEmployees,
                            TaxIdNumber = item.TaxIdNumber,
                            Employees = item.Employees,
                            BusinessLicenses = item.BusinessLicenses,
                            CustomFields = item.CustomFields,
                        },

                        Contacts = item.Stakeholder.Contacts.ToList(),
                        Representatives = item.Stakeholder.Representatives.ToList()
                    }
                };


                try
                {
                    await _membershipRepository.ImportNew(m, TheUser.Id);
                }
                catch (Exception ex)
                {
                    TempData["CommandStatus"] = ex.Message;
                }
            }
            return RedirectToAction("Index", new { associationId });
        }
        [HttpGet]
        public async Task<IActionResult> ExportIndividuals(Guid associationId)
        {
            try
            {
                var sectors = (await _lookupRepository.LookupTypeById(Persistence.Repositories.Constants.SectorLookupTypeId)).LookupValues.ToList();
                var locations = _locationRepository.Locations(Guid.Empty).ToList();
                var list = await _membershipRepository.ListForExport(associationId, TheUser.Id);
                var workbook = IndividualExportColumnMap.ExportExcel(list, locations, sectors);

                if (!Directory.Exists(TempAbsolutePath))
                {
                    Directory.CreateDirectory(TempAbsolutePath);
                }
                string fullPath = Path.Combine(TempAbsolutePath, "ExportIndividualList.xls");
                var stream = new MemoryStream();
                workbook.Write(stream);
                var fileName = "Members.xls";
                Response.Headers.Add("content-disposition", "inline;filename=" + $"{fileName}");

                return File(stream.ToArray(), "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> SaveLicense(IndividualViewModel model)
        {
            var m = model.License;
            try
            {
                await _individualRepoistory.Save(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }
        [HttpPost]
        public async Task<IActionResult> SaveManufacturing(IndividualViewModel model)
        {
            var m = model.Manufacturing;
            try
            {
                await _individualRepoistory.Save(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }
        [HttpPost]
        public async Task<IActionResult> SaveService(IndividualViewModel model)
        {
            var m = model.Service;
            try
            {
                await _individualRepoistory.Save(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }
        [HttpPost]
        public async Task<IActionResult> SaveTrade(IndividualViewModel model)
        {
            var m = model.Trade;
            try
            {
                await _individualRepoistory.Save(m, TheUser.Id);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = model.TheModel.Id, activeTabIndex = model.ActiveTabIndex });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteLicense(Guid id, Guid individualId, Guid memberId, int activeTabIndex)
        {

            try
            {
                await _individualRepoistory.DeleteIndividualLicenses(id, individualId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = memberId, activeTabIndex });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteManufacturing(Guid id, Guid individualId, Guid memberId, int activeTabIndex)
        {

            try
            {
                await _individualRepoistory.DeleteIndividualManufacturing(id, individualId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = memberId, activeTabIndex });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteService(Guid id, Guid individualId, Guid memberId, int activeTabIndex)
        {
            try
            {
                await _individualRepoistory.DeleteIndividualService(id, individualId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = memberId, activeTabIndex });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTrade(Guid id, Guid individualId, Guid memberId, int activeTabIndex)
        {
            try
            {
                await _individualRepoistory.DeleteIndividualTrade(id, individualId, TheUser.Id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id = memberId, activeTabIndex });
        }
        private FileStream GetCurrentFile(string fileName)
        {
            var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            if (System.IO.File.Exists(tmpFileName))
            {
                return new FileStream(tmpFileName, FileMode.Open);
            }
            return null;
        }
        public async Task<IActionResult> UpdateMembershipIdInfo(IdCardInfo data, Guid id)
        {
            try
            {
                await _individualRepoistory.PrepareIdCard(data);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }

            return RedirectToAction("Detail", new { id, activeTabIndex = 0 });
        }

        [HttpGet]
        public async Task<IActionResult> PrintMembershipId(Guid id)
        {
            var data = new List<IdCardInfo>();
            var gd = Guid2String(id);

            var fileName = $"MembershipId_{gd}.pdf";
            var stream = GetCurrentFile(fileName);
            if (stream != null)
            {
                Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
                return File(stream, "application/pdf");
            }
            try
            {
                var tmp = await _membershipRepository.MembershipEnrollmentById(id, TheUser.Id);



                data.Add(tmp.IdCard);

                stream = GetReportStream<IdCardInfo>("MembershipId.frx", data, "Badge", fileName);
                return File(stream, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private FileStream GetReportStream<T>(string reportName, IEnumerable<T> data, string dataName, string fileName)
        {
            var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            FastReport.Report report = new();
            report.Load(Path.Combine(_hostingEnvironment.WebRootPath, "ReportTemplates", reportName));
            report.RegisterData(data, dataName);
            report.Prepare();
            var pdfExport = new PDFSimpleExport();
            pdfExport.Export(report, tmpFileName);
            report.Dispose();

            Response.Headers.Add("content-disposition", "inline;filename=" + fileName);
            return new FileStream(tmpFileName, FileMode.Open);
        }


        [HttpGet]
        public async Task<FileResult> DownLoadImportTemplate()
        {
            var fileName = "Member Import Template New.xlsx";
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "ReportTemplates", fileName);
            return await DownLoadFile(path, fileName);
        }
    }
}
