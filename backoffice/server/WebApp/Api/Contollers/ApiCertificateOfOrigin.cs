using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiCertificateOfOrigin : BaseController
    {
        public ApiCertificateOfOrigin(
            UserManager<User> userManager,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
        }
        [HttpGet]
        public IActionResult List()
        {
            //var list = new List<CertificateOfOrigin>();
            //for (int i = 0; i < 40; i++)
            //{
            //    list.Add(new CertificateOfOrigin
            //    {
            //        ReferenceNo = i.ToString(),// Guid2String(Guid.NewGuid()),
            //        ReferenceDate = DateTime.UtcNow,
            //    });
            //}
            //return Ok(new
            //{
            //    list
            //});
            var jsonloc = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot", "json",
                                        "coo_f1.json");
            CertificateOfOrigin o1 = JsonConvert.DeserializeObject<CertificateOfOrigin>(System.IO.File.ReadAllText(jsonloc));
            return Ok(o1);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AnonymousList()
        {
            //var list = new List<CertificateOfOrigin>();
            //for (int i = 0; i < 30; i++)
            //{
            //    list.Add(new CertificateOfOrigin
            //    {
            //        ReferenceNo = Guid2String(Guid.NewGuid()) + "Anon",
            //        ReferenceDate = DateTime.UtcNow,
            //    });
            //}
            //return Ok(new
            //{
            //    list
            //});
            var jsonloc = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot", "json",
                                        "coo_f1.json");
            CertificateOfOrigin o1 = JsonConvert.DeserializeObject<CertificateOfOrigin>(System.IO.File.ReadAllText(jsonloc));
            return Ok(o1);
        }
    }

    //public class CertificateOfOrigin
    //{
    //    public string ReferenceNo { get; set; }
    //    public DateTime ReferenceDate { get; set; }
    //}

    public class Submitter
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }
    }

    public class AdditionalCode
    {
        [JsonPropertyName("ResultTypeCode")]
        public string ResultTypeCode { get; set; }
    }

    public class LPCOAuthorizedParty
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }
    }

    public class LPCO
    {
        [JsonPropertyName("EffectiveDateTime")]
        public string EffectiveDateTime { get; set; }

        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("TypeCode")]
        public string TypeCode { get; set; }

        [JsonPropertyName("LPCOAuthorizedParty")]
        public LPCOAuthorizedParty LPCOAuthorizedParty { get; set; }
    }

    public class AdditionalInformation
    {
        [JsonPropertyName("StatementCode")]
        public string StatementCode { get; set; }

        [JsonPropertyName("StatementTypeCode")]
        public string StatementTypeCode { get; set; }
    }

    public class CountQuantity
    {
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Classification
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }
    }

    public class NetNetWeightMeasure
    {
        [JsonPropertyName("unitCode")]
        public string UnitCode { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class GoodsMeasure
    {
        [JsonPropertyName("GrossMassMeasure")]
        public string GrossMassMeasure { get; set; }

        [JsonPropertyName("NetNetWeightMeasure")]
        public NetNetWeightMeasure NetNetWeightMeasure { get; set; }
    }

    public class InvoiceLine
    {
        [JsonPropertyName("ItemChargeAmount")]
        public string ItemChargeAmount { get; set; }

        [JsonPropertyName("SequenceNumeric")]
        public string SequenceNumeric { get; set; }
    }

    public class Commodity
    {
        [JsonPropertyName("CargoDescription")]
        public string CargoDescription { get; set; }

        [JsonPropertyName("CountQuantity")]
        public CountQuantity CountQuantity { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("ValueAmount")]
        public string ValueAmount { get; set; }

        [JsonPropertyName("Classification")]
        public Classification Classification { get; set; }

        [JsonPropertyName("GoodsMeasure")]
        public GoodsMeasure GoodsMeasure { get; set; }

        [JsonPropertyName("InvoiceLine")]
        public List<InvoiceLine> InvoiceLine { get; set; }
    }

    public class Origin
    {
        [JsonPropertyName("CountryCode")]
        public string CountryCode { get; set; }
    }

    public class Packaging
    {
        [JsonPropertyName("QuantityQuantity")]
        public string QuantityQuantity { get; set; }

        [JsonPropertyName("TypeCode")]
        public string TypeCode { get; set; }
    }

    public class GovernmentAgencyGoodsItem
    {
        [JsonPropertyName("SequenceNumeric")]
        public string SequenceNumeric { get; set; }

        [JsonPropertyName("AdditionalCode")]
        public AdditionalCode AdditionalCode { get; set; }

        [JsonPropertyName("AdditionalInformation")]
        public AdditionalInformation AdditionalInformation { get; set; }

        [JsonPropertyName("Commodity")]
        public Commodity Commodity { get; set; }

        [JsonPropertyName("Origin")]
        public Origin Origin { get; set; }

        [JsonPropertyName("Packaging")]
        public Packaging Packaging { get; set; }
    }

    public class Invoice1
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("IssueDateTime")]
        public string IssueDateTime { get; set; }

        [JsonPropertyName("TypeCode")]
        public string TypeCode { get; set; }
    }

    public class GoodsShipment
    {
        [JsonPropertyName("GovernmentAgencyGoodsItem")]
        public GovernmentAgencyGoodsItem GovernmentAgencyGoodsItem { get; set; }

        [JsonPropertyName("Invoice")]
        public Invoice1 Invoice { get; set; }
    }

    public class LoadingLocation
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }
    }

    public class ResponsibleGovernmentAgency
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("BranchCode")]
        public string BranchCode { get; set; }
    }

    public class Trader
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("LicenseID")]
        public string LicenseID { get; set; }

        [JsonPropertyName("LicenseTypeCode")]
        public string LicenseTypeCode { get; set; }
    }

    public class Address
    {
        [JsonPropertyName("CountryCode")]
        public string CountryCode { get; set; }
    }

    public class TraderPartner
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Address")]
        public Address Address { get; set; }
    }

    public class UnloadingLocation
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }
    }

    public class Declaration
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("GoodsShipment")]
        public GoodsShipment GoodsShipment { get; set; }

        [JsonPropertyName("LoadingLocation")]
        public LoadingLocation LoadingLocation { get; set; }

        [JsonPropertyName("ResponsibleGovernmentAgency")]
        public ResponsibleGovernmentAgency ResponsibleGovernmentAgency { get; set; }

        [JsonPropertyName("Trader")]
        public Trader Trader { get; set; }

        [JsonPropertyName("TraderPartner")]
        public TraderPartner TraderPartner { get; set; }

        [JsonPropertyName("UnloadingLocation")]
        public UnloadingLocation UnloadingLocation { get; set; }
    }

    public class CertificateOfOrigin
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }

        [JsonPropertyName("IssueDateTime")]
        public string IssueDateTime { get; set; }

        [JsonPropertyName("Submitter")]
        public Submitter Submitter { get; set; }

        [JsonPropertyName("AdditionalCode")]
        public AdditionalCode AdditionalCode { get; set; }

        [JsonPropertyName("LPCO")]
        public LPCO LPCO { get; set; }

        [JsonPropertyName("Declaration")]
        public Declaration Declaration { get; set; }
    }
}
