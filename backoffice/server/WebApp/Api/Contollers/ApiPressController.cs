using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using System.Linq;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiPressController : BaseController
    {
        private readonly ILogger<ApiPressController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly MediaRepository _mediaRepository;
        public ApiPressController(ILogger<ApiPressController> logger,
           UserManager<User> userManager,
           MediaRepository mediaRepository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _mediaRepository = mediaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List(int mediaType)
        {
            var list = await _mediaRepository.List(mediaType);
            return Ok(list.OrderByDescending(s => s.DateCreated));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ById(Guid id)
        {
            var result = await _mediaRepository.View(id);
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("{id}/{text}")]
        public async Task<IActionResult> AddComment(Guid id, string text)
        {
            if(TheUser == null)
            {
                return BadRequest("Invalid User");
            }
            var data = new Comment
            {
                Id = Guid.NewGuid().ToString(),
                Text = text,
                Author = TheUser.Id,
                AuthorName = TheUser.FullName,
                DateCreated = DateTime.Now,
            };
            await _mediaRepository.AddComment(id, data);
            return Ok(data);
        }
    }
}





