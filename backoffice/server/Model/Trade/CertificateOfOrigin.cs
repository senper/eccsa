using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Trade
{
    public class Commodity
    {
        public long Id { get; set; }
        //[Newtonsoft.Json.JsonProperty("cmdtSn")] // Commodity Serial No
        public int SerialNo { get; set; }

       // [Newtonsoft.Json.JsonProperty("orginlCmdtSn")] //Original Commodity Sequence Number  
        public string OriginalSequenceNumber { get; set; }

       // [Newtonsoft.Json.JsonProperty("godsDesc")] // Goods Description
        public string GoodsDescription { get; set; }

        //[Newtonsoft.Json.JsonProperty("qty")] // Quantity 
        public decimal Quantity { get; set; }

       // [Newtonsoft.Json.JsonProperty("qtyCd")] // Unit of Measurement
        public string QuantityUnitCode { get; set; }

        //[Newtonsoft.Json.JsonProperty("commNm")] // Common Name
        public string CommonName { get; set; }

       // [Newtonsoft.Json.JsonProperty("cmdtAmt")] // Commodity Amount
        public string CommodityAmount { get; set; }

       // [Newtonsoft.Json.JsonProperty("cmdtHsCd")] // HS Code
        public string HSCode { get; set; }

       // [Newtonsoft.Json.JsonProperty("cmdtSpcfnCd")] // Commodity Description
        public string SpecificationCode { get; set; }

        //[Newtonsoft.Json.JsonProperty("gwght")]   // Gross Weight Unit Code
        public string GrossWeight { get; set; }

      //  [Newtonsoft.Json.JsonProperty("gWghtUnitCd")]   // Gross Weight Unit Code
        public string GrossWeightCode { get; set; }

      //  [Newtonsoft.Json.JsonProperty("nwght")] // Net Weight
        public string NetWeight { get; set; }

      //  [Newtonsoft.Json.JsonProperty("wghtUnitCd")]
        public string NetWeightCode { get; set; }

      //  [Newtonsoft.Json.JsonProperty("invcAmt")]
        public string InvoiceAmount { get; set; }

      //  [Newtonsoft.Json.JsonProperty("invcAmtTpCd")]
        public string InvoiceAmountCode { get; set; }

     //   [Newtonsoft.Json.JsonProperty("etbInvcAmt")]
        public string InvoiceAmountEtb { get; set; }

       // [Newtonsoft.Json.JsonProperty("etbInvcAmtTpCd")]
        public string InvoiceAmountEtbCode { get; set; }

       // [Newtonsoft.Json.JsonProperty("orcyCd")]
        public string OrcyCd { get; set; }

      //  [Newtonsoft.Json.JsonProperty("pkgCnt")]
        public string PkgCnt { get; set; }

      //  [Newtonsoft.Json.JsonProperty("pkgTpCd")]
        public string PkgTpCd { get; set; }

     //   [Newtonsoft.Json.JsonProperty("originCrtrn")]
        public string OriginCrtrn { get; set; }
        public long CertificateOfOriginId { get; set; }
        public virtual CertificateOfOrigin CertificateOfOrigin { get; set; }
    }
    public class CertificateOfOrigin
    {
        [NotMapped]
        public static readonly Guid CertificateOfOriginId = new("d7571107-ab29-43b3-b95c-c0ae12505a61");
        public long Id { get; set; }
    //    [Newtonsoft.Json.JsonProperty("lpcoNo")] //Unique
        public string LPCONo { get; set; }
   //     [Newtonsoft.Json.JsonProperty("apfmNo")] //ECCSA
        public string ApfMNo { get; set; }
  //      [Newtonsoft.Json.JsonProperty("apfmRefno")] //ECCSA
        public string ApfRefNo { get; set; }
     //   [Newtonsoft.Json.JsonProperty("apfmRgsrDt")] //ECCSA
        public string RegDate { get; set; }
      //  [Newtonsoft.Json.JsonProperty("cbraCd")] //ECCSA
        public string CbraCd { get; set; }

      //  [Newtonsoft.Json.JsonProperty("cbraBrnchCd")] // Branch code for ECCSA00, ECCSA01, ...
        public string CbraBrnchCd { get; set; }

       // [Newtonsoft.Json.JsonProperty("trdTin")] // Trader TIN
        public string TraderTIN { get; set; }

     //   [Newtonsoft.Json.JsonProperty("trdParCntryCd")]
        public string TraderPartnerCode { get; set; }

      //  [Newtonsoft.Json.JsonProperty("aplcTinAgencyID")] //Applicant Agent TIN Id
        public string AplcTinAgencyID { get; set; }

      //  [Newtonsoft.Json.JsonProperty("aplcCompTin")] // Applicant Agent TIN
        public string AplcCompTin { get; set; }

      //  [Newtonsoft.Json.JsonProperty("aplcIdAgencyID")]
        public string AplcIdAgencyID { get; set; }

     //   [Newtonsoft.Json.JsonProperty("aplcId")] // Applicant Id
        public string ApplicantId { get; set; }

      //  [Newtonsoft.Json.JsonProperty("crrerNm")] // Name of Carrier
        public string CarrierName { get; set; }

      //  [Newtonsoft.Json.JsonProperty("crrerNmCd")]// Name of Carrier Code
        public string CarrierNameCode { get; set; }

     //   [Newtonsoft.Json.JsonProperty("aditDclrCn")] //OtherCondition , additional declaration comment
        public string AdditionalDeclarationComment { get; set; }

      //  [Newtonsoft.Json.JsonProperty("aditDclrCnCd")] // Code for the additional declaration
        public string AdditionalDeclarationCode { get; set; }

     //   [Newtonsoft.Json.JsonProperty("dptrTrmnCd")] // Type of Carriage
        public string DptrTrmnCd { get; set; }

      //  [Newtonsoft.Json.JsonProperty("costAmt")] // Cost Amount 
        public string CostAmount { get; set; }

      //  [Newtonsoft.Json.JsonProperty("costTypeCd")] // Currency Code
        public string CostTypeCurrencyCode { get; set; }

       // [Newtonsoft.Json.JsonProperty("frgtForgAmt")] // Freight Foreign Amount
        public string FreightForeignAmount { get; set; }

      //  [Newtonsoft.Json.JsonProperty("frgtTypeCd")] // Freight Currency Code
        public string FreightCurrencyCode { get; set; }

      //  [Newtonsoft.Json.JsonProperty("insrncAmt")] // Insurance Amount
        public string InsuranceAmount { get; set; }

       // [Newtonsoft.Json.JsonProperty("insrncTypeCd")] // Insurance Currency Code
        public string InsuranceCurrencyCode { get; set; }

      //  [Newtonsoft.Json.JsonProperty("othChrgeAmt")] // Other Charge// not null
        public string OtherCharge { get; set; }

      //  [Newtonsoft.Json.JsonProperty("othTypeCd")]
        public string OtherChargeCode { get; set; }

     //   [NotMapped]
      //  [Newtonsoft.Json.JsonProperty("cmdtList")]
        public virtual List<Commodity> CommodityList { get; set; }

      //  [Newtonsoft.Json.JsonProperty("invcNo")] // InvoiceNumber
        public string InvoiceNumber { get; set; }

     //   [Newtonsoft.Json.JsonProperty("invcDt")] // Invoice Date  yyyyMMdd
        public string InvoiceDate { get; set; }

     //   [Newtonsoft.Json.JsonProperty("invcTpCd")] // InvoiceTypeCode 1-Proforma 2 - commercial
        public string InvoiceTypeCode { get; set; }

    //    [Newtonsoft.Json.JsonProperty("dtermCd")] // Terms of Delivery
        public string TermsOfDelivery { get; set; }

    //    [Newtonsoft.Json.JsonProperty("ptexitCd")]  // Port of Exit ETADD ETMDP ET
        public string PortOfExitCode { get; set; }

    //    [Newtonsoft.Json.JsonProperty("lcneNo")]   // Business
        public string LicenseNo { get; set; }

     //   [Newtonsoft.Json.JsonProperty("lcneTpCd")] // Trade License Type, BL, IP, MP Business License, Investment Permit, MiningPermit
        public string TradeLicenseType { get; set; }

     //   [Newtonsoft.Json.JsonProperty("trdParSn")] // TraderPartner Sequence Number
        public string TraderPartnerSequenceNo { get; set; }

    //    [Newtonsoft.Json.JsonProperty("imexCntryCd")] // Country of Destination DE, KR, ZA
        public string CountryOfDestinationCode { get; set; }

     //   [Newtonsoft.Json.JsonProperty("ptentCd")] // Port of Destination
        public string PortOfDestinationCode { get; set; }

    //    [Newtonsoft.Json.JsonProperty("dptrTrmnNm")] // Type of Carriage Name
        public string CarriageName { get; set; }

     //   [Newtonsoft.Json.JsonProperty("ieCntryNm")]// Export Destination Name
        public string DestinationCountryName { get; set; }

     //   [Newtonsoft.Json.JsonProperty("ptentNm")] // Port of Destination Name
        public string PortOfDestinationName { get; set; }

     //   [Newtonsoft.Json.JsonProperty("ptexitNm")] // Port of Exit Name
        public string PortOfExit { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdNm")] // Consignee Info
        public string ConsigneeCompany { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdRppnNm")] // Consignee Info
        public string ConsigneeName { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdTelNo")] // Consignee Info
        public string ConsigneeTelNo { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdAddr")] // Consignee Info
        public string ConsigneeAddress { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdHno")] // Consignee Info
        public string ConsigneeHouseNo { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdPobox")] // Consignee Info
        public string ConsigneePoBox { get; set; }
        public string ConsigneeTin => TraderTIN;// Consignee Info
        //   [Newtonsoft.Json.JsonProperty("trdParNm")] // Consignor Info
        public string ConsignorCompany { get; set; }
        public string ConsignorCountryCode => TraderPartnerCode; // Consignor Info
        //   [Newtonsoft.Json.JsonProperty("trdParCntryNm")] // Consignor Info
        public string ConsignorCountryName { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdParAddr")] // Consignor Info
        public string ConsignorAddress { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdParPlcCd")] // Consignor Info
        public string ConsignorPlaceCode { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdParCityNm")] // Consignor Info
        public string ConsignorCityName { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdParTelNo")] // Consignor Info
        public string ConsignorTelephone { get; set; }
        //   [Newtonsoft.Json.JsonProperty("trdParEl")] // Consignor Info
        public string ConsignorEmail { get; set; }
        public WorkflowStateEnum Status { get; set; }

        public DateTime ImportDate { get; set; }
    }

    public class ESWRequest
    {
        public DateTime RequestDate { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        [NotMapped]
        public string Status => Success ? "" : "- Failed";
    }
}
