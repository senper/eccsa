using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.PublicSecurity;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.Public.WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ECCSA.Public.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : BaseController
    {
        private ILogger<HomeController> _logger;
        AssociationRepository _associationRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly LegalDocumentTypeRepository _legalDocumentTypeRepository;
        private readonly LocationRepository _locationRepository;

        private readonly ApplicantRepository _applicantRepository;


        public HomeController(ILogger<HomeController> logger,
            UserManager<PublicUser> userManager,
            AssociationRepository associationRepository,
            LegalDocumentTypeRepository legalDocumentTypeRepository,
            LocationRepository locationRepository,
            InvoiceRepository invoiceRepository,
            ApplicantRepository applicantRepository) : base(userManager)
        {

            _logger = logger;
            _associationRepository = associationRepository;

            _locationRepository = locationRepository;



            _legalDocumentTypeRepository = legalDocumentTypeRepository;

            _invoiceRepository = invoiceRepository;
            _applicantRepository = applicantRepository;


        }



        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var userRoles = await _userManager.GetRolesAsync(user);
                return Ok(new PublicUserProfile
                {
                    Id = user.Id,
                    Email = user.Email,
                    Email_verified = false,
                    MemberId = user.MemberId,
                    Name = user.FullName,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetTree")]
        public async Task<JsonResult> GetTree()
        {
            var associations = await _associationRepository.List(TheUser.Id);
            var tview = TreeView.BuildTree(associations, Guid.Empty);
            return new JsonResult(tview);
        }

        [HttpGet]
        [Route("GetLocation")]
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
                pId = Guid.Parse("00000000-0000-0000-0000-0000000f4240");
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
            return new JsonResult(lst);
        }

        

        [HttpGet("{id}")]
        public async Task<IActionResult> ApplicantById([FromRoute]Guid id)
        {
            var data = await _applicantRepository.ApplicantById(id, TheUser.Id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveApplicant([FromBody]ApplicantViewModel m)
        {

            try
            {
                if (m.Id == Guid.Empty)
                {
                    var cai = new CreateApplicantInput(m.NameEng, m.NameLocal, DateTime.Today, m.TaxIdNumber, m.TradeRegistrationNumber, TheUser.Id, m.LocationId, m.AssociationId, 0, 0, "", "", "", MembershipWorkflowEnum.Started);
                    var ret = await _applicantRepository.Create(new GenericInput<CreateApplicantInput>(cai, TheUser.Id));
                    return CreatedAtAction("ApplicantById", new { id = ret.PayLoad.Id }, ret.PayLoad);

                }
                else
                {
                    var uai = new UpdateApplicantInput(m.Id, m.NameEng, m.NameLocal, DateTime.Today, m.TaxIdNumber, m.TradeRegistrationNumber, TheUser.Id, m.LocationId, m.AssociationId, 0, 0, "", "", "", MembershipWorkflowEnum.Started);
                    var ret = await _applicantRepository.Update(new GenericInput<UpdateApplicantInput>(uai, TheUser.Id));
                    return CreatedAtAction("ApplicantById", new { id = ret.PayLoad.Id }, ret.PayLoad);
                }
               

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Applicant Save");
                 
            }
            
            return BadRequest();
        }



        //[HttpGet]
        //public async Task<FileResult> DownLoadFile(Guid id, FolderTypeEnum type, string extension)
        //{
        //    var filePath = type == FolderTypeEnum.Document ? MembershipDocumentAbsolutePath : MembershipPhotoAbsolutePath;
        //    var fileName = String.Concat(Guid2String(id), extension);
        //    return await DownLoadFile(filePath, fileName);
        //}

        //[HttpGet]
        //public async Task<IActionResult> EditDocument(Guid id, Guid applicantId, Guid associationId)
        //{
        //    var userName = TheUser.FullName;

        //    var model = new ApplicantViewModel
        //    {
        //        FullName = userName,
        //        CurrentPage = "Applicants",
        //        CurrentOperation = "Detail",
        //        TheModel = new Applicant
        //        {
        //            Id = applicantId,
        //            AssociationId = associationId
        //        }
        //    };
        //    if (id == Guid.Empty)
        //    {
        //        model.Document = new ApplicantDocument
        //        {
        //            Id = Guid.Empty,
        //            ApplicantId = applicantId,

        //        };
        //    }
        //    else
        //    {
        //        model.Document = await _applicantRepository.ApplicantDocumentById(id);
        //    }
        //    model.DocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();
        //    model.ChangeActive(MenuItemEnum.MembershipApplicant);
        //    return View(model);

        //}


        //[HttpPost]
        //public async Task<IActionResult> SaveDocument(ApplicantViewModel model)
        //{
        //    var m = model.Document;
        //    string extension = string.Empty;
        //    if (model.Photo.Length > 0)
        //    {
        //        extension = Path.GetExtension(Path.GetFileName(model.Photo.FileName));
        //    }
        //    try
        //    {
        //        ApplicantDocumentPayload payload;
        //        if (m.Id != Guid.Empty)
        //        {
        //            var uldi = new UpdateApplicantDocumentInput(m.Id, m.ApplicantId, m.LegalDocumentTypeId, m.Document, extension);
        //            payload = await _applicantRepository.Update(new GenericInput<UpdateApplicantDocumentInput>(uldi, TheUser.Id));

        //            if (model.Photo.Length > 0)
        //            {
        //                var newFileName = String.Concat(Guid2String(payload.PayLoad.Id), extension);
        //                await SaveFile(MembershipDocumentAbsolutePath, newFileName, model.Photo);
        //            }
        //        }

        //        CommandStatus(CommandStatusEnum.Saved);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["CommandStatus"] = ex.Message;
        //    }

        //    return RedirectToAction("Detail", new { id = model.TheModel.Id });
        //}
    }
}
