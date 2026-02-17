using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiGalleryController : BaseController
    {
        private readonly ILogger<ApiGalleryController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        static readonly string[] scopeRequiredByApi = new string[] { "Viewer" };
        private readonly GalleryRepository _repository;
        public ApiGalleryController(ILogger<ApiGalleryController> logger,
            UserManager<User> userManager,
            GalleryRepository repository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "ApiResource";
        }
        [HttpGet]
        public async Task<IActionResult> PhotoAlbums()
        {
            LogStart(_logger, controllerName, "PhotoAlbums");

            var Gallery = (await _repository.PhotoAlbumList()).ToList();
            return Ok(Gallery);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAlbumPhotos(Guid id)
        {
            var album = await _repository.AlbumById(id);
            var list = GetImages(album.Title);
            return Ok(list);
        }

        private List<string> GetImages(string title)
        {
            string filePath = Path.Combine(GalleryImagePath, title);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => Path.Combine(GalleryImagePathRelativePath, title,
                    (s.Split($"{title}\\").Length > 1 ? s.Split($"{title}\\")[s.Split($"{title}\\").Length - 1] :
                (s.Split($"{title}/").Length > 1 ? s.Split($"{title}/")[s.Split($"{title}/").Length - 1] : "")
                ))
                ).ToList();

                return lst;
            }
            return new List<string>();
        }

        [HttpGet]
        public async Task<IActionResult> VideoGalleries()
        {
            LogStart(_logger, controllerName, "VideoGalleries");

            var Gallery = (await _repository.VideoGalleryList()).ToList();
            return Ok(Gallery);
        }
    }
}








