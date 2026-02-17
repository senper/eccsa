using Microsoft.AspNetCore.Mvc;
using System;
using ClosedXML.Excel;
using ECCSA.BackOffice.Server.Model.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Net.Mime;

using System.Xml;
using System.ServiceModel.Syndication;
using System.Collections.Generic;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Model.Common;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Linq;
using System.Data;
using ECCSA.BackOffice.Server.Model.ExternalData;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.PublicSecurity;
using System.Net.Mail;
using ECCSA.BackOffice.Server.WebApp.Settings;

namespace ECCSA.BackOffice.Server.WebApp.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected string controllerName;
        public readonly Guid SupportingUnitLookupTypeId = Persistence.Repositories.Constants.SupportingUnitLookupTypeId;
        public static readonly string MembershipUserRole = "Membership User";
        public static readonly string MembershipAdminRole = "Membership Admin";
        public static readonly string ViewerRole = "Viewer";
        public static readonly string ArbitrationUserRole = "Arbitration User";
        public static readonly string ArbitrationAdminRole = "Arbitration Admin";
        public static readonly string PRUserRole = "PR User";
        public static readonly string PRAdminRole = "PR Admin";
        public static readonly string AcademyUserRole = "Academy User";
        public static readonly string AcademyAdminRole = "Academy Admin";
        public static readonly string SuperAdminRole = "Super Admin";
        public User theUser;
        protected readonly UserManager<User> _userManager;
        protected readonly PublicUserRepository _userRepository;
        private readonly NotificationRepository _notificationRepository;
        protected readonly string FileRoot = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot");
        protected const string MembershipPhotoRelativePath = "/members/photo/";
        protected const string MembershipNoPersonUrl = "/members/photo/person.png";
        protected const string MembershipNoLogoUrl = "/members/photo/logo.png";
        protected readonly string MembershipPhotoAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "members",
                                        "photo");

        protected readonly string ResourceProfileImagePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "Resource",
                                        "photo", "img");

        protected const string ResourceProfileRelativePath = "/Resource/photo/img/";

        protected readonly string NewsProfileImagePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "pr",
                                        "news");

        protected const string NewsProfileRelativePath = "/pr/news/";

        protected readonly string TextMediaProfileImagePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "pr",
                                        "text_media");
        protected const string TextMediaProfileRelativePath = "/pr/text_media/";

        protected readonly string GalleryImagePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "pr",
                                        "gallery");
        protected readonly string GalleryImagePathRelativePath = Path.Combine("pr",
                                        "gallery");


        protected readonly string ProfileImagePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "members",
                                        "photo", "img");

        protected const string ProfileRelativePath = "/members/photo/img/";


        protected readonly string ProfileAttachmentPath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "members",
                                        "documents", "attachment");

        protected const string ProfileRelativeAttachmentPath = "/members/documents/attachment/";

        protected const string MembershipDocumentRelativePath = "/membership/documents";
        protected readonly string MembershipDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "membership",
                                        "documents");

        protected const string CommunicationDocumentRelativePath = "/pr/documents";
        protected readonly string CommunicationDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "pr",
                                        "documents");

        protected const string TradePromotionDocumentRelativePath = "/tradepromotion/documents";
        protected readonly string TradePromotionDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "tradepromotion",
                                        "documents");

        protected const string InvestmentDocumentRelativePath = "/investment/documents";
        protected readonly string InvestmentDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "investment",
                                        "documents");

        protected const string ArbitrationDocumentRelativePath = "/arbitration/documents";
        protected readonly string ArbitrationDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "arbitration",
                                        "documents");

        protected const string AcademyDocumentRelativePath = "/academy/documents";
        protected readonly string AcademyDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "academy",
                                        "documents");

        protected const string ResearchDocumentRelativePath = "/research/documents";
        protected readonly string ResearchDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "research",
                                        "documents");


        protected const string OtherDocumentRelativePath = "/other/documents";
        protected readonly string OtherDocumentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "other",
                                        "documents");


        protected const string EmalAttachmentRelativePath = "/EmailAttachments";
        protected readonly string EmalAttachmentAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "EmailAttachments");

        protected readonly string TempAbsolutePath = Path.Combine(
                                       Directory.GetCurrentDirectory(),
                                       "wwwroot", "Temp");

        protected const string UserLegalProfileDocumentsRelativePath = "/Users/LegalProfiles";
        protected readonly string UserLegalProfileDocumentsAbsolutePath = Path.Combine(
                                       Directory.GetCurrentDirectory(),
                                       "wwwroot", "Users", "LegalProfiles");

        protected const string UserLogoDocumentsRelativePath = "/Users/Logo";
        protected readonly string UserLogoDocumentsAbsolutePath = Path.Combine(
                                       Directory.GetCurrentDirectory(),
                                       "wwwroot", "Users", "Logo");

        protected readonly string ReportAbsolutePath = Path.Combine(
                                       Directory.GetCurrentDirectory(),
                                       "wwwroot", "ReportTemplates");

        protected const string DirectoryRelativePath = "/directory";
        protected readonly string DirectoryAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "directory");


        protected readonly string IssuesAbsolutePath = Path.Combine(
                                    Directory.GetCurrentDirectory(),
                                    "wwwroot", "NTBIssues");


        protected const string AdHeaderRelativePath = "/AdvertisingImages/header";
        protected readonly string AdHeaderAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "header");
        protected const string AdFooterRelativePath = "/AdvertisingImages/footer";
        protected readonly string AdFooterAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "footer");
        protected const string AdHomeLeftRelativePath = "/AdvertisingImages/homeleft";
        protected readonly string AdHomeLeftAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "homeleft");
        protected const string AdHomeRightRelativePath = "/AdvertisingImages/homeright";
        protected readonly string AdHomeRightAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "homeright");
        protected const string AdPlatinumRelativePath = "/AdvertisingImages/platinum";
        protected readonly string AdPlatinumAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "platinum");
        protected const string AdGoldRelativePath = "/AdvertisingImages/gold";
        protected readonly string AdGoldAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "gold");
        protected const string AdSilverRelativePath = "/AdvertisingImages/silver";
        protected readonly string AdSilverAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "silver");
        protected const string AdBronzeRelativePath = "/AdvertisingImages/bronze";
        protected readonly string AdBronzeAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "bronze");
        protected const string AdOtherRelativePath = "/AdvertisingImages/other";
        protected readonly string AdOtherAbsolutePath = Path.Combine(
                                        Directory.GetCurrentDirectory(),
                                        "wwwroot", "AdvertisingImages",
                                        "other");


        [NonAction]
        public async Task<RoleLevelEnum> GetRoleLevel(ECCSAModuleFlag moduleEnum)
        {
            if (_userRepository == null) return RoleLevelEnum.None;
            var roles = await _userRepository.GetUserRoles(TheUser.Id);
            var adminRoleName = GetAdminRoleName(moduleEnum);
            var rl = roles.Contains(adminRoleName);
            if (rl) return RoleLevelEnum.Admin;
            rl = roles.Contains(GetUserRoleName(moduleEnum));
            if (rl) return RoleLevelEnum.User;
            rl = roles.Contains(GetLimitedUserRoleName(moduleEnum));
            if (rl) return RoleLevelEnum.LimitedUser;
            if (TheUser.AssignedModules.HasFlag(moduleEnum)) return RoleLevelEnum.View;
            return RoleLevelEnum.None;
        }
        [NonAction]
        public async Task<List<User>> GetPublicUsers()
        {
            return await _userRepository.PublicUsers();
        }

        [NonAction]
        private static string GetAdminRoleName(ECCSAModuleFlag moduleEnum)
        {
            var roles = Role.GetRoles(moduleEnum);
            var role = roles.FirstOrDefault(r => r.RoleLevel == RoleLevelEnum.Admin);
            return role?.NormalizedName ?? "";
        }
        [NonAction]
        private static string GetUserRoleName(ECCSAModuleFlag moduleEnum)
        {
            var roles = Role.GetRoles(moduleEnum);
            var role = roles.FirstOrDefault(r => r.RoleLevel == RoleLevelEnum.User);
            return role?.NormalizedName ?? "";

        }
        [NonAction]
        private static string GetLimitedUserRoleName(ECCSAModuleFlag moduleEnum)
        {
            var roles = Role.GetRoles(moduleEnum);
            var role = roles.FirstOrDefault(r => r.RoleLevel == RoleLevelEnum.LimitedUser);
            return role?.NormalizedName ?? "";

        }
        [NonAction]
        public string RelativePath(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Membership => MembershipDocumentRelativePath,
                ECCSAModuleFlag.Communication => CommunicationDocumentRelativePath,
                ECCSAModuleFlag.Investment => InvestmentDocumentRelativePath,
                ECCSAModuleFlag.Academy => AcademyDocumentRelativePath,
                ECCSAModuleFlag.Arbitration => ArbitrationDocumentRelativePath,
                ECCSAModuleFlag.Research => ResearchDocumentRelativePath,
                ECCSAModuleFlag.TradePromotion => TradePromotionDocumentRelativePath,
                _ => OtherDocumentRelativePath,
            };
        }
        [NonAction]
        public string RelativePath(AdvertisementSectionEnum section)
        {
            return section switch
            {
                AdvertisementSectionEnum.Header => AdHeaderRelativePath,
                //   AdvertisementSectionEnum.Footer => AdFooterRelativePath,
                //AdvertisementSectionEnum.HomeLeft => AdHomeLeftRelativePath,
                //AdvertisementSectionEnum.HomeRight => AdHomeRightRelativePath,
                AdvertisementSectionEnum.Platinum => AdPlatinumRelativePath,
                AdvertisementSectionEnum.Gold => AdGoldRelativePath,
                AdvertisementSectionEnum.Silver => AdSilverRelativePath,
                AdvertisementSectionEnum.Bronze => AdBronzeRelativePath,
                _ => AdOtherRelativePath,
            };
        }
        [NonAction]
        public string AbsolutePath(ECCSAModuleFlag module)
        {
            return module switch
            {
                ECCSAModuleFlag.Membership => MembershipDocumentAbsolutePath,
                ECCSAModuleFlag.Communication => CommunicationDocumentAbsolutePath,
                ECCSAModuleFlag.Investment => InvestmentDocumentAbsolutePath,
                ECCSAModuleFlag.Academy => AcademyDocumentAbsolutePath,
                ECCSAModuleFlag.Arbitration => ArbitrationDocumentAbsolutePath,
                ECCSAModuleFlag.Research => ResearchDocumentAbsolutePath,
                ECCSAModuleFlag.TradePromotion => TradePromotionDocumentAbsolutePath,
                _ => OtherDocumentAbsolutePath,
            };
        }
       

        [NonAction]
        public string AbsolutePath(AdvertisementSectionEnum section)
        {
            return section switch
            {
                AdvertisementSectionEnum.Header => AdHeaderAbsolutePath,
                //   AdvertisementSectionEnum.Footer => AdFooterAbsolutePath,
               // AdvertisementSectionEnum.HomeLeft => AdHomeLeftAbsolutePath,
               // AdvertisementSectionEnum.HomeRight => AdHomeRightAbsolutePath,
                AdvertisementSectionEnum.Platinum => AdPlatinumAbsolutePath,
                AdvertisementSectionEnum.Gold => AdGoldAbsolutePath,
                AdvertisementSectionEnum.Silver => AdSilverAbsolutePath,
                AdvertisementSectionEnum.Bronze => AdBronzeAbsolutePath,
                _ => AdOtherAbsolutePath,
            };
        }

        public BaseController(
            UserManager<User> userManager,
            PublicUserRepository userRepository,
            NotificationRepository notificationRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
        }
        //public BaseController(
        //   UserManager<User> userManager)
        //{
        //    _userManager = userManager;
        //}
        [NonAction]
        public void LogStart<T>(ILogger<T> _logger, string controllerName, string methodName)
        {
            _logger.LogInformation(controllerName, methodName, "Started", DateTime.UtcNow);
        }

        [NonAction]
        public void LogEnd<T>(ILogger<T> _logger, string controllerName, string methodName)
        {
            _logger.LogInformation(controllerName, methodName, "End", DateTime.UtcNow);
        }

        [NonAction]
        public void LogError<T>(ILogger<T> _logger, Exception ex, string controllerName, string methodName, object model)
        {
            _logger.LogError(ex, controllerName, methodName, model, DateTime.UtcNow);
        }
        [NonAction]
        public void LogSecurityError<T>(ILogger<T> _logger, string controllerName, string methodName)
        {
            _logger.LogError("Insufficient Privilege", controllerName, methodName, TheUser, DateTime.UtcNow);
        }
        [NonAction]
        public List<ExchangeRate> Rss()
        {
            string[] feeds = new string[]
            {
                "https://market.nbebank.com/xml/rss.php"
            };
            List<SyndicationItem> finalItems = new();
            List<ExchangeRate> lst = new();

            try
            {
                foreach (string feed in feeds)
                {
                    XmlReader reader = XmlReader.Create(feed);
                    Rss20FeedFormatter formatter = new();
                    formatter.ReadFrom(reader);
                    reader.Close();
                    finalItems.AddRange(formatter.Feed.Items);
                }
            }
            catch
            {
                return lst;
            }

            foreach (var si in finalItems)
            {
                var prices = si.Summary.Text.Split(",");
                decimal buy, sell;
                buy = 0; sell = 0;
                foreach (var price in prices)
                {
                    var pi = price.Trim().Split(" ");
                    if (pi[0].ToLowerInvariant().StartsWith("buy"))
                    {
                        buy = decimal.Parse(pi[1]);
                    }
                    if (pi[0].ToLowerInvariant().StartsWith("sell"))
                    {
                        sell = decimal.Parse(pi[1]);
                    }
                }
                var title = si.Title.Text;
                lst.Add(new ExchangeRate
                {
                    Id = Guid.NewGuid(),
                    TransactionDate = si.PublishDate.Date,
                    Currency = title,
                    Buying = buy,
                    Selling = sell
                });
            }
            return lst;
        }
        public User TheUser
        {
            get
            {
                if (theUser != null) return theUser;
                AsyncContext.Run(InitUser);
                return theUser;
            }
        }
        private async Task InitUser()
        {
            if (HttpContext != null && HttpContext.User != null && theUser == null)
            {
                theUser = await _userManager.GetUserAsync(HttpContext.User);
                if (_userRepository != null && theUser != null)
                    theUser.Roles = await _userRepository.GetUserRoles(theUser.Id);

                //var isInRole = await _userManager.IsInRoleAsync(theUser, "PRA");
            }
        }
        [NonAction]
        protected void CommandStatus(int Status)
        {
            if (Status == 1)
            {
                TempData["CommandStatus"] = "Data Saved Successfully !!";
            }
            else if (Status == 2)
            {
                TempData["CommandStatus"] = "Data Already Exist !!";
            }
            else if (Status == 3)
            {
                TempData["CommandStatus"] = "Data Updated Successfully !!";
            }
            else if (Status == 4)
            {
                TempData["CommandStatus"] = "Data Deleted Successfully !!";
            }
            else if (Status == 5)
            {
                TempData["CommandStatus"] = "Invoice Generated Successfully !!";
            }
            else if (Status == 6)
            {
                TempData["CommandStatus"] = "Data can't be deleted due to Dependancy!!";

            }
            else if (Status == 7)
            {
                TempData["CommandStatus"] = "Validation Erorr!!";

            }
            else if (Status == 8)
            {
                TempData["CommandStatus"] = "Invoice Cancelled !!";
            }
            else if (Status == 9)
            {
                TempData["CommandStatus"] = "Payment made against invoice !!";
            }
            else if (Status == 0)
            {
                TempData["CommandStatus"] = "Something Went Wrong. Please Try Again !!";
            }
            else if (Status == 10)
            {
                TempData["CommandStatus"] = "Membership Process Initiated Successfully!!";
            }
            else if (Status == 10)
            {
                TempData["CommandStatus"] = "Please configure PW Schedule Period on this location before assigning attendance !!";
            }
            else if (Status == 11)
            {
                TempData["CommandStatus"] = "Please select location !!";
            }
            else if (Status == 11)
            {
                TempData["CommandStatus"] = "The Selected Household has no Family Members,Please Check again !!";
            }
            else if (Status == 12)
            {
                TempData["CommandStatus"] = "Current Location does not have Period !!";
            }
            else if (Status == 13)
            {
                TempData["CommandStatus"] = "Schedule has not been created for current location !1";
            }
            else if (Status == 14)
            {
                TempData["CommandStatus"] = "Please select a Woreda !!";
            }
            else if (Status == 15)
            {
                TempData["CommandStatus"] = "Data can't be be Updated due to dependency !!";
            }
            else if (Status == 16)
            {
                TempData["CommandStatus"] = "Selected Location does not have Budget Celling !!";
            }
            else if (Status == 17)
            {
                TempData["CommandStatus"] = "Data Rejected !!";
            }
            else if (Status == 18)
            {
                TempData["CommandStatus"] = "Data Approved Successfully !!";
            }
            else if (Status == 19)
            {
                TempData["CommandStatus"] = "You do not have Permission !!";
            }
            else
            {
                TempData["CommandStatus"] = null;
            }
        }
        protected void CommandStatus(string message)
        {
            TempData["CommandStatus"] = message;
        }
        [NonAction]
        protected void CommandStatus(CommandStatusEnum statusEnum)
        {
            int Status = (int)statusEnum;
            CommandStatus(Status);
        }

        [NonAction]
        protected string Guid2String(Guid id) => id.ToString()[^15..].Replace("-", ""); //  Convert.ToBase64String(id.ToByteArray()).Replace("==", "").Replace("/","^");
        [NonAction]
        protected string Guid2String2(Guid id) => id.ToString()[^4..].Replace("-", ""); //  Convert.ToBase64String(id.ToByteArray()).Replace("==", "").Replace("/","^");


        [NonAction]
        protected BinaryFile ProcessImage(IFormFile files)
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    CheckFileExtension(fileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Guid2String(Guid.NewGuid()), fileExtension);

                    var objfiles = new BinaryFile()
                    {
                        Name = newFileName,
                        FileType = fileExtension,
                    };

                    using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        objfiles.DataFiles = target.ToArray();
                    }
                    return objfiles;

                }
            }
            return new BinaryFile()
            {
                Name = string.Empty,
                FileType = string.Empty,
                DataFiles = null
            }; ;
        }

        //[NonAction]
        //protected string ProcessImage(Guid id, byte[] data, string extension, string path,  string defaultPath)
        //{
        //    if (data != null)
        //    {
        //        if (data.Length > 0)
        //        {
        //            var newFileName = String.Concat(Guid2String(id), extension);
        //            var p = path.Replace("/", "\\");
        //            var fullName =  p + newFileName; // Path.Combine(wwwRootPath, p); // Path.Combine(Environment.CurrentDirectory, path, newFileName);
        //            try
        //            {
        //                System.IO.File.WriteAllBytes(fullName, data);
        //            }
        //            catch(Exception ex)
        //            {
        //                newFileName = ex.Message;
        //            }
        //            return Path.Combine(path, newFileName);
        //        }
        //    }
        //    return defaultPath ;
        //}

        [NonAction]
        protected string ProcessImage(Guid id, string extension, string relativePath, string absoultePath, string defaultPath)
        {
            string fileName = $"{absoultePath}/{Guid2String(id)}{extension}";
            if (System.IO.File.Exists(fileName))
            {
                return $"{relativePath}/{Guid2String(id)}{extension}";
            }
            return defaultPath;
        }

        [NonAction]
        protected async Task<String> SaveDocument(Guid id, ECCSAModuleFlag module, IFormFile file, string subFolder = "")
        {
            var absolutePath = AbsolutePath(module);
            absolutePath = Path.Combine(absolutePath, subFolder);
            var shortName = Guid2String(id);
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);
            return RelativePath(module) + "/" + newFileName;
        }
        protected string GetLocalFileName(string shortName)
        {
            return shortName + "_AMH_";
        }
        [NonAction]
        protected async Task<String> SaveDocumentExtension(Guid id, ECCSAModuleFlag module, IFormFile file, bool localFile = false)
        {
            var absolutePath = AbsolutePath(module);
            var shortName = Guid2String(id);
            shortName = localFile ? GetLocalFileName(shortName) : shortName;

            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);
            return extension;// RelativePath(module) + "/" + newFileName;
        }



        [NonAction]
        protected async Task<String> SaveDocument(Guid id, AdvertisementSectionEnum module, IFormFile file)
        {
            var absolutePath = AbsolutePath(module);
            var shortName = Guid2String(id);
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);
            return extension;
        }

        [NonAction]
        protected async Task<string> SaveFile(string folder, string fileName, IFormFile file, string shortFileName = "")
        {
            if (file != null)
            {
                CheckFileExtension(file.FileName);
                var dInfo = Directory.CreateDirectory(folder);
                if (!string.IsNullOrEmpty(shortFileName))
                {
                    var files = dInfo.GetFiles();
                    foreach (var fInfo in files)
                    {
                        if (fInfo.Name.StartsWith(shortFileName))
                        {
                            System.IO.File.Delete(fInfo.FullName);
                        }
                    }
                }
                var result = Path.Combine(folder, fileName);
                using var stream = new FileStream(result, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
        [NonAction]
        protected bool CheckFileExtension(string fileName)
        {
            string extension = Path.GetExtension(Path.GetFileName(fileName)) ?? "";
            List<string> imageExts = new() { "jpg", "jpeg", "pdf", "png" };
            if (!imageExts.Contains(extension.Replace(".", "").ToLower()))
            {
                throw new Exception("Invalid file! Only PNG, JPG, JPEG Images, and PDF are Allowed!");
            }
            return true;
        }

        [NonAction]
        public async Task<string> GetHashCode(string path, string fileName)
        {
            var fullName = path;
            if (!System.IO.File.Exists(fullName))
            {
                if (fullName.EndsWith("\\"))
                    fullName = string.Concat(path, fileName);
                else
                    fullName = string.Concat(path, "\\", fileName);
            }
            MemoryStream memoryStream = new();
            try
            {
                using var stream = new FileStream(fullName, FileMode.Open, FileAccess.Read);
                await stream.CopyToAsync(memoryStream);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return BaseConverter.GetCode(memoryStream.ToArray().GetHashCode());
        }

        [NonAction]
        public async Task<string> GetHashCode(IFormFile file)
        {

            MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream);
            return BaseConverter.GetCode(memoryStream.ToArray().GetHashCode());
        }
        [NonAction]
        public string GetExtension(string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                return string.Empty;
            }
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (file.Contains(fileName))
                {
                    var extension = Path.GetExtension(file);
                    return extension;
                }
            }
            return string.Empty;

        }

        [NonAction]
        public async Task<FileResult> DownLoadFile(string path, string fileName, string downloadFileName = "")
        {
            if (string.IsNullOrEmpty(downloadFileName))
                downloadFileName = fileName;
            var fullName = path;
            if (!System.IO.File.Exists(fullName))
            {
                if (fullName.EndsWith("//"))
                    fullName = string.Concat(path, fileName);
                else
                    fullName = Path.Combine(path, fileName);
            }
            MemoryStream memoryStream = new();
            try
            {
                using var stream = new FileStream(fullName, FileMode.Open, FileAccess.Read);
                await stream.CopyToAsync(memoryStream);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return File(memoryStream.ToArray(), MediaTypeNames.Application.Octet, downloadFileName);
        }

        [NonAction]
        public List<string> GetReportTemplates()
        {

            var files = Directory.GetFiles(ReportAbsolutePath);
            var lst = new List<string>
            {
                string.Empty
            };
            foreach (var file in files)
            {
                lst.Add(file.Replace(".frx", ""));
            }

            return lst;

        }

        [HttpPost]
        public FileResult ExportToExcel<T>(List<T> data, Dictionary<string, string> columnNames, string fileName)
        {
            var workbook = new XLWorkbook();
            IXLWorksheet sheet = workbook.Worksheets.Add("Sheet 1");
            var i = 1;
            foreach (var columnName in columnNames.Values)
            {
                sheet.Cell(1, i).Value = columnName;
                i++;
            }
            var index = 2;
            foreach (var val in data)
            {
                i = 1;
                foreach (var pName in columnNames.Keys)
                {
                    sheet.Cell(index, i).Value = val.GetType().GetProperty(pName).GetValue(val);
                    i++;
                }
                index++;
            }

            using MemoryStream stream = new();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
        }
        [NonAction]
        public DataTable ImportExcel(IFormFile file
            , ExternalDataType dataType
            , DateTime startDate
            , DateTime endDate
            )
        {
            string webRootPath = TempAbsolutePath;
            string newPath = TempAbsolutePath;

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file == null || file.Length == 0)
            {
                throw new InvalidDataException("Nothing to import");
            }
            string sFileExtension = Path.GetExtension(file.FileName).ToLower();
            if (sFileExtension != ".xls" && sFileExtension != ".xlsx")
            {
                throw new InvalidDataException("Only Excel file is supported");
            }
            var table = dataType.GetDataTable();

            if (file.Length > 0)
            {
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                file.CopyTo(stream);

                stream.Position = 0;
                if (sFileExtension == ".xls")
                {
                    HSSFWorkbook hssfwb = new(stream); //This will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new(stream); //This will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                }
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                Dictionary<int, int> cellToColumn = new();
                for (int i = 0; i < cellCount; i++)
                {
                    var headerName = headerRow.GetCell(i).ToString().ToLowerInvariant().Replace(" ", "").Trim();
                    var index = dataType.MetaData.FindIndex(x => x.ExcelHeaderName.ToLowerInvariant().Replace(" ", "").Trim() == headerName);
                    cellToColumn.Add(i, index);
                }



                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    object[] tmp = new object[cellCount + 3];
                    tmp[0] = Guid.NewGuid().ToString();
                    tmp[1] = startDate.ToString("yyyy-MM-dd");
                    tmp[2] = endDate.ToString("yyyy-MM-dd");


                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            var sVal = row.GetCell(j).ToString();
                            if (cellToColumn.ContainsKey(j))
                            {
                                tmp[cellToColumn[j]] = sVal;
                            }

                        }
                    }

                    table.LoadDataRow(tmp, true);
                }
            }
            return table;
        }
        [NonAction]
        public DataTable ImportExcel(FileInfo file)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidDataException("Nothing to import");
            }
            string sFileExtension = file.Extension.ToLower();
            if (sFileExtension != ".xls" && sFileExtension != ".xlsx")
            {
                throw new InvalidDataException("Only Excel file is supported");
            }
            var table = new DataTable();

            if (file.Length > 0)
            {
                ISheet sheet;
                string webRootPath = TempAbsolutePath;
                var tmpPath = Path.Combine(webRootPath, file.Name);
                using var stream = new FileStream(tmpPath, FileMode.Create);
                file.OpenRead().CopyTo(stream);
                stream.Position = 0;
                if (sFileExtension == ".xls")
                {
                    HSSFWorkbook hssfwb = new(stream); //This will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new(stream); //This will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                }
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                Dictionary<int, int> cellToColumn = new();
                for (int i = 0; i < cellCount; i++)
                {
                    var headerName = headerRow.GetCell(i).ToString().ToLowerInvariant().Replace(",", "").Replace(" ", "").Trim();
                    headerName = headerName.Length > 10 ? headerName[..^10] : headerName;
                    table.Columns.Add(new DataColumn()
                    {
                        ColumnName = headerName,
                        DataType = typeof(string)
                    });
                }



                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {

                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    object[] tmp = new object[cellCount];
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            var sVal = row.GetCell(j).ToString();

                            tmp[j] = sVal;
                        }
                    }

                    table.LoadDataRow(tmp, true);
                }
            }
            return table;
        }
        [NonAction]
        public List<T> ImportExcel<T>(IFormFile file, List<ImportTemplate> columns) where T : IId, new()
        {
            string webRootPath = TempAbsolutePath;
            string newPath = TempAbsolutePath;// Path.Combine(webRootPath, folderName);

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file == null || file.Length == 0)
            {
                throw new InvalidDataException("Nothing to import");
            }
            string sFileExtension = Path.GetExtension(file.FileName).ToLower();
            if (sFileExtension != ".xls" && sFileExtension != ".xlsx")
            {
                throw new InvalidDataException("Only Excel file is supported");
            }
            List<T> lst = new();
            if (file.Length > 0)
            {

                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                file.CopyTo(stream);
                stream.Position = 0;
                if (sFileExtension == ".xls")
                {
                    HSSFWorkbook hssfwb = new(stream); //This will read the Excel 97-2000 formats  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                }
                else
                {
                    XSSFWorkbook hssfwb = new(stream); //This will read 2007 Excel format  
                    sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                }
                IRow headerRow = sheet.GetRow(0); //Get Header Row

                int cellCount = headerRow.LastCellNum;


                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                {
                    T t = new()
                    {
                        Id = Guid.NewGuid()
                    };
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            var sVal = row.GetCell(j).ToString();
                            var col = columns[j];
                            if (col.Name == String.Empty) continue;
                            switch (col.ColumnType)
                            {
                                case DataTypeEnum.Date:
                                    t.GetType().GetProperty(col.Name).SetValue(t, DateTime.Parse(sVal));
                                    break;
                                case DataTypeEnum.Decimal:
                                    t.GetType().GetProperty(col.Name).SetValue(t, decimal.Parse(sVal));
                                    break;
                                case DataTypeEnum.Integer:
                                    t.GetType().GetProperty(col.Name).SetValue(t, int.Parse(sVal));
                                    break;
                                case DataTypeEnum.String:
                                    t.GetType().GetProperty(col.Name).SetValue(t, sVal);
                                    break;
                            }

                        }
                    }

                    lst.Add(t);
                }
            }
            return lst;
        }
        [NonAction]
        protected async Task SaveNotification(ECCSAModuleFlag owner, String subject, String body,
            List<INotificationAddress> recipents, IFormFile file, SendThroughFlag sendThrough)
        {
            var id = Guid.NewGuid();
            if (sendThrough.HasFlag(SendThroughFlag.Email))
            {
                var msg = new EmailMessage()
                {
                    Id = id,
                    Owner = owner,
                    Subject = subject.Length > 200 ? subject.Substring(0, 200) : subject,
                    Body = body,
                    Recipients = new List<MessageDelivery>(),
                    Status = MessageStatus.Submitted,
                    Attachment = ""
                };
                foreach (var r in recipents)
                {
                    if (!string.IsNullOrWhiteSpace(r.Email))
                    {
                        msg.Recipients.Add(new MessageDelivery { Address = r.Email, CCGroup = 0, Label = r.FullName, Status = DeliveryStatus.Planned });
                    }
                }
                if (file != null && file.Length > 0)
                {
                    string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                    await SaveFile(EmalAttachmentAbsolutePath, $"{Guid2String(msg.Id)}{extension}", file);
                    CommandStatus(CommandStatusEnum.Saved);
                    msg.Attachment = extension;
                }
                await _notificationRepository.Save(msg, TheUser != null ? TheUser.Id : Guid.Empty);
            }
            //
            if (sendThrough.HasFlag(SendThroughFlag.SMS))
            {
                var sms = new SMSMessage()
                {
                    Id = id,
                    Owner = owner,
                    Message = subject.Length > 200 ? subject.Substring(0, 200) : subject,
                    Recipients = new List<MessageDelivery>(),
                    Priority = 1,
                    Status = MessageStatus.Submitted,
                };
                foreach (var r in recipents)
                {
                    if (!string.IsNullOrWhiteSpace(r.MobileNo))
                    {
                        var rec = new MessageDelivery { Address = r.MobileNo, Label = r.FullName, Status = DeliveryStatus.Planned };
                        try
                        {
                           var result = await SmsService.Send(r.MobileNo, subject);
                            if (result)
                            {
                                rec.Status = DeliveryStatus.Sent;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        sms.Recipients.Add(rec);
                    }
                }
                if (!sms.Recipients.Any(s => s.Status != DeliveryStatus.Sent && s.Status != DeliveryStatus.Delivered
                 && s.Status != DeliveryStatus.NotDelivered))
                {
                    sms.Status = MessageStatus.Processed;
                }
                await _notificationRepository.Save(sms, TheUser != null ? TheUser.Id : Guid.Empty);
            }
        }

        [NonAction]
        protected bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
                return true;
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }

        }
        [NonAction]
        protected bool IsValidPhoneNo(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return true;
            phone = phone.Trim()
                .Replace(" ", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("+", "")
                .Replace("-", "");
            if (phone.Length < 9 || phone.Length > 12)
                return false;
            var digits = phone.ToCharArray();

            foreach (var c in digits)
            {
                if (!char.IsNumber(c)) return false;
            }

            return true;

        }

        [NonAction]
        protected async Task<String> SaveLogo(Guid id, IFormFile file)
        {
            var absolutePath = UserLogoDocumentsAbsolutePath;
            var shortName = Guid2String(id);
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);
            return UserLogoDocumentsRelativePath + "/" + newFileName;
        }
    }

    public sealed class ImportTemplate
    {
        public string Name { get; set; }

        public DataTypeEnum ColumnType { get; set; }

    }
    public enum DataTypeEnum
    {
        String = 0,
        Date = 1,
        Integer = 3,
        Decimal = 4
    }
    public class BinaryFile
    {

        public string Name { get; set; }
        [MaxLength(100)]
        public string FileType { get; set; }
        [MaxLength]
        public byte[] DataFiles { get; set; }
    }
    public enum CommandStatusEnum
    {
        None = -1,
        UnkownError = 0,
        Saved = 1,
        DataExist = 2,
        Updated = 3,
        Deleted = 4,
        InvoiceGenerated = 5,

        CannotDelete = 6,
        ValidationError = 7,
        InvoiceCancelled = 8,
        PaymentMade = 9,
        RenewalInitiated = 10,
        SelectLocation = 11,
        Rejected = 17,
        Approved = 18,
        InsufficientPrevilage = 19
    }

}
