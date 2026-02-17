using ECCSA.BackOffice.Server.Model.Common.Servicing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class CommercialInvoice: IPrintReportModelWithDetail<CommercialInvoiceDetail>
    {
        public CommercialInvoice()
        {
            //CompanyAddress = "Tel: +251-115522083  Fax: +251-115517699  P.O.Box: 512 Email: ethchamber@ethionet.et, info@ethiopainchamber.com  Address: Addis Ababa Ethiopia";
        }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;

        public string ConsigneeName { get; set; } = string.Empty;
        public string ConsignorName { get; set; } = string.Empty;
        public string Importer { get; set; } = string.Empty;
        public string CountryOfOrigin { get; set; } = string.Empty;
        public string DateOfShipment { get; set; }

        public string PermitNo { get; set; } = string.Empty;
        public string CertificateOfOriginNo { get; set; } = string.Empty;


        public string AmountInWords => NumberToWords((double)Details.Sum(s => s.TPrice));
        public string TotalAmount => ((double)Details.Sum(s => s.TPrice)).ToString("n2") + " " + Details.FirstOrDefault().Currency;
        public string BankDetail { get; set; } = string.Empty;
        
        public List<CommercialInvoiceDetail> Details { get; set; }
        public string AuthorizedBy { get; set; }
        public string AuthorizerTitle { get; set; } = string.Empty;
        public string AuthorizationDate { get ; set; }
        public string Signature { get; set; }
        public String CertificateNo { get; set; }
        public string ApproverName { get; set; }
        public string ApproverTitle { get; set; }
        public string ApproveredDate { get; set; }
        public string RequestDate { get; set; }
        public string Logo { get; set; }
        public string Watermark { get; set; } = string.Empty;
        public List<OptionalEntryForReport> OptionalEntries { get; set; }

        public string NumberToWords(double doubleNumber)
        {
            int beforeFloatingPoint = (int)Math.Floor(doubleNumber);
            string beforeFloatingPointWord = string.Format("{0}", NumberToWords(beforeFloatingPoint));
            string afterFloatingPointWord = string.Format("{0}", SmallNumberToWord((int)((doubleNumber - beforeFloatingPoint) * 100), ""));
            if ((int)((doubleNumber - beforeFloatingPoint) * 100) > 0)
            {
                return string.Format("{0} and {1}", beforeFloatingPointWord, afterFloatingPointWord);
            }
            else
            {
                return string.Format("{0} only", beforeFloatingPointWord);
            }
        }

        private string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            var words = "";

            if (number / 1000000000 > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if (number / 1000000 > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if (number / 1000 > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if (number / 100 > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            words = SmallNumberToWord(number, words);

            return words;
        }

        private string SmallNumberToWord(int number, string words)
        {
            if (number <= 0) return words;
            if (words != "")
                words += " ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
            return words;
        }
    }

    public class CommercialInvoiceDetail
    {
        public string No { get; set; } = string.Empty;
        public string MarksNo { get; set; } = string.Empty;
        public int NoPackages { get; set; }
        public string TypeOfPackaging { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string UnitOfM { get; set; } = string.Empty;
        public int Qty { get; set; }
        public int GrossWeight { get; set; }
        public int NetWeight { get; set; }
        public decimal UPrice { get; set; }
        public String Currency { get; set; }
        public decimal TPrice { get; set; }

    }
}
