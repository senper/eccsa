using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Payment
{
    public class InvoiceRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public InvoiceRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Invoice> Invoices(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Invoices.Include(x => x.Settlments)
;
        }

        public async Task<Invoice> InvoiceById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Invoices.Include(x => x.Settlments)
.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice> InvoiceByNo(string invoiceNo, Guid referenceId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            if(Invoice.BlankInovice.InvoiceNo == invoiceNo)
            {
                var inv = Invoice.BlankInovice;
                inv.ReferenceId = referenceId;
                return inv;
            }
            return await _context.Invoices.Include(i => i.Settlments)
                        .FirstOrDefaultAsync(i => i.InvoiceNo == invoiceNo && i.ReferenceId == referenceId);
        }
        public async Task<Invoice> InvoiceByOutTradeNo(string outTradeNo)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Invoices.Include(i => i.Settlments)
                        .FirstOrDefaultAsync(i => i.OutTradeNo == outTradeNo);
        }
        public async Task SaveTeleOutTradeNo(string invoiceNo, Guid referenceId, string outTradeNo, string url)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var invoice = await _context.Invoices.Include(i => i.Settlments)
                        .FirstOrDefaultAsync(i => i.InvoiceNo == invoiceNo && i.ReferenceId == referenceId);
            if (invoice == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(outTradeNo))
            {
                throw new InvalidOperationException("Invalid pay url");
            }
            invoice.OutTradeNo = outTradeNo;
            invoice.TelePayUrl = url;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Invoice>> InvoicesByRef(ECCSAModuleFlag subSystem, Guid referenceId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Invoices.Include(i => i.Settlments)
                        .Where(i => i.SubSystem == subSystem && i.ReferenceId == referenceId).ToListAsync();
        }

        public async Task<Settlment> MakeAutoUserPayment(string invoiceNo
            , decimal amount
            , DateTime paymentDate
            , String receiptNo
            , PaymentMethodEnum paymentMethod)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException($"Invalid payment amount.");
            }
            receiptNo = receiptNo.ToUpperInvariant().Replace(" ", "").Trim();
            var invoice = await _context.Invoices.Include(i => i.Settlments).FirstOrDefaultAsync(i => i.InvoiceNo == invoiceNo);
            if (invoice == null) throw new KeyNotFoundException("Invoice  not found");
            var settlement = await _context.Settlments.Include(s => s.Invoice).FirstOrDefaultAsync(s => s.ReceiptNo == receiptNo);

            if (settlement != null && invoice.Id != settlement.InvoiceId)
            {
                throw new InvalidOperationException($"Receipt {settlement.ReceiptNo} already used.");
            }
            if (settlement != null)
            {
                if (settlement.Status == PaymentStatusEnum.Rejected)
                {
                    settlement.Status = PaymentStatusEnum.Accepted;
                    settlement.AmountPaid = amount;
                    settlement.AmountConfirmed = amount;
                    settlement.Extension = "";
                    settlement.PaymentDate = paymentDate;
                    settlement.PaymentMethod = paymentMethod;
                    _context.Settlments.Update(settlement);
                    await _context.SaveChangesAsync();
                    return settlement;
                }
                throw new InvalidOperationException($"Receipt {settlement.ReceiptNo} already used!");
            }
            settlement = new Settlment
            {
                Id = Guid.NewGuid(),
                Status = PaymentStatusEnum.Accepted,
                AmountPaid = amount,
                AmountConfirmed = amount,
                InvoiceId = invoice.Id,
                PaymentDate = paymentDate,
                PaymentMethod = paymentMethod,
                ReceiptNo = receiptNo,
                CreatedBy = Guid.Empty,
                CreatedOn = DateTime.UtcNow,
                Remark = "",
                Extension = "",
            };
            _context.Settlments.Add(settlement);

            await _context.SaveChangesAsync();
            settlement.Invoice = invoice;
            return settlement;

        }
        public async Task<Settlment> MakeUserPayment(Guid id
            , string invoiceNo
            , decimal amount
            , DateTime paymentDate
            , String receiptNo
            , PaymentMethodEnum paymentMethod
            , string remark
            , string receiptExtention
            , Guid userId)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException($"Invalid payment amount.");
            }
            receiptNo = receiptNo.ToUpperInvariant().Replace(" ", "").Trim();
            var invoice = await _context.Invoices.Include(i => i.Settlments).FirstOrDefaultAsync(i => i.ReferenceId == id && i.InvoiceNo == invoiceNo);
            if (invoice == null) throw new KeyNotFoundException("Invoice  not found");
            var settlement = await _context.Settlments.Include(s => s.Invoice).FirstOrDefaultAsync(s => s.ReceiptNo == receiptNo);

            if (settlement != null && invoice.Id != settlement.InvoiceId)
            {
                throw new InvalidOperationException($"Receipt {settlement.ReceiptNo} already used.");
            }
            if (settlement != null)
            {
                if (settlement.Status == PaymentStatusEnum.Rejected)
                {
                    settlement.Status = PaymentStatusEnum.NotSet;
                    settlement.AmountPaid = amount;
                    settlement.Extension = receiptExtention;
                    settlement.PaymentDate = paymentDate;
                    settlement.PaymentMethod = paymentMethod;
                    _context.Settlments.Update(settlement);
                    await _context.SaveChangesAsync();
                    return settlement;
                }
                throw new InvalidOperationException($"Receipt {settlement.ReceiptNo} already used!");
            }
            //if (invoice.DueRemaining < 0.01M && invoice.Settlments.Count > 0)
            //{
            //    settlement = invoice.Settlments.LastOrDefault(w => w.AmountPaid > 0);
            //    return settlement;
            //}
            settlement = new Settlment
            {
                Id = Guid.NewGuid(),
                AmountPaid = amount,
                InvoiceId = invoice.Id,
                PaymentDate = paymentDate,
                PaymentMethod = paymentMethod,
                ReceiptNo = receiptNo,
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow,
                Remark = remark,
                Extension = receiptExtention,
            };
            _context.Settlments.Add(settlement);

            await _context.SaveChangesAsync();
            settlement.Invoice = invoice;
            return settlement;

        }

        public async Task<Settlment> ConfirmPayment(Guid id
           , string invoiceNo
           , String receiptNo
            , decimal confirmedAmount
           , Guid userId)
        {
            receiptNo = receiptNo.ToUpperInvariant().Replace(" ", "").Trim();
            var invoice = await _context.Invoices.Include(i => i.Settlments).FirstOrDefaultAsync(i => i.ReferenceId == id && i.InvoiceNo == invoiceNo);
            if (invoice == null) throw new KeyNotFoundException("Invoice  not found");
            var settlement = await _context.Settlments.Include(s => s.Invoice).FirstOrDefaultAsync(s => s.ReceiptNo == receiptNo);

            if (settlement == null)
            {
                throw new KeyNotFoundException("Receipt  not found");
            }
            if (settlement != null && invoice.Id != settlement.InvoiceId)
            {
                throw new InvalidOperationException($"Receipt {settlement.ReceiptNo} already used.");
            }


            settlement.AmountConfirmed = confirmedAmount;
            if (confirmedAmount < invoice.DueRemaining)
                settlement.Status = PaymentStatusEnum.Partial;
            else
                settlement.Status = PaymentStatusEnum.Accepted;
            _context.Settlments.Update(settlement);

            await _context.SaveChangesAsync();
            settlement.Invoice = invoice;
            return settlement;

        }
        public async Task<Settlment> RejectReceipt(Guid id
           , string invoiceNo
           , String receiptNo
           , Guid userId)
        {
            receiptNo = receiptNo.ToUpperInvariant().Replace(" ", "").Trim();
            var invoice = await _context.Invoices.Include(i => i.Settlments).FirstOrDefaultAsync(i => i.ReferenceId == id && i.InvoiceNo == invoiceNo);
            if (invoice == null) throw new KeyNotFoundException("Invoice  not found");
            var settlement = await _context.Settlments.Include(s => s.Invoice).FirstOrDefaultAsync(s => s.ReceiptNo == receiptNo);

            if (invoice == null) throw new KeyNotFoundException("Invoice  not found");
            if (settlement == null)
            {
                throw new KeyNotFoundException("Receipt  not found");
            }
            if (settlement != null && invoice.Id != settlement.InvoiceId)
            {
                throw new InvalidOperationException($"Receipt {settlement.ReceiptNo} already used.");
            }

            settlement.Status = PaymentStatusEnum.Rejected;
            _context.Settlments.Update(settlement);

            await _context.SaveChangesAsync();
            settlement.Invoice = invoice;
            return settlement;

        }

        private string GenerateInvoiceNo(int count)
        {
            var formattedCount = $"0000{count + 1}";
            var random = $"000{ (new Random()).Next(1, 999)}";
            formattedCount = formattedCount.Substring(formattedCount.Length - 5);
            random = random.Substring(random.Length - 3);
            var invoiceNo = $"{DateTime.UtcNow.ToString("yyMMddHHmm")}{formattedCount}{random}";
            long no = long.Parse(invoiceNo);
            var newInvoiceNo = BaseConverter.GetCode(no);
            return newInvoiceNo;
        }


        public async Task<Invoice> GetOrCreate(Invoice input, Guid userId)
        {
            var m = input;
            if(input.TotalAmount <= 0.01M)
            {
                var t = Invoice.BlankInovice;
                t.SubSystem = input.SubSystem;
                t.ReferenceId = input.ReferenceId;
                t.CreatedBy = userId;
                return t;
            }
            var invoices = await InvoicesByRef(input.SubSystem, input.ReferenceId);
            if (invoices != null && invoices.Count > 0)
            {
                var inv = invoices.FirstOrDefault(i => !i.Cancelled);
                if (inv != null)
                    return inv;
            }
            var tmp = await _context.Invoices.Where(r => r.InvoiceDate == DateTime.Today).ToListAsync();
            input.Id = Guid.NewGuid();
            input.InvoiceNo = GenerateInvoiceNo(tmp.Count); ;
            input.CreatedBy = userId;
            input.InvoiceDate = DateTime.UtcNow;
            if (input.TotalAmount <= 0.01M)
            {
                return Invoice.BlankInovice;
                throw new InvalidOperationException("Cannot generate Invoice with amount 0!");
            }
            await _context.Invoices.AddAsync(input);
            var val = await _context.SaveChangesAsync();
            return input;
        }



        public async Task Delete(Invoice data)
        {
            var tmp = await _context.Invoices.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            tmp.Cancelled = true;
            tmp.DateCancelled = DateTime.UtcNow;
            _context.Invoices.Update(tmp);
            await _context.SaveChangesAsync();
        }
    }
}