using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Payment
{
    public class Invoice
    {
        public static Invoice BlankInovice
        {
            get
            {
                return new Invoice()
                {
                    Id = Guid.Empty,
                    InvoiceDate = DateTime.UtcNow,
                    InvoiceNo = "INVALID",
                    Settlments = new List<Settlment>() { new Settlment()
                    {
                        Id = Guid.Empty,
                        InvoiceId = Guid.Empty,
                        ReceiptNo= "INVALID",
                        Status = PaymentStatusEnum.Accepted,
                        PaymentMethod = PaymentMethodEnum.NotSet            }
                    }
                };
            }
        }
        public Guid Id { get; set; }
        public ECCSAModuleFlag SubSystem { get; set; }

        public Guid ReferenceId { get; set; }

        public String CompanyName { get; set; }
        public string TIN { get; set; }
        public string Address { get; set; }

        public string PaymentReason { get; set; }
        public DateTime InvoiceDate { get; set; }

        public bool IsCreditService { get; set; }

        public int CreditDueDays { get; set; }

        public string InvoiceNo { get; set; }

        public Guid CreatedBy { get; set; }

        public string CurrencyId { get; set; }
        public decimal BasePrice { get; set; }

        public decimal Discount { get; set; }


        [NotMapped]
        public decimal VATAmount => TotalAmount - NetAmount;

        [NotMapped]
        public decimal NetAmount => BasePrice - Discount;

        public decimal TotalAmount { get; set; }

        public bool Cancelled { get; set; }

        public Guid CancelledBy { get; set; }

        public DateTime DateCancelled { get; set; }

        [NotMapped]
        public decimal DueRemaining
        {
            get
            {
                if (Settlments != null && Settlments.Count > 0)
                {
                    return TotalAmount - Settlments.Where(s => s.Status != PaymentStatusEnum.NotSet &&
                    s.Status != PaymentStatusEnum.Rejected).Sum(s => s.AmountConfirmed);
                }
                return TotalAmount;
            }
        }

        [NotMapped]
        public decimal TotalPaid
        {
            get
            {
                if (Settlments != null && Settlments.Count > 0)
                {
                    return Settlments.Where(s => s.Status != PaymentStatusEnum.NotSet &&
                    s.Status != PaymentStatusEnum.Rejected).Sum(s => s.AmountConfirmed);
                }
                return 0;
            }
        }
        public string TelePayUrl { get; set; }
        public string OutTradeNo { get; set; }
        public virtual ICollection<Settlment> Settlments { get; set; }
    }
}
