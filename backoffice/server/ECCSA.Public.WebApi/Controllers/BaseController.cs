using ECCSA.BackOffice.Server.Model.PublicSecurity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECCSA.Public.WebApi.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {

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
        public PublicUser theUser;
        protected readonly UserManager<PublicUser> _userManager;

        public BaseController(
            UserManager<PublicUser> userManager)
        {
            _userManager = userManager;
        }
        public PublicUser TheUser
        {
            get
            {
                if (theUser != null) return theUser;
                InitUser();
                return theUser;
            }
        }
        private void InitUser()
        {
            if (HttpContext != null && HttpContext.User != null && theUser == null)
                theUser = _userManager.GetUserAsync(HttpContext.User).Result;
        }
        [NonAction]
        protected string Guid2String(Guid id) => id.ToString()[^15..].Replace("-", ""); //  Convert.ToBase64String(id.ToByteArray()).Replace("==", "").Replace("/","^");

        [NonAction]
        protected async Task<string> SaveFile(string folder, string fileName, IFormFile file)
        {
            if (file != null)
            {
                Directory.CreateDirectory(folder);
                using var stream = new FileStream(folder + "\\" + fileName, FileMode.Create);
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
