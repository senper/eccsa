using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.WebApp.Models
{
    public class PrintInvoice
    {

        public PrintInvoice()
        {

        }
        public PrintInvoice(Invoice invoice)
        {
            PaymentReason = invoice.PaymentReason;
            CompanyName = invoice.CompanyName;
            TIN = invoice.TIN;
            Address = invoice.Address;
            InvoiceDate = invoice.InvoiceDate;
            IsCreditService = invoice.IsCreditService;
            CreditDueDays = invoice.CreditDueDays;
            InvoiceNo = invoice.InvoiceNo;
            DiscountRate = invoice.Discount;
            BasePrice = invoice.BasePrice;
            Total = invoice.TotalAmount;
            var count = 1;
            Settlements = invoice.Settlments.Where(s => s.Status == PaymentStatusEnum.Accepted ||
            s.Status == PaymentStatusEnum.Partial).Select(s => new PrintSettlement
            {
                No = count++,
                ReceiptNo = s.ReceiptNo,
                TransactionDate = s.PaymentDate,
                PaymentMethod = s.PaymentMethod,
                AmountPaid = s.AmountConfirmed
            }).ToList();
        }
        public String PaymentReason { get; set; }

        public String CompanyName { get; set; }

        public String TIN { get; set; }

        public String Address { get; set; }

        public DateTime InvoiceDate { get; set; }

        public String IssueDate => InvoiceDate.ToString("MMMM dd, yyyy");

        public bool IsCreditService { get; set; }

        public int CreditDueDays { get; set; }

        public string InvoiceNo { get; set; }

        public decimal BasePrice { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal Discount => decimal.Round(BasePrice * DiscountRate, 2, MidpointRounding.ToZero);
        public decimal NetAmount => BasePrice - Discount;


        public decimal VAT => Total - NetAmount;


        public decimal Total {get;set;}


        public List<PrintSettlement> Settlements { get; set; }

        public decimal Balance
        {
            get
            {
                return NetAmount - Settlements.Sum(x => x.AmountPaid);
            }
        }

    }

    public class PrintSettlement
    {
        public int No { get; set; }
        public String ReceiptNo { get; set; }

        public DateTime TransactionDate { get; set; }

        public String PaymentDate => TransactionDate.ToString("MMMM dd, yyyy");
        public string Gateway
        {
            get
            {
                switch (PaymentMethod)
                {
                    case PaymentMethodEnum.Manual:
                        return "Manual Payment";

                    case PaymentMethodEnum.MobileTransfer:
                        return "Mobile";
                    case PaymentMethodEnum.NotSet:
                        return "Not Set";
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public PaymentMethodEnum PaymentMethod { get; set; }

        public decimal AmountPaid { get; set; }
    }
}
