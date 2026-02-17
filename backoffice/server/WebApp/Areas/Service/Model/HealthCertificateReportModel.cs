using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class HealthCertificateReportModel : IPrintReportModelWithDetail<object>
    {
        public HealthCertificateReportModel()
        {
            //CompanyAddress = "Tel: +251-115522083  Fax: +251-115517699  P.O.Box: 512 Email: ethchamber@ethionet.et, info@ethiopainchamber.com  Address: Addis Ababa Ethiopia";
        }
        public string RefNo { get; set; }
        public string Consignor { get; set; }
        public string ConsignorAddress { get; set; }
        public string Consignee { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ExportDeclarationNo { get; set; }
        public string InvoiceNo { get; set; }
        public string PlaceofInspection { get; set; }
        public string DateofInspection { get; set; }
        public string ProductGoodName { get; set; }
        public string NoofPackage { get; set; }
        public string GrossWeight { get; set; }
        public string NetWeight { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string AuthorizedBy { get; set; }
        public string AuthorizerTitle { get; set; }
        public string AuthorizationDate { get; set; }
        public string Signature { get; set; }
        public String CertificateNo { get; set; }
        public string ApproverName { get; set; }
        public string ApproverTitle { get; set; }
        public string ApproveredDate { get; set; }
        public string RequestDate { get; set; }
        public string Logo { get; set; }
        public string Watermark { get; set; } = string.Empty;
        public List<OptionalEntryForReport> OptionalEntries { get; set; }
        public List<object> Details { get; set; }
    }

    public class SupportLetterReportModel : IPrintReportModelWithDetail<object>
    {
        public SupportLetterReportModel()
        {
            //CompanyAddress = "Tel: +251-115522083  Fax: +251-115517699  P.O.Box: 512 Email: ethchamber@ethionet.et, info@ethiopainchamber.com  Address: Addis Ababa Ethiopia";
        }
        public string EmbassyName { get; set; }
        public string EmbassyAddress { get; set; }
        public string FullName { get; set; }
        public string DestinationCountry { get; set; }
        public string Watermark { get; set; } = string.Empty;


        
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string AuthorizedBy { get; set; }
        public string AuthorizerTitle { get; set; }
        public string AuthorizationDate { get; set; }
        public string Signature { get; set; }
        public string CertificateNo { get; set; }
        public string ApproverName { get; set; }
        public string ApproverTitle { get; set; }
        public string ApproveredDate { get; set; }
        public string RequestDate { get; set; }
        public string InvoiceNo { get; set; }
        public string Logo { get; set; }
        public List<OptionalEntryForReport> OptionalEntries { get; set; }
        public string SignatoryName { get; set; }
        public string SignatoryPosition { get; set; }


        public List<object> Details { get; set; }
    }
}
