using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Website;
using ECCSA.BackOffice.Server.WebApp.Areas.Communication.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using ECCSA.BackOffice.Server.Persistence.Repositories.Communication;
using ECCSA.BackOffice.Server.Persistence.Repositories.Investment;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;

namespace ECCSA.BackOffice.Server.WebApp.Controllers
{
    public class FileScrapperController : BaseController
    {
        private readonly ILogger<NewsController> _logger;
        private readonly NewsRepository _repository;
        private readonly LawsRepository _lawsRepository;
        private readonly GalleryRepository _galleryRepository;
        private readonly InvestmentRepository _profileRepository;
        private readonly ResourceRepository _resourceRepository;
        public FileScrapperController(
            UserManager<User> userManager
            , NewsRepository repository
            , LawsRepository lawsRepository
             , GalleryRepository galleryRepository
            , PublicUserRepository userRepository
            , InvestmentRepository profileRepository
            , ResourceRepository resourceRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _repository = repository;
            _lawsRepository = lawsRepository;
            _galleryRepository = galleryRepository;
            _resourceRepository = resourceRepository;
            _profileRepository = profileRepository;
        }

        public async Task<IActionResult> Index()
        {
            //    await ScrapNews();
            var itemList = await _lawsRepository.List();
            var resourceList = await _resourceRepository.Resources(ECCSAModuleFlag.Communication);
            ScrapLaws(itemList.OrderBy(i=> i.Title).ToList());
            ScrapResources(resourceList.OrderBy(i => i.Title).ToList());
            return View();
        }
        private async Task ScrapProfile()
        {
            var profileList = await _profileRepository.RegionSectors();
            //foreach (var item in profileList)
            //{
            //    var url = GetUrl(item.Id, ECCSAModuleFlag.Communication, item.Extension);
            //    var localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Communication, item.Extension);
            //    Scrap(url, localPath);
            //    url = GetUrl(item.Id, ECCSAModuleFlag.Communication, item.Extension, "home");
            //    localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Communication, item.Extension, "home");
            //    Scrap(url, localPath);
            //}
        }
        private async Task ScrapNews()
        {
            var newsList = await _repository.GetNews();
            foreach (var item in newsList)
            {
                var url = GetUrl(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                var localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                Scrap(url, localPath);
                url = GetUrl(item.Id, ECCSAModuleFlag.Communication, item.Extension, "home");
                localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Communication, item.Extension, "home");
                Scrap(url, localPath);
            }
        }
        private void ScrapResources(List<Model.Communication.Resource> itemList)
        {

            var engFiles = Directory.EnumerateFiles(@"C:\wwwroot\Resources").ToList();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in itemList)
            {
                var cleanTitle = item.Title.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_");
                var shortId = Guid2String(item.Id);
                var engExists = engFiles.Exists(i => i.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_").Contains(cleanTitle));
                var engFileName = string.Empty;
                var fileName = string.Empty;
                var newFileName = string.Empty;
                var newFileName2 = string.Empty;
               
                if (engExists)
                {
                    fileName = engFiles.FirstOrDefault(f => f.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_").Contains(cleanTitle));
                    engFileName = fileName.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_").Replace(cleanTitle, $"{shortId}");
                    engFileName = engFileName.Replace(@"\eng", "");
                    try
                    {
                        System.IO.File.Copy(fileName, engFileName);
                        newFileName = fileName.Replace(@"C:\wwwroot\Resources", @"C:\wwwroot\Resources\moved");
                        newFileName2 = engFileName.Replace(@"C:\wwwroot\Resources", @"C:\wwwroot\Resources\new");
                        System.IO.File.Move(fileName, newFileName);
                        System.IO.File.Move(engFileName, newFileName2);
                    }
                    catch { }
                }
                else
                {
                    stringBuilder.AppendLine(item.Title);
                }
               

                //var url = GetUrl(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                //var localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                //Scrap(url, localPath);
            }
            System.IO.File.WriteAllText(@"c:\wwwroot\resources.txt", stringBuilder.ToString());
        }
        private void ScrapLaws(List<Model.Website.Law> itemList)
        {
           
            var engFiles = Directory.EnumerateFiles(@"C:\wwwroot\laws\eng").ToList();
            var amhFiles = Directory.EnumerateFiles(@"C:\wwwroot\laws\amh").ToList();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in itemList)
            {
                var cleanTitle = item.Title.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_");
                var cleanTitleAmh = string.IsNullOrEmpty(item.TitleLocal) ? cleanTitle : item.TitleLocal.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_");
                var shortId = Guid2String(item.Id);
                var engExists = engFiles.Exists(i => i.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_").Contains(cleanTitle));
                var amhExists = amhFiles.Exists(i => i.Trim().Replace(" ", "").Replace("(", "_").Replace(")", "_").Contains(cleanTitleAmh));
                var engFileName = string.Empty;
                var amhFileName = string.Empty;
                var fileName = string.Empty;
                var newFileName = string.Empty;
                if (engExists )
                {
                    fileName = engFiles.FirstOrDefault(f => f.Replace(" ", "").Replace("(", "_").Replace(")", "_").Contains(cleanTitle));
                    engFileName = fileName.Replace(item.Title, shortId);
                    engFileName = engFileName.Replace(@"\eng", "");
                    try
                    {
                        System.IO.File.Copy(fileName, engFileName);
                        newFileName = fileName.Replace(@"\laws\eng", @"\laws\moved\eng");
                        System.IO.File.Move(fileName, newFileName);
                    }
                    catch { }
                }
               
                if (amhExists)
                {
                    fileName = amhFiles.FirstOrDefault(f => f.Replace(" ", "").Replace("(", "_").Replace(")", "_").Contains(cleanTitleAmh));
                    amhFileName = String.IsNullOrEmpty(engFileName) ? fileName.Replace(" ", "").Replace("(", "_").Replace(")", "_").Replace(cleanTitleAmh, shortId) : fileName.Replace(" ", "").Replace("(", "_").Replace(")", "_").Replace(cleanTitleAmh, $"{shortId}_AMH_");
                    amhFileName = amhFileName.Replace(@"\amh", "");
                    try
                    {
                        System.IO.File.Copy(fileName, amhFileName);
                        newFileName = fileName.Replace(@"\laws\amh", @"\laws\moved\amh");
                        System.IO.File.Move(fileName, newFileName);
                    }
                    catch { }
                }
                if( string.IsNullOrEmpty(engFileName) && string.IsNullOrEmpty(amhFileName))
                    stringBuilder.AppendLine(item.Title);
                //var url = GetUrl(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                //var localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                //Scrap(url, localPath);
            }
            System.IO.File.WriteAllText(@"c:\wwwroot\laws.txt", stringBuilder.ToString());
        }
        private async Task ScrapResources()
        {
            var itemList = await _resourceRepository.Resources(ECCSAModuleFlag.Communication);
            foreach (var item in itemList)
            {
                var url = GetUrl(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                var localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Communication, item.Extension);
                Scrap(url, localPath);
            }
            itemList = await _resourceRepository.Resources(ECCSAModuleFlag.Research);
            foreach (var item in itemList)
            {
                var url = GetUrl(item.Id, ECCSAModuleFlag.Research, item.Extension);
                var localPath = FullLocalPath(item.Id, ECCSAModuleFlag.Research, item.Extension);
                Scrap(url, localPath);
            }
        }
        private void Scrap(string url, string localName)
        {
            if (System.IO.File.Exists(localName)) return;
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(url, localName);
                }
            }
            catch { }
        }
        private string GetUrl(Guid entityId, ECCSAModuleFlag module,string extension)
        {
            var id = Guid2String(entityId);
            var relativePath = RelativePath(module);
            var url = $"https://ethiopianchamber.com{relativePath}/{id}{extension}";
            return url;
        }
        private string GetUrl(Guid entityId, string path, string extension)
        {
           
            var id = Guid2String(entityId);
            var url = $"https://ethiopianchamber.com{path}/{id}{extension}";
            return url;
        }
        private string GetUrl(Guid entityId, ECCSAModuleFlag module, string extension, string subFolder)
        {
            if(string.IsNullOrEmpty(subFolder))
                return GetUrl(entityId, module, extension);
            var id = Guid2String(entityId);
            var relativePath = RelativePath(module);
            var url = $"https://ethiopianchamber.com{relativePath}/{subFolder}/{id}{extension}";
            return url;
        }
        private string FullLocalPath(Guid entityId, string path, string extension)
        {
            var id = Guid2String(entityId);
            var newFileName = Path.Combine(path, id);
            return newFileName + extension;
        }
        private string FullLocalPath(Guid entityId, ECCSAModuleFlag module, string extension)
        {
            var id = Guid2String(entityId);
            var absolutePath = AbsolutePath(module);
            var newFileName = Path.Combine(absolutePath, id);
            return newFileName+ extension;
        }
        private string FullLocalPath(Guid entityId, ECCSAModuleFlag module, string extension, string subFolder)
        {
            if (string.IsNullOrEmpty(subFolder))
                return FullLocalPath(entityId, module, extension);
            var id = Guid2String(entityId);
            var absolutePath = AbsolutePath(module);
            var newFileName = Path.Combine(absolutePath,subFolder, id);
            return newFileName + extension;
        }
        private List<string> GetProfileImages(Guid profileId)
        {
            var id = Guid2String(profileId);
            string filePath = Path.Combine(ProfileImagePath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{ProfileRelativePath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }

        private List<string> GetProfileAttachments(Guid profileId)
        {
            var id = Guid2String(profileId);
            string filePath = Path.Combine(ProfileAttachmentPath, id);
            if (Directory.Exists(filePath))
            {
                var lst = Directory.GetFiles(filePath).Select(s => $"{ProfileRelativeAttachmentPath}{id}/" +
                (s.Split($"{id}\\").Length > 1 ? s.Split($"{id}\\")[1] :
                (s.Split($"{id}/").Length > 1 ? s.Split($"{id}/")[1] : "")
                )).ToList();

                return lst;
            }
            return new List<string>();
        }

    }
}
