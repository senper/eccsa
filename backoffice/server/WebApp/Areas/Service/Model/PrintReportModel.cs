using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public interface IPrintReportModel
    {
        String CompanyName { get; set; }
        String CompanyAddress { get; set; }
        String AuthorizedBy { get; set; }
        String AuthorizerTitle { get; set; }
        String AuthorizationDate { get; set; }
        String Signature { get; set; }
        String CertificateNo { get;set; }
        public string ApproverName { get; set; }
        public string ApproverTitle { get; set; }
        public string ApproveredDate { get; set; }
        public string RequestDate { get; set; }
        public string InvoiceNo { get; set; }
        public string Logo { get; set; }
        public string Watermark { get; set; }
        public List<OptionalEntryForReport> OptionalEntries { get; set; }
    }
    public interface IPrintReportModelWithDetail<D> : IPrintReportModel
    {
        List<D> Details { get; set; }
    }

    public class OptionalEntryForReport
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public string Label1 { get; set; }
        public string Value1 { get; set; }
    }
}
