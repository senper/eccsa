using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{

    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiNewsController : Controller
    {
        private readonly NewsRepository _newsRepository;

        public ApiNewsController(UserManager<User> userManager,
           NewsRepository newsRepository) 
        {
            _newsRepository = newsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> LoadNews()
        {
            var news = (await _newsRepository.GetNews()).ToList();
            return Ok(news);
        }

        //api GetNewsById isnot needed and used in the front so its best to delete it
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetNewsById(Guid id)
        {
            var result = await _newsRepository.NewsById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> NewsHighlight()
        {
            var news = (await _newsRepository.TopFourNews()).ToList();
            return Ok(news);
        }

        [HttpGet]
        public async Task<IActionResult> NewsForSlider()
        {
            var news = (await _newsRepository.GetNews()).ToList();
            news = news.Where(s => s.HomeBannerSetted).OrderByDescending(i => i.NewsDate).Take(4).ToList();
            return Ok(news);
        }
    }
}






