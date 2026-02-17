using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Payment;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Payment
{
    public class SettlmentRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public SettlmentRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Settlment> Settlments(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Settlments.Include(x => x.Invoice)
;
        }

        public async Task<Settlment> SettlmentById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Settlments.Include(x => x.Invoice)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<SettlmentPayload> Create(GenericInput<CreateSettlmentInput> input)
        {
            Settlment data = new()
            {
                Id = Guid.NewGuid()
            ,
                InvoiceId = input.PayLoad.InvoiceId
            ,
                ReceiptNo = input.PayLoad.ReceiptNo
            ,
                PaymentDate = input.PayLoad.PaymentDate
            ,
                PaymentMethod = input.PayLoad.PaymentMethod
            ,
                AmountPaid = input.PayLoad.AmountPaid
            ,
                Remark = input.PayLoad.Remark
            ,
                CreatedBy = input.UserId
            ,
                CreatedOn = input.PayLoad.CreatedOn

            };
            await _context.Settlments.AddAsync(data);
            var val = await _context.SaveChangesAsync();
            return new SettlmentPayload(data);
        }

        public async Task<SettlmentPayload> Update(GenericInput<UpdateSettlmentInput> data)
        {
            var tmp = await _context.Settlments.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            tmp.InvoiceId = data.PayLoad.InvoiceId;
            tmp.ReceiptNo = data.PayLoad.ReceiptNo;
            tmp.PaymentDate = data.PayLoad.PaymentDate;
            tmp.PaymentMethod = data.PayLoad.PaymentMethod;
            tmp.AmountPaid = data.PayLoad.AmountPaid;
            tmp.Remark = data.PayLoad.Remark;
            tmp.CreatedOn = data.PayLoad.CreatedOn;

            _context.Settlments.Update(tmp);
            await _context.SaveChangesAsync();
            return new SettlmentPayload(tmp);

        }

        public async Task Delete(Settlment data)
        {
            var tmp = await _context.Settlments.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Settlments.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}