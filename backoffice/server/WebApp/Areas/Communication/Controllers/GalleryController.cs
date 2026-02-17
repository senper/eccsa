using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers
{
    [Area("Communication")]
    public class GalleryController : BaseController
    {
        private readonly ILogger<GalleryController> _logger;
        private readonly GalleryRepository _repository;
        public GalleryController(
            UserManager<User> userManager
            , ILogger<GalleryController> logger
            , GalleryRepository repository
            , PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
            controllerName = "Gallery";
        }
        [HttpGet]
        public async Task<IActionResult> PhotoAlbums()
        {
            var model = new PhotoAlbumViewModel();
            LogStart(_logger, controllerName, "PhotoAlbums");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "PhotoAlbums");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.PhotoAlbumList();
            }
            catch (Exception ex)
            {
                model.TheList = new List<PhotoAlbum>();
                _logger.LogError(ex, "PhotoAlbums");
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Photo Albums";
            model.CurrentOperation = "List";
            model.CurrentModule = ECCSAModuleFlag.Communication;
            model.ChangeActive(MenuItemEnum.CommunicationPhotoAlbum);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "PhotoAlbums");

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> PhotoAlbumDetail(Guid id)
        {
            LogStart(_logger, controllerName, "PhotoAlbumDetail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "PhotoAlbumDetail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new PhotoAlbumViewModel();
            try
            {
                model.TheModel = await _repository.AlbumById(id);
                model.CurrentModule = ECCSAModuleFlag.Communication;
                model.ProfileImages = GetImages(model.TheModel.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Photo Album Detail Exception {id}");
            }
            if (model.TheModel == null)
            {
                return RedirectToAction("Error", "Home", new { area = "" });
            }
            model.CurrentPage = "Photo Album";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.CommunicationPhotoAlbum);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "PhotoAlbumDetail");
            return View(model);
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
        [HttpPost]
        public async Task<IActionResult> SavePhotoAlbum(PhotoAlbum model)
        {
            var id = model.Id;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SavePhotoAlbum");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("PhotoAlbums");
            }
            try
            {
                await _repository.SaveAlbum(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SavePhotoAlbum", ex, model);
                CommandStatus(ex.Message);
                return RedirectToAction("PhotoAlbums");
            }
            catch (Exception ex)
            {
                _logger.LogError("SavePhotoAlbum", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
                return RedirectToAction("PhotoAlbums");
            }
            if (id == Guid.Empty)
                return RedirectToAction("PhotoAlbums");
            else
                return RedirectToAction("PhotoAlbumDetail", new { id = model.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAlbum(Guid id)
        {
            LogStart(_logger, controllerName, "DeleteAlbum");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteAlbum");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("PhotoAlbums");
            }
            try
            {
                var album = await _repository.AlbumById(id);
                string filePath = Path.Combine(GalleryImagePath, album.Title);
                DirectoryInfo di = new(filePath);
                if (di.Exists)
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    di.Delete(true);
                }
                await _repository.DeleteAlbum(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteAlbum", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteAlbum", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("PhotoAlbums");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(Guid id, List<IFormFile> image)
        {
            try
            {
                var album = await _repository.AlbumById(id);
                string filePath = Path.Combine(GalleryImagePath, album.Title);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                foreach (var im in image)
                {
                    await SaveFile(filePath, im.FileName, im);
                }
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (Exception ex)
            {
                TempData["CommandStatus"] = ex.Message;
            }
            return RedirectToAction("PhotoAlbumDetail", new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveImage(Guid id, string fileName)
        {
            string filePath = Path.Combine(FileRoot, fileName);
            try
            {
                System.IO.File.Delete(Path.Combine(filePath));
            }
            catch (Exception ex) { TempData["CommandStatus"] = ex.Message; }

            CommandStatus(CommandStatusEnum.Deleted);
            return RedirectToAction("PhotoAlbumDetail", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> VideoGalleries()
        {
            var model = new VideoGalleryViewModel();
            LogStart(_logger, controllerName, "VideoGalleries");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "VideoGalleries");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            try
            {
                model.TheList = await _repository.VideoGalleryList();
            }
            catch (Exception ex)
            {
                model.TheList = new List<VideoGallery>();
                _logger.LogError(ex, "VideoGalleries");
            }
            model.CurrentPage = "Video Galleries";
            model.CurrentOperation = "List";
            model.CurrentModule = ECCSAModuleFlag.Communication;
            model.ChangeActive(MenuItemEnum.CommunicationVideoGallery);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "VideoGalleries");

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> VideoGalleryDetail(Guid id)
        {
            LogStart(_logger, controllerName, "VideoGalleryDetail");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.None)
            {
                LogSecurityError(_logger, controllerName, "VideoGalleryDetail");
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            var model = new VideoGalleryViewModel();
            try
            {
                model.TheModel = await _repository.VideoGalleryById(id);
                model.CurrentModule = ECCSAModuleFlag.Communication;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Video Gallery Detail Exception {id}");
            }

            model.CurrentPage = "Video Gallery";
            model.CurrentOperation = "Detail";
            model.ChangeActive(MenuItemEnum.CommunicationVideoGallery);
            model.RoleLevel = roleLevel;
            LogEnd(_logger, controllerName, "VideoGalleryDetail");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveVideoGallery(VideoGallery model)
        {
            var id = model.Id;
            LogStart(_logger, controllerName, "Save");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "SaveVideoGallery");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("PhotoAlbums");
            }
            try
            {
                await _repository.SaveVideoGallery(model);
                CommandStatus(CommandStatusEnum.Saved);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("SaveVideoGallery", ex, model);
                CommandStatus(ex.Message);
                return RedirectToAction("VideoGalleries");
            }
            catch (Exception ex)
            {
                _logger.LogError("SaveVideoGallery", ex, model);
                CommandStatus(CommandStatusEnum.UnkownError);
                return RedirectToAction("VideoGalleries");
            }
            if (id == Guid.Empty)
                return RedirectToAction("VideoGalleries");
            else
                return RedirectToAction("VideoGalleryDetail", new { id = model.Id });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteVideoGallery(Guid id)
        {
            LogStart(_logger, controllerName, "DeleteVideoGallery");
            var roleLevel = await GetRoleLevel(ECCSAModuleFlag.Communication);
            if (roleLevel <= RoleLevelEnum.View)
            {
                LogSecurityError(_logger, controllerName, "DeleteVideoGallery");
                CommandStatus(CommandStatusEnum.InsufficientPrevilage);
                return RedirectToAction("VideoGalleries");
            }
            try
            {
                await _repository.DeleteVideoGallery(id);
                CommandStatus(CommandStatusEnum.Deleted);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("DeleteVideoGallery", ex, id);
                CommandStatus(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("DeleteVideoGallery", ex, id);
                CommandStatus(CommandStatusEnum.UnkownError);
            }

            return RedirectToAction("VideoGalleries");
        }
    }
}
