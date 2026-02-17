using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Api.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
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

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiContactUsController : BaseController
    {
        private readonly ILogger<ApiContactUsController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly YesNoRepository _yesNoRepository;
        private readonly IssueRepository _issueRepository;
        public ApiContactUsController(ILogger<ApiContactUsController> logger,
            UserManager<User> userManager,
            IssueRepository issueRepository, YesNoRepository yesNoRepository,
        PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _issueRepository = issueRepository;
            _yesNoRepository = yesNoRepository;
        }
        [HttpGet]
        public async Task<IActionResult> IssueTypes()
        {
            try
            {
                var issueTypes = await _issueRepository.IssueTypes();
                return Ok(issueTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveContactUs(ContactUsModel data)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _issueRepository.SaveContactUs(new ContactUs
                {
                    Name = data.Name,
                    Message = data.Message,
                    Date = DateTime.UtcNow,
                    Email = data.Email,
                });
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Issue Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpPost]
        public async Task<IActionResult> SaveIssue(IFormCollection formCollection)
        {
            decimal.TryParse(formCollection["ProductValueAmount"], out decimal amt);
            DateTime date = DateTime.Today;
            DateTime.TryParse(formCollection["dateOfOccurrence"], out date);
            var locationIsSet = Guid.TryParse(formCollection["locationId"], out Guid locationId);
            Issue data = new()
            {
                Id = Guid.NewGuid(),
                TypeId = Guid.Parse(formCollection["typeId"]),
                CompanyName = formCollection["companyName"],
                DateOfOccurrence = date,
                DetailsOfOccurrence = formCollection["detailsOfOccurrence"],
                MeasureCode = formCollection["measureCode"],
                PlaceOfOccurrence = formCollection["placeOfOccurrence"],
                CountryRegion = formCollection["countryRegion"],
                TypeOfLocationId = Guid.Parse(formCollection["typeOfLocationId"]),
                LocationId = locationIsSet ? locationId : null,
                OtherLocation = formCollection["otherLocation"],
                Description = formCollection["description"],
                ProductTariffCode = formCollection["productTariffCode"],
                ProductValueCurrency = formCollection["productValueCurrency"],
                ProductValueAmount = amt,
                DescriptionOfProducts = formCollection["descriptionOfProducts"],
                PreferredHandlerId = Guid.Parse(formCollection["preferredHandlerId"]),
                IssueDate = DateTime.Now,
                AffectedProducts = formCollection["affectedProducts"],
                Status = IssueStatus.New,
                Attachments = new List<IssueAttachment>()
            };
            var message = string.Empty;
            var isValid = true;
            var issueTypes = await _issueRepository.IssueTypes();
            if (!issueTypes.Any(s => s.Id == data.TypeId))
            {
                isValid = false;
                message = "Invalid Type";
            }
            else
            {
                try
                {
                    await PopulateAttachment(formCollection, data);
                    await _issueRepository.Save(data, Guid.Empty);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    isValid = false;
                    _logger.LogError(ex, "Issue Save");
                }
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }

        private async Task PopulateAttachment(IFormCollection formCollection, Issue issue)
        {
            if (!string.IsNullOrEmpty(formCollection["AttachmentTitle"]))
            {
                foreach (var title in formCollection["AttachmentTitle"])
                {
                    if (string.IsNullOrEmpty(title)) continue;
                    IssueAttachment attachment = new()
                    {
                        Id = Guid.NewGuid(),
                        Title = title,
                    };
                    var file = formCollection.Files.FirstOrDefault(f => f.Name == title);
                    if (file != null)
                    {
                        string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                        attachment.Extension = extension;
                        var shortName = Guid2String(attachment.Id);
                        var newFileName = String.Concat(shortName, extension);
                        await SaveFile(IssuesAbsolutePath, newFileName, file, shortName);
                    }
                    issue.Attachments.Add(attachment);
                }
            }
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            try
            {
                return Ok(Country.Countries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetLookupValues(Guid id)
        {
            try
            {
                var type = await _yesNoRepository.LookupTypeById(id);
                return Ok(type.LookupValues);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{country}")]
        public async Task<IActionResult> GetIssueLocations(string country)
        {
            try
            {
                var locations = await _issueRepository.IssueLocations(country);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetIssuesSummary(DateTime? date = null)
        {
            try
            {
                date ??= DateTime.UtcNow.Date.AddDays(-30);
                var list = await _issueRepository.IssuesSummary(date.Value);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
