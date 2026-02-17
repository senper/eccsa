using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Payment
{
    public class Settlment
    {
        public Guid Id { get; set; }

        public Guid InvoiceId { get; set; }

        public String ReceiptNo { get; set; }

        public DateTime PaymentDate { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal AmountConfirmed { get; set; }

        public PaymentStatusEnum Status { get; set; }

        public string Extension { get; set; }

        public String Remark { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }



        public virtual Invoice Invoice { get; set; }

        [NotMapped]
        public string RelativePath { get; set; }
    }
}
