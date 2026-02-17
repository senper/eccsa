using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiServiceTypeController : BaseController
    {
        private readonly ILogger<ApiServiceTypeController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly ServiceTypeRepository _serviceTypeRepository;
        public ApiServiceTypeController(ILogger<ApiServiceTypeController> logger,
            UserManager<User> userManager,
            ServiceTypeRepository serviceTypeRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _serviceTypeRepository = serviceTypeRepository;
        }
        [HttpGet]
        [Route("owners")]
        public async Task<IActionResult> Owners()
        {
            var serviceOWners = await _serviceTypeRepository.ActiveServiceOwners();

            return Ok(serviceOWners);
        }
        [HttpGet]
        [Route("GetOwner/{id}")]
        public async Task<IActionResult> GetOwner(ECCSAModuleFlag id)
        {
            var serviceOWner = await _serviceTypeRepository.ServiceOwnerById(id);

            return Ok(serviceOWner);
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
        public async Task<IActionResult> Get(Guid id)
        {
            var serviceType = await _serviceTypeRepository.ServiceTypeWithRequiremnts(id);

            return Ok(serviceType);
        }
    }
}
