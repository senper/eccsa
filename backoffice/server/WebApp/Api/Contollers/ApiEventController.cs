using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Events;
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
    [Route("api/[controller]/[action]")]
    public class ApiEventController : BaseController
    {
        private readonly ILogger<ApiEventController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly EventRepository _eventRepository;
        public ApiEventController(ILogger<ApiEventController> logger,
            UserManager<User> userManager,
            EventRepository eventRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Events()
        {
            var events = (await _eventRepository.PublicEvents()).ToList();
            return Ok(events);
        }
        [HttpGet]
        public async Task<IActionResult> PastEvents()
        {
            var events = (await _eventRepository.PublicPastEvents()).OrderBy(s => s.Title).ToList();
            return Ok(events);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var result = await _eventRepository.EventById(id);
            return Ok(result);
        }
    }
}
