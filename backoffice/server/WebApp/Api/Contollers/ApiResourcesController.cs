using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Advocacy;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiResourcesController : BaseController
    {
        private readonly ILogger<ApiResourcesController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly ResourceRepository _resourceRepository;
        private readonly AdvocacyRepository _advocacyRepository;
        public ApiResourcesController(ILogger<ApiResourcesController> logger,
            UserManager<User> userManager,
            ResourceRepository resourceRepository,
            AdvocacyRepository advocacyRepository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _resourceRepository = resourceRepository;
            _advocacyRepository = advocacyRepository;
            controllerName = "ApiResource";
        }
        [HttpGet]
        public async Task<IActionResult> Resources(int type = 1)
        {
            LogStart(_logger, controllerName, "Resources");

            var resources = (await _resourceRepository.ResourcesForApi()).Where(s => (int)s.Type == type).ToList();
            return Ok(resources);
        }

        [HttpGet]
        public async Task<IActionResult> ResourcesByTypeId(Guid typeId)
        {
            LogStart(_logger, controllerName, "Resources");

            var resources = (await _resourceRepository.ResourcesForApi(typeId)).OrderByDescending(s => s.PublicationDate).ToList();
            return Ok(resources);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetResource(Guid id)
        {
            var result = await _resourceRepository.ByIdForApi(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> PPDialogCategories()
        {
            LogStart(_logger, controllerName, "PPDialogCategories");

            var dialogs = (await _advocacyRepository.DialogForPublic())
                .Select(s => new { s.Id, s.Title, s.Description });
            return Ok(dialogs);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Dialog(Guid id)
        {
            var result = await _advocacyRepository.DialogByIdForPublic(id);
            return Ok(result);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> DownloadPPDialogAttachment(Guid id)
        {
            try
            {
                var request = await _advocacyRepository.GetAttachment(id);
                if (request == null)
                {
                    return NotFound();
                }
                var gd = id.ToString()[^15..].Replace("-", "");
                var fileName = $"{gd}{request.Extension}";
                return await base.DownLoadFile(Path.Combine(AbsolutePath(ECCSAModuleFlag.Research), fileName), request.Title + request.Extension);
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to download Attachment");
            }
        }
    }
}








