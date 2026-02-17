using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiLawsController : Controller
    {
        private readonly ILogger<ApiLawsController> _logger;
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly LawsRepository _lawsRepository;
        public ApiLawsController(ILogger<ApiLawsController> logger,
           UserManager<User> userManager,
           LawsRepository lawsRepository)
        {
            _logger = logger;
            _lawsRepository = lawsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List(LawCategoryEnum category, LawTypeEnum lawType = LawTypeEnum.Proclamation)
        {
            var list = await _lawsRepository.List(category, lawType);
            return Ok(list);
        }

        private FileStream GetCurrentFile(string fileName, string outFolder)
        {
            //var outFolder = Path.GetTempPath();
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            var tmpFileName = Path.Combine(outFolder, fileName);
            if (System.IO.File.Exists(tmpFileName))
            {
                return new FileStream(tmpFileName, FileMode.Open);
            }
            return null;
        }


        [HttpGet]
        [Route("{id}/{isLocal}")]
        public async Task<IActionResult> Download(Guid id, bool isLocal)
        {
            try
            {
                var request = await _lawsRepository.ById(id);
                if (request == null)
                {
                    return NotFound();
                }
                var gd = id.ToString()[^15..].Replace("-", "");
                var fileName = $"{gd}{(isLocal && !string.IsNullOrWhiteSpace(request.ExtensionLocal) ? "_AMH_" + request.ExtensionLocal : request.Extension)}";
                var stream = GetCurrentFile(fileName, Path.Combine("wwwroot", "pr", "documents"));
                if (stream != null)
                {
                    Response.Headers.Add("content-disposition",
                        "inline;filename=" + $"laws{(isLocal && !string.IsNullOrWhiteSpace(request.ExtensionLocal) ? request.ExtensionLocal : request.Extension)}");
                    return File(stream, $"application/{(isLocal && !string.IsNullOrWhiteSpace(request.ExtensionLocal) ? request.ExtensionLocal : request.Extension).Replace(".", "")}");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to download document.");
            }
        }
    }
}






