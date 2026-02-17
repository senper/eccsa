using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class PackingListModel : IPrintReportModelWithDetail<PackingListModelDetail>
    {
        public PackingListModel()
        {
            //CompanyAddress = "Tel: +251-115522083  Fax: +251-115517699  P.O.Box: 512 Email: ethchamber@ethionet.et, info@ethiopainchamber.com  Address: Addis Ababa Ethiopia";
        }
        public string ShipperSeller { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string VATRegNo { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public string ShippedBy { get; set; } = string.Empty;
        public string ConsigneeBuyer { get; set; } = string.Empty;
        public string ConsigneeFullAddress { get; set; } = string.Empty;
        public string ConsigneeTel { get; set; } = string.Empty;
        public string ConsigneeFax { get; set; } = string.Empty;
        public string ConsigneeEmail { get; set; } = string.Empty;
        public string ConsigneeVATRegNo { get; set; } = string.Empty;
        public string ConsigneePlaceofdeliveryFullAddress { get; set; } = string.Empty;
        public string InvoiceDate { get; set; } = string.Empty;
        public List<PackingListModelDetail> Details { get; set; } = new List<PackingListModelDetail>();
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
    }
    public class PackingListModelDetail
    {
        public string DescriptionoftheGoods { get; set; } = string.Empty;
        public decimal Noofpackages { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
    }
}
