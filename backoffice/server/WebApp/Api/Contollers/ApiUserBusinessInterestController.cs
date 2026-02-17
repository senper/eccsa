using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Api.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiUserBusinessInterestController : BaseController
    {
        private readonly ILogger<ApiUserBusinessInterestController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly BusinessInterestRepository _businessInterestRepository;
        private readonly BusinessRepository _businessrepository;
        public ApiUserBusinessInterestController(ILogger<ApiUserBusinessInterestController> logger,
            UserManager<User> userManager, BusinessInterestRepository businessInterestRepository,
            BusinessRepository businessrepository, 
        PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _businessInterestRepository = businessInterestRepository;
            _businessrepository = businessrepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRootSectors()
        {
            try
            {
                var list = (await _businessrepository.RootSectors()).OrderBy(s => s.Name).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ChildSectors(Guid id)
        {
            try
            {
                var list = (await _businessrepository.ChildSectors(id)).OrderBy(s => s.Name).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetBusinessInterestProducts()
        {
            try
            {
                var list = (await _businessInterestRepository.BusinessInterestProducts()).OrderBy(s => s.Name).ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveBusinessInterest(ApiUserBusinessInterestModel data)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _businessInterestRepository.SaveUserBusinessInterest(data.ToBusinessInterestSource(), TheUser.Id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Match Removed");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetUserBusinessInterest()
        {
            try
            {
                var data = await _businessInterestRepository.GetUserBusinessInterest(TheUser.Id);
                return Ok(ApiUserBusinessInterestModel.FromBusinessInterestSource(data));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> RemoveBusinessInterest()
        {
            try
            {
                await _businessInterestRepository.RemoveUserBusinessInterest(TheUser.Id);
                return Ok(new
                {
                    saved = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
