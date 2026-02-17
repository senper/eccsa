using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiSearchController : Controller
    {
        private readonly ILogger<ApiLawsController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly BaseRepository _repository;
        public ApiSearchController(ILogger<ApiLawsController> logger,
           UserManager<User> userManager,
           BaseRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [Route("{keyword}")]

        public async Task<IActionResult> Search(string keyword)
        {
            var list = await _repository.GetSearchResults(keyword);
            return Ok(list);
        }

    }
}