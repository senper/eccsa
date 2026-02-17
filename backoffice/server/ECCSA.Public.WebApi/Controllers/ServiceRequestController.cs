using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.PublicSecurity;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.Public.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ServiceRequestController : BaseController
    {
        private readonly ILogger<ServiceRequestController> _logger;
        private readonly ServiceRequestRepository _repository;
        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly ServiceTypeRepository _serviceTypeRepository;
        public ServiceRequestController(ILogger<ServiceRequestController> logger,
            UserManager<PublicUser> userManager, ServiceRequestRepository repository,
            ServiceTypeRepository serviceTypeRepository) : base(userManager)
        {
            _logger = logger;
            _serviceTypeRepository = serviceTypeRepository;
            _repository = repository;
        }
        private async Task<String> SaveDocument(Guid id, IFormFile file)
        {
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(Guid2String(id), extension);
            return await SaveFile(InvestmentDocumentAbsolutePath, newFileName, file);
        }
        [HttpPost]
        public async Task<IActionResult> Save(IFormCollection formCollection)
        {
            Guid id = Guid.Parse(formCollection["Id"]);
            Guid serviceTypeId = Guid.Parse(formCollection["ServiceTypeId"]);
            var serviceType = await _serviceTypeRepository.ServiceTypeById(serviceTypeId);
            ServiceRequest request = new ServiceRequest()
            {
                Id = Guid.Empty,
                ServiceTypeId = serviceTypeId,
                RequestingUserId = TheUser.Id,
            Responses = new List<ServiceRequirementResponse>()
            };
            foreach (var req in serviceType.ServiceRequirements)
            {
                var key = req.Id.ToString();
                var val = formCollection[key];
                var resp = new ServiceRequirementResponse
                {
                    Id = Guid.NewGuid(),
                    ServiceRequestId = Guid.Empty,
                    RequirementId = req.Id,
                };
                switch (req.InputMethod)
                {
                    case ServiceInputTypeEnum.Upload:
                        var file = formCollection.Files.FirstOrDefault(f => f.Name == key);
                        if (file != null)
                        {
                            var fileName = file.FileName;
                            await SaveDocument(resp.Id, file);
                            resp.TextResponse = Path.GetExtension(Path.GetFileName(fileName));
                        }
                            break;
                    case ServiceInputTypeEnum.Integer:
                        //  case ServiceInputTypeEnum.SingleSelect:
                        resp.IntResponse = int.Parse(val);
                        break;
                    case ServiceInputTypeEnum.Decimal:
                        resp.DecimalResponse = decimal.Parse(val);
                        break;
                    case ServiceInputTypeEnum.Date:
                        resp.DateResponse = DateTime.Parse(val);
                        break;
                    case ServiceInputTypeEnum.Email:
                    case ServiceInputTypeEnum.ShortText:
                    case ServiceInputTypeEnum.LongText:
                    case ServiceInputTypeEnum.Url:
                        resp.TextResponse = val;
                        break;
                }

                request.Responses.Add(resp);
            }
            await _repository.Save(request, TheUser.Id);
            return Ok(new
            {
                saved = true
            });
        }
        [HttpGet]
        public IActionResult Get()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var serviceRequests = _repository.UserServiceRequests(TheUser.Id);

            return Ok(serviceRequests);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRequest(Guid id)
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var serviceRequest = await _repository.UserServiceRequest(TheUser.Id, id);

            return Ok(serviceRequest);
        }
    }
}
