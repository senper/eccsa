using ECCSA.BackOffice.Server.Model.BusinessDirectory;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiBusinessInterestController : BaseController
    {
        private readonly ILogger<ApiBusinessInterestController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly BusinessInterestRepository _businessInterestRepository;
        public ApiBusinessInterestController(ILogger<ApiBusinessInterestController> logger,
            UserManager<User> userManager, BusinessInterestRepository businessInterestRepository,
        PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _businessInterestRepository = businessInterestRepository;
        }
        [HttpGet]
        [Route("{parentsOnly}")]
        public async Task<IActionResult> Sectors(bool parentsOnly = false)
        {
            try
            {
                var sectors = await _businessInterestRepository.SectorsWithSources(parentsOnly);
                var counts = await _businessInterestRepository.SourcesBySectorCount();
                return Ok(new { sectors, counts });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{sectorId}")]
        public async Task<IActionResult> GetSources(Guid sectorId)
        {
            try
            {
                var list = await _businessInterestRepository.BusinessInterestSourcesForPublic(sectorId); 
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{bussinessType}/{key}")]
        public async Task<IActionResult> SearchSources(Guid bussinessType, string key)
        {
            try
            {
                var list = await _businessInterestRepository.BusinessInterestSourcesForPublic(bussinessType, key);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSourceDetail(Guid id)
        {
            try
            {
                var list = await _businessInterestRepository.BusinessInterestSourceForPublic(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveInterest(Guid id)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _businessInterestRepository.SaveInterest(id, TheUser.Id);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Match Making Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveInterest(Guid id)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _businessInterestRepository.RemoveInterest(id, TheUser.Id);
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
        [HttpPost]
        [Route("{id}/{remark}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SetInterestRemark(Guid id, string remark)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _businessInterestRepository.SetUserInterestRemark(id, TheUser.Id, remark);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UserMatches()
        {
            try
            {
                if (TheUser != null)
                {
                    var list = await _businessInterestRepository.UserMatches(TheUser.Id);
                    return Ok(list);
                }
                 else
                {
                    return Ok(new List<MatchMaking>());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserMatchSources()
        {
            try
            {
                var list = await _businessInterestRepository.UserMatchSources(TheUser.Id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
