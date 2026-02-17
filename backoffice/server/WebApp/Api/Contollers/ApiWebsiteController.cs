using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiWebsiteController : Controller
    {
        private readonly ILogger<ApiWebsiteController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly WebsiteRepository _repository;
        public ApiWebsiteController(ILogger<ApiWebsiteController> logger,
           UserManager<User> userManager,
           WebsiteRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Translations()
        {
            var list = await _repository.Translations();
            return Ok(list);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ById(string key)
        {
            var result = await _repository.Translate(key);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Menus()
        {
            var list = await _repository.MenusForWeb();
           
            return Ok(list);
        }
        //[HttpGet]
        //[Route("{id}")]
        //public async Task<IActionResult> MediaType(int id)
        //{
        //    var mediaType = await _repository.MediaType(id);
        //    return Ok(mediaType);
        //}

        [HttpGet]
        [Route("{url}")]
        public async Task<IActionResult> MediaTypeByUrl(string url)
        {
            var mediaType = await _repository.MediaTypeByUrl(url);
            return Ok(mediaType);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetContent(Guid id)
        {
            var content = await _repository.GetContent(id);
            return Ok(content);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> QuickLinkContents(int id)
        {
            var content = await _repository.QuickLinkContents(id);
            if(content == null)
            {
                var path = await _repository.GetQuicklinkMenuUrl(id);
                return Ok(new { isPath = true, path });
            }
            return Ok(content);
        }

        [HttpGet]
        public async Task<IActionResult> FooterMenu()
        {
            var list = await _repository.FooterMenuForWeb();
            return Ok(list);
        }
        

    }
}






