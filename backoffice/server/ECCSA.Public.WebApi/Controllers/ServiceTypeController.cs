using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.PublicSecurity;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.Public.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTypeController : BaseController
    {
        private readonly ILogger<ServiceTypeController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly ServiceTypeRepository _serviceTypeRepository;
        public ServiceTypeController(ILogger<ServiceTypeController> logger,
            UserManager<PublicUser> userManager,
            ServiceTypeRepository serviceTypeRepository) : base(userManager)
        {
            _logger = logger;
            _serviceTypeRepository = serviceTypeRepository;
        }
        [HttpGet]
        [Route("owners")]
        public IActionResult Owners()
        {
            var serviceOWners = _serviceTypeRepository.ServiceOwners().OrderBy(s => s.Title);

            return Ok(serviceOWners);
        }
        [HttpGet]
        public IActionResult Get()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var serviceTypes = _serviceTypeRepository.ServiceTypesWithRequiremnts().OrderBy(s => s.Name);

            return Ok(serviceTypes);
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            var serviceType = _serviceTypeRepository.ServiceTypeWithRequiremnts(id);

            return Ok(serviceType);
        }
    }
}
