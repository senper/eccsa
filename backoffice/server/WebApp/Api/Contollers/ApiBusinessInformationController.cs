using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.BusinessDirectory;
using ECCSA.BackOffice.Server.Persistence.Repositories.Investment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
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
    public class ApiBusinessInformationController : BaseController
    {
        private readonly ILogger<ApiBusinessInformationController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly BusinessInformationRepository _businessInformationRepository;
        private readonly BusinessRepository _businessRepository;
        private readonly InvestmentRepository _investmentRepository;
        public ApiBusinessInformationController(ILogger<ApiBusinessInformationController> logger,
            UserManager<User> userManager,
            BusinessInformationRepository businessInformationRepository,
            BusinessRepository businessRepository,
            InvestmentRepository investmentRepository,
        PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _businessInformationRepository = businessInformationRepository;
            _businessRepository = businessRepository;
            _investmentRepository = investmentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetInvestmentProfilesList()
        {
            var et = await _investmentRepository.Ethiopia();
            var regions = await _investmentRepository.Regions();
            var list = regions.OrderBy(s => s.Name).Select(s => new { s.Id, s.Name, s.NameLocal, IsRegion = true }).ToList();
            var sectors = await _investmentRepository.Sectors();
            var slist = sectors.OrderBy(s => s.Name).Select(s => new { s.Id, s.Name, s.NameLocal, IsRegion = false }).ToList();
            list.Add(new { et.Id, et.Name, et.NameLocal, et.IsRegion });
            list.AddRange(slist);

            return Ok(list);

        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetInvestmentProfile(Guid id)
        {
            var entity = await _investmentRepository.RegionSectorById(id);
            return Ok(entity);
        }
        [HttpGet]
        public async Task<IActionResult> BusinessInformationList()
        {
            var businessInformations = (await _businessInformationRepository.ActiveBusinessInformationList()).ToList();
            return Ok(businessInformations);
        }
        [HttpGet]
        public async Task<IActionResult> PastBusinessInformationList()
        {
            var businessInformations = (await _businessInformationRepository.PastBusinessInformationList()).ToList();
            return Ok(businessInformations);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBusinessInformation(Guid id)
        {
            var result = await _businessInformationRepository.BusinessInformation(id);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Sectors()
        {
            try
            {
                var sectors = await _businessRepository.SectorsWithCompany(); // await _yesNoRepository.LookupTypeById(new Guid("00000000-0000-0000-0000-000000000002"));
                var counts = await _businessRepository.CompaniesBySectorCount();
                return Ok(new { sectors, counts });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> SectorCompanies(Guid id)
        {
            try
            {
                var list = await _businessRepository.CompaniesBySector(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> CompaniesByKey(string key)
        {
            try
            {
                var list = await _businessRepository.CompaniesByKey(key);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
