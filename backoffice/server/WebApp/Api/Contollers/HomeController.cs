using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.PublicSecurity;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing;
using ECCSA.BackOffice.Server.Persistence.Repositories.Membership;
using ECCSA.BackOffice.Server.Persistence.Repositories.Payment;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Trade;
using ECCSA.BackOffice.Server.WebApp.Api.Model;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using NPOI.SS.Formula.Functions;
using System.Drawing;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        readonly AssociationRepository _associationRepository;
        private readonly InvoiceRepository _invoiceRepository;
        private readonly LegalDocumentTypeRepository _legalDocumentTypeRepository;
        private readonly LocationRepository _locationRepository;
        private readonly LegalProfileRepository _legalProfileRepository;
        private readonly ExportAgreementRepository _exportAgreementRepository;
        private readonly UserContactRepository _userContactRepository;
        private readonly ApplicantRepository _applicantRepository;
        private readonly TradeRepository _tradeRepository;
        private readonly YesNoRepository _yesNoRepository;


        public HomeController(ILogger<HomeController> logger,
            UserManager<User> userManager,
            AssociationRepository associationRepository,
            LegalDocumentTypeRepository legalDocumentTypeRepository,
            LocationRepository locationRepository,
            InvoiceRepository invoiceRepository,
            ApplicantRepository applicantRepository,
            UserContactRepository userContactRepository,
            LegalProfileRepository legalProfileRepository,
            ExportAgreementRepository exportAgreementRepository,
            TradeRepository tradeRepository,
            YesNoRepository yesNoRepository,
            PublicUserRepository userRepository
            , NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _associationRepository = associationRepository;

            _locationRepository = locationRepository;
            _legalDocumentTypeRepository = legalDocumentTypeRepository;

            _invoiceRepository = invoiceRepository;
            _applicantRepository = applicantRepository;

            _legalProfileRepository = legalProfileRepository;
            _exportAgreementRepository = exportAgreementRepository;

            _userContactRepository = userContactRepository;
            _tradeRepository = tradeRepository;
            _yesNoRepository = yesNoRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                if (User == null || User.Identity == null || User.Identity.Name == null)
                {
                    return Ok(false);
                }
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return Ok(false);
                }
                var userRoles = await _userManager.GetRolesAsync(user);
                string logoUrl = !string.IsNullOrWhiteSpace(user.LogoExtension) ?
                    $"{UserLogoDocumentsRelativePath}/{Guid2String(user.Id) + user.LogoExtension}" : "";
                if (string.IsNullOrWhiteSpace(user.LogoExtension) ||
                     !System.IO.File.Exists(Path.Combine(UserLogoDocumentsAbsolutePath, Guid2String(user.Id) + user.LogoExtension)))
                {
                    logoUrl = string.Empty;
                }
                return Ok(new PublicUserProfile
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Email_verified = false,
                    MemberId = user.AssociationId,
                    FullName = user.FullName,
                    TIN = user.TIN,
                    Address = user.Address,
                    CompanyName = user.CompanyName,
                    MobileNo = user.MobileNo,
                    PhoneNo = user.PhoneNumber,
                    BusinessAccount = user.BusinessAccount,
                    Logo = logoUrl
                });
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to get user profile");
            }
        }
        [HttpPost]
        [Route("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm]RegisterInputModel model, IFormCollection formCollection)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var errorMsg = string.Empty;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return Ok(new
                {
                    errorMsg = "Invalid user"
                });
            }
            if (user.EmailConfirmed == false || user.MobileNoConfirmed == false)
            {
                return Ok(new
                {
                    errorMsg = "Cannot update profile before confirming email and mobile no"
                });
            }
            if (user.Email != model.Email && user.MobileNo != model.MobileNo)
            {
                return Ok(new
                {
                    errorMsg = "Email & Mobile no change at the same time is not allowed"
                });
            }
            var hasEmailOrMobileChange = user.Email != model.Email || user.MobileNo != model.MobileNo;
            if (hasEmailOrMobileChange)
            {
                user.EmailConfirmed = user.Email == model.Email;
                user.MobileNoConfirmed = user.MobileNo == model.MobileNo;
                user.OldMobileNo = user.MobileNo;
                user.OldEmail = user.Email;
            }
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNo;
            user.FullName = model.FullName;
            //user.TIN = model.TIN;
            user.Address = model.Address;
            //user.CompanyName = model.CompanyName;
            user.MobileNo = model.MobileNo;
            var file = formCollection.Files.FirstOrDefault(f => f.Name == "logo");
            if (file != null)
            {
                var kbyteSize = file.Length / 1000;
                if (kbyteSize > 65)
                {
                    return Ok(new
                    {
                        errorMsg = "Invalid Image size for logo! Size must be below 65kb"
                    });
                }
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    using (var img = Image.FromStream(memoryStream))
                    {
                        if(img.Width > 114 || img.Height > 114)
                        {
                            return Ok(new
                            {
                                errorMsg = "Invalid Image resolution for logo! Must have a max of 114 width and max of 114 height"
                            });
                        }
                    }
                }
                List<string> imageExts = new() { /*"pjp", "jfif", "jpg", "jpeg", "pjpeg", "bmp", "gif",*/ "png" };
                string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                if (!imageExts.Contains(extension.Replace(".", "").ToLower()))
                {
                    return Ok(new
                    {
                        errorMsg = "Invalid Image type for Log! Only PNG Allowed"
                    });
                }
                user.LogoExtension = extension;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (file != null)
                {
                    var fileName = file.FileName;
                    await SaveLogo(user.Id, file);
                }
                _logger.LogInformation("User profile updated.");
                return Ok(new
                {
                    success = true,
                    hasEmailOrMobileChange
                });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }
            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                errorMsg
            });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("undoEmailMobileChange")]
        public async Task<IActionResult> UndoEmailMobileChange([FromBody] InputModel model)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var errorMsg = string.Empty;
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest("Invalid User");
            }
            if (user.Disabled)
            {
                return BadRequest("User Disabled");
            }
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
            {
                return Ok(new
                {
                    errorMsg = "Invalid user"
                });
            }

            if (user.OldEmail == null && user.OldMobileNo == null)
            {
                return Ok(new
                {
                    errorMsg = "Unable to find previous email/mobile #"
                });
            }
            user.Email = user.OldEmail ?? user.Email;
            user.MobileNo = user.OldMobileNo ?? user.MobileNo;
            user.EmailConfirmed = true;
            user.MobileNoConfirmed = true;
            user.EmailConfirmationCode = null;
            user.MobileConfirmationCode = null;
            user.ConfirmationCodeExpiryDate = null;
            user.OldEmail = null;
            user.OldMobileNo = null;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("User email/mobile # change undone.");
                return Ok(new
                {
                    success = true,
                });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }
            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                errorMsg
            });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("getEmailMobileVerification")]
        public async Task<IActionResult> GetEmailMobileVerification([FromBody] GetEmailMobileConfirmationModel model)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var errorMsg = string.Empty;
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new
                {
                    errorMsg = "Invalid user"
                });
            }
            if (user.EmailConfirmed && user.MobileNoConfirmed)
            {
                return Ok(new
                {
                    errorMsg = "Confirmation Not needed for user!"
                });
            }

            user.EmailConfirmationCode = Guid.NewGuid().ToString().Replace("-", "");
            user.MobileConfirmationCode = Guid2String2(Guid.NewGuid()).ToUpper();
            user.ConfirmationCodeExpiryDate = DateTime.UtcNow.AddHours(1);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var subject = $"Verify Email and Phone For your ECCSA account.";
                var body = $"<a href='{model.ConfirmationUrl}/{user.EmailConfirmationCode}' target='_blank'><strong>Click Here to go to email/mobile confirmation page!</strong></a>";
                var recipients = new List<INotificationAddress> { user };
                await SaveNotification(ECCSAModuleFlag.Administration, subject, body,
                    recipients,
                    null, SendThroughFlag.Email);
                subject = $"Verify Phone For your ECCSA account code {user.MobileConfirmationCode}.";
                await SaveNotification(ECCSAModuleFlag.Administration, subject, "",
                    recipients,
                    null, SendThroughFlag.SMS);
                //
                _logger.LogInformation("Email/mobile verification link send");
                return Ok(new
                {
                    success = true,
                });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }
            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                errorMsg
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getPasswordResetLink")]
        public async Task<IActionResult> GetPasswordResetLink([FromBody] GetEmailMobileConfirmationModel model)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var errorMsg = string.Empty;
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Ok(new
                {
                    errorMsg = "Invalid user"
                });
            }

            user.PasswordResetCode = Guid.NewGuid().ToString().Replace("-", "");
            user.PasswordResetCodeExpiryDate = DateTime.UtcNow.AddHours(1);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var subject = $"Reset Password Link for your ECCSA account.";
                var body = $"<a href='{model.ConfirmationUrl}/{user.PasswordResetCode}' target='_blank'><strong>Click Here to go to reset password page!</strong></a>";
                var recipients = new List<INotificationAddress> { user };
                await SaveNotification(ECCSAModuleFlag.Administration, subject, body,
                    recipients,
                    null, SendThroughFlag.Email);
                subject = $"Reset Password Link for your ECCSA account sent to your email.";
                await SaveNotification(ECCSAModuleFlag.Administration, subject, body,
                    recipients,
                    null, SendThroughFlag.SMS);
                //
                _logger.LogInformation("Password reset link send.");
                return Ok(new
                {
                    success = true,
                });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }
            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                errorMsg
            });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("verifyEmailMobile")]
        public async Task<IActionResult> VerifyEmailMobile([FromBody] VerifyEmailMobileModel model)
        {
            var errorMsg = string.Empty;
            var user = await _userRepository.GetPublicUser(model.EmailCode, model.MobileNoCode);
            if (user == null)
            {
                return Ok(new
                {
                    success = false,
                    errorMsg = "Email/Mobile confirmation code invalid!"
                });
            }
            if (user.ConfirmationCodeExpiryDate < DateTime.UtcNow)
            {
                return Ok(new
                {
                    success = false,
                    errorMsg = "Email/Mobile confirmation code Expired! Request New confirmation codes.!"
                });
            }

            user = await _userManager.FindByNameAsync(user.UserName);
            user.EmailConfirmed = true;
            user.MobileNoConfirmed = true;
            user.EmailConfirmationCode = null;
            user.MobileConfirmationCode = null;
            user.ConfirmationCodeExpiryDate = null;
            user.OldEmail = null;
            user.OldMobileNo = null;
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Email/Mobile Confirmed.");
                    return Ok(new
                    {
                        success = true,
                    });
                }
                foreach (var error in result.Errors)
                {
                    errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
                }
            }
            catch (Exception ex)
            {

            }
            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                errorMsg
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var errorMsg = string.Empty;
            var user = await _userRepository.GetPublicUser(model.ResetCode);
            if (user.PasswordResetCodeExpiryDate < DateTime.UtcNow)
            {
                return Ok(new
                {
                    success = false,
                    errorMsg = "Reset password link Expired! Request New reset link!"
                });
            }

            user = await _userManager.FindByNameAsync(user.UserName);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (result.Succeeded)
            {
                user.PasswordResetCode = null;
                user.PasswordResetCodeExpiryDate = null;
                await _userManager.UpdateAsync(user);
                _logger.LogInformation("Password Reseted.");
                return Ok(new
                {
                    success = true
                });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }

            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                success = false,
                errorMsg
            });
        }
        [HttpPost]
        [Route("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");
            var errorMsg = string.Empty;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var subject = $"Password for your ECCSA account {User.Identity.Name} Changed.";
                var body = $"Password for your ECCSA account {User.Identity.Name} Changed at {DateTime.UtcNow.Date.ToString("yyyy-MM-dd")}.";
                var recipients = new List<INotificationAddress> { user };
                await SaveNotification(ECCSAModuleFlag.Administration, subject, body,
                    recipients,
                    null, SendThroughFlag.Email | SendThroughFlag.SMS);
                //
                _logger.LogInformation("Password changed.");
                return Ok(new
                {
                    success = true
                });
            }
            foreach (var error in result.Errors)
            {
                errorMsg += (errorMsg != string.Empty ? ", " : "") + error.Description;
            }
            // If we got this far, something failed, redisplay form
            return Ok(new
            {
                errorMsg
            });
        }

        [HttpGet]
        [Route("GetTree")]
        public async Task<JsonResult> GetTree()
        {
            var associations = await _associationRepository.List(TheUser.Id);
            var tview = Areas.Membership.Model.TreeView.BuildTree(associations, Guid.Empty);
            return new JsonResult(tview);
        }

        [HttpGet]
        [Route("GetLocation")]
        public JsonResult GetLocation(Guid? parentId)
        {
            var locations = _locationRepository.Locations(TheUser.Id).ToList();
            Guid pId;
            if (parentId.HasValue && parentId.Value != Guid.Empty)
            {
                pId = parentId.Value;
            }
            else
            {
                pId = Guid.Parse("00000000-0000-0000-0000-0000000f4240");
            }
            var tmp = locations.Where(l => l.ParentId == pId
                  || l.Id == pId).ToList();

            tmp.Insert(0, new Location
            {
                Id = Guid.Empty,
                NameEng = string.Empty,
                NameLocal = string.Empty,
                Code = string.Empty
            });
            var lst = new List<Areas.Membership.Model.LookupView>();
            foreach (var t in tmp)
            {
                lst.Add(new Areas.Membership.Model.LookupView
                {
                    Id = t.Id,
                    Name = t.Name,
                    Code = t.Code,
                    ParentId = t.ParentId
                });
            }
            return new JsonResult(lst);
        }
        [HttpPost]
        [Route("SaveLegalProfile")]
        public async Task<IActionResult> SaveLegalProfile(IFormCollection formCollection)
        {
            var message = string.Empty;
            var isValid = true;
            LegalProfile data = new();
            if (Guid.TryParse(formCollection["id"], out var idParse))
            {
                data.Id = idParse;
            }
            data.Title = formCollection["title"];
            data.DocumentNumber = formCollection["documentNumber"];
            if (int.TryParse(formCollection["legalDocumentTypeValue"], out var type))
            {
                data.LegalDocumentType = (LegalDocumentTypes)type;
            }
            if (int.TryParse(formCollection["statusValue"], out var status))
            {
                data.Status = (StatusEnum)status;
            }
            if (DateTime.TryParse(formCollection["expiryDateStr"], out var date))
            {
                data.ExpiryDate = date;
            }
            try
            {
                var file = formCollection.Files.FirstOrDefault(f => f.Name == "attachment");
                if (file != null)
                {
                    string extension = Path.GetExtension(Path.GetFileName(file.FileName));
                    data.FileExtension = extension;
                }
                var id = await _legalProfileRepository.Save(TheUser.Id, data);
                if (file != null)
                {
                    var fileName = file.FileName;
                    await SaveDocument(id, TheUser.Id, file);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Legal Profile Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpGet]
        [Route("GetLegalProfiles")]
        public IActionResult GetLegalProfiles(int status = -1)
        {
            var legalProfiles = _legalProfileRepository.UserLegalProfiles(TheUser.Id, status).OrderBy(s => s.Title);

            return Ok(legalProfiles);
        }
        [HttpGet]
        [Route("GetLegalProfileLookup")]
        public IActionResult GetLegalProfileLookup(int status = -1)
        {
            var legalProfiles = _legalProfileRepository.UserLegalProfileLookup(TheUser.Id, status);

            return Ok(legalProfiles);
        }
        [HttpGet]
        [Route("GetLegalProfile/{id}")]
        public async Task<IActionResult> GetLegalProfile(Guid id)
        {
            var legalProfile = await _legalProfileRepository.UserLegalProfile(TheUser.Id, id);

            return Ok(legalProfile);
        }

        [HttpPost]
        [Route("DeleteLegalProfile/{id}")]
        public async Task<IActionResult> DeleteLegalProfile(Guid id)
        {
            var absolutePath = Path.Combine(UserLegalProfileDocumentsAbsolutePath, Guid2String(TheUser.Id));
            var shortName = Guid2String(id);
            var result = Path.Combine(absolutePath, shortName);
            await _legalProfileRepository.Delete(TheUser.Id, id, result);
            return Ok(new { success = true });
        }
        [HttpPost]
        [Route("SaveExportAgreement")]
        public async Task<IActionResult> SaveExportAgreement(ExportAgreement data)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _exportAgreementRepository.Save(TheUser.Id, data);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Export Agreement Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpPost]
        [Route("SaveCommodityList/{id}")]
        public async Task<IActionResult> SaveCommodityList(Guid id, List<AgreementCommodity> data)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _exportAgreementRepository.SaveCommodityList(TheUser.Id, id, data);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Agreement Commodity Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpGet]
        [Route("GetExportAgreements")]
        public IActionResult GetExportAgreements(int status = -1)
        {
            var exportAgreements = _exportAgreementRepository.UserExportAgreements(TheUser.Id, status).OrderBy(s => s.AgreementNo);

            return Ok(exportAgreements);
        }
        [HttpGet]
        [Route("GetExportAgreementLookup")]
        public IActionResult GetExportAgreementLookup(int status = -1)
        {
            var exportAgreements = _exportAgreementRepository.UserExportAgreementLookup(TheUser.Id, status);

            return Ok(exportAgreements);
        }
        [HttpGet]
        [Route("GetExportAgreement/{id}")]
        public async Task<IActionResult> GetExportAgreement(Guid id)
        {
            var exportAgreement = await _exportAgreementRepository.UserExportAgreement(TheUser.Id, id);

            return Ok(exportAgreement);
        }

        [HttpPost]
        [Route("DeleteExportAgreement/{id}")]
        public async Task<IActionResult> DeleteExportAgreement(Guid id)
        {
            await _exportAgreementRepository.Delete(TheUser.Id, id);

            return Ok(new { success = true });
        }
        [HttpPost]
        [Route("SaveContact")]
        public async Task<IActionResult> SaveContact(UserContact data)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _userContactRepository.Save(TheUser.Id, data);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Contact Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }
        [HttpGet]
        [Route("GetContacts")]
        public IActionResult GetContacts()
        {
            var serviceRequests = _userContactRepository.UserContacts(TheUser.Id).OrderBy(s => s.Priority);

            return Ok(serviceRequests);
        }
        [HttpGet]
        [Route("GetContact/{id}")]
        public async Task<IActionResult> GetContact(Guid id)
        {
            var serviceRequest = await _userContactRepository.UserContact(TheUser.Id, id);

            return Ok(serviceRequest);
        }
        [HttpPost]
        [Route("DeleteContact/{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            await _userContactRepository.Delete(TheUser.Id, id);

            return Ok(new { success = true });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ApplicantById([FromRoute] Guid id)
        {
            var data = await _applicantRepository.ApplicantById(id, TheUser.Id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }
        [NonAction]
        protected async Task<String> SaveDocument(Guid id, Guid userId, IFormFile file)
        {
            var absolutePath = Path.Combine(UserLegalProfileDocumentsAbsolutePath, Guid2String(userId));
            var shortName = Guid2String(id);
            string fileName = file.FileName;
            string extension = Path.GetExtension(Path.GetFileName(fileName));
            var newFileName = String.Concat(shortName, extension);
            await SaveFile(absolutePath, newFileName, file, shortName);
            return UserLegalProfileDocumentsRelativePath + "/" + newFileName;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetSubscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            if (User == null || User.Identity == null || User.Identity.Name == null)
            {
                return Ok(false);
            }
            var subscriptions = await _tradeRepository.GetSubscriptions(TheUser.Id);

            return Ok(subscriptions);
        }
        [HttpPost]
        [Route("SaveSubscription")]
        public async Task<IActionResult> SaveSubscription(List<Subscription> data)
        {
            var message = string.Empty;
            var isValid = true;
            try
            {
                await _tradeRepository.ChangeSubscription(TheUser.Id, data);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isValid = false;
                _logger.LogError(ex, "Topic Subscription Save");
            }
            return Ok(new
            {
                saved = isValid,
                erorMsg = message
            });
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveApplicant([FromBody] ApplicantViewModel model)
        {
            var m = new Applicant()
            {
                Id = model.Id,
                Address = TheUser.Address,
                TIN = TheUser.TIN,
                MobileNo = TheUser.MobileNo,
                EmailAddress = TheUser.Email,
                NameEng = model.NameEng,
                ApplicationDate = model.ApplicationDate,
                ApplicationFee = model.ApplicationFee,
                AssociatedUserId = TheUser.Id,
                AssociationId = model.AssociationId,
                ApplicationStatus = model.ApplicationStatus,
                Responses = model.LegalDocuments

            };
            try
            {
                var ret = await _applicantRepository.Save(m, TheUser.Id);
                return CreatedAtAction("ApplicantById", new { id = ret.Id }, ret);


            }
            catch (Exception ex)
            {
                _logger.LogError("SaveApplicant", ex);
            }

            return BadRequest();
        }



        //[HttpGet]
        //public async Task<FileResult> DownLoadFile(Guid id, FolderTypeEnum type, string extension)
        //{
        //    var filePath = type == FolderTypeEnum.Document ? MembershipDocumentAbsolutePath : MembershipPhotoAbsolutePath;
        //    var fileName = String.Concat(Guid2String(id), extension);
        //    return await DownLoadFile(filePath, fileName);
        //}

        //[HttpGet]
        //public async Task<IActionResult> EditDocument(Guid id, Guid applicantId, Guid associationId)
        //{
        //    var userName = TheUser.FullName;

        //    var model = new ApplicantViewModel
        //    {
        //        FullName = userName,
        //        CurrentPage = "Applicants",
        //        CurrentOperation = "Detail",
        //        TheModel = new Applicant
        //        {
        //            Id = applicantId,
        //            AssociationId = associationId
        //        }
        //    };
        //    if (id == Guid.Empty)
        //    {
        //        model.Document = new ApplicantDocument
        //        {
        //            Id = Guid.Empty,
        //            ApplicantId = applicantId,

        //        };
        //    }
        //    else
        //    {
        //        model.Document = await _applicantRepository.ApplicantDocumentById(id);
        //    }
        //    model.DocumentTypes = _legalDocumentTypeRepository.LegalDocumentTypes(Guid.Empty).ToList();
        //    model.ChangeActive(MenuItemEnum.MembershipApplicant);
        //    return View(model);

        //}


        //[HttpPost]
        //public async Task<IActionResult> SaveDocument(ApplicantViewModel model)
        //{
        //    var m = model.Document;
        //    string extension = string.Empty;
        //    if (model.Photo.Length > 0)
        //    {
        //        extension = Path.GetExtension(Path.GetFileName(model.Photo.FileName));
        //    }
        //    try
        //    {
        //        ApplicantDocumentPayload payload;
        //        if (m.Id != Guid.Empty)
        //        {
        //            var uldi = new UpdateApplicantDocumentInput(m.Id, m.ApplicantId, m.LegalDocumentTypeId, m.Document, extension);
        //            payload = await _applicantRepository.Update(new GenericInput<UpdateApplicantDocumentInput>(uldi, TheUser.Id));

        //            if (model.Photo.Length > 0)
        //            {
        //                var newFileName = String.Concat(Guid2String(payload.PayLoad.Id), extension);
        //                await SaveFile(MembershipDocumentAbsolutePath, newFileName, model.Photo);
        //            }
        //        }

        //        CommandStatus(CommandStatusEnum.Saved);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["CommandStatus"] = ex.Message;
        //    }

        //    return RedirectToAction("Detail", new { id = model.TheModel.Id });
        //}
    }
}
