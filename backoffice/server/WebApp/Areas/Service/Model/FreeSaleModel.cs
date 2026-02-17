using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class FreeSaleModel : IPrintReportModelWithDetail<FreeSaleModelDetail>
    {
        public FreeSaleModel()
        {
            //CompanyAddress = "Tel: +251-115522083  Fax: +251-115517699  P.O.Box: 512 Email: ethchamber@ethionet.et, info@ethiopainchamber.com  Address: Addis Ababa Ethiopia";
        }
        public string Laws { get; set; } = string.Empty;
        public string DestinationCountries { get; set; } = string.Empty;
        public string PositionOfUndersigned => ApproverName; //{ get; set; } = string.Empty;
        public string RequestorTitle { get; set; } = string.Empty;
        public string RequestorName { get; set; } = string.Empty;
        public string Day => RequestDate;
        public string CompanyName { get; set; }
        public string CompanyProduct { get; set; }
        public string CompanyAddress { get; set; }
        public string AuthorizedBy { get; set; }
        public string AuthorizerTitle { get; set; }
        public string AuthorizationDate { get; set; }

        public string ApproverName { get; set; }
        public string ApproverTitle { get; set; }
        public string ApproveredDate { get; set; }
        public string RequestDate { get; set; }
        public string Signature { get; set; }
        public String CertificateNo { get; set; }
        public string Logo { get; set; }
        public string Watermark { get; set; } = string.Empty;
        public List<OptionalEntryForReport> OptionalEntries { get; set; }
        public List<FreeSaleModelDetail> Details { get; set; } = new List<FreeSaleModelDetail>();
        public string InvoiceNo { get; set; }
    }
    public class FreeSaleModelDetail
    {
        public string ProductName { get; set; } = string.Empty;
    }
}
