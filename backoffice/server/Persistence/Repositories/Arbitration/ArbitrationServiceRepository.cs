using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Arbitration;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Arbitration;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Arbitration
{
    public class ArbitrationServiceRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ArbitrationServiceRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ArbitrationService> ArbitrationServices(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ArbitrationServices.Include(x => x.ServiceType)
.Include(x => x.Documents)
.Include(x => x.Followups)
.Include(x => x.RelatedParties)
;
        }

        public async Task<ArbitrationService> ArbitrationServiceById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ArbitrationServices.Include(x => x.ServiceType)
.Include(x => x.Documents)
.Include(x => x.Followups).ThenInclude(x => x.FollowupResult)
.Include(x => x.Followups).ThenInclude(x => x.TaskType)
.Include(x => x.RelatedParties)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ArbitrationServicePayload> Create(GenericInput<CreateArbitrationServiceInput> input)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == input.UserId);
            ArbitrationService data = new()
            {
                Id = Guid.NewGuid()
            ,
                AssociationId = user.AssociationId //input.PayLoad.AssociationId
                                                   //, RequestingBodyId = input.PayLoad.RequestingBodyId
            ,
                TIN = input.PayLoad.TIN
            ,
                FullName = input.PayLoad.FullName
            ,
                EmailAddress = input.PayLoad.EmailAddress
            ,
                MobileNo = input.PayLoad.MobileNo
            ,
                ServiceTypeId = input.PayLoad.ServiceTypeId
            ,
                RequestDate = input.PayLoad.RequestDate
            //, InvoiceNo = input.PayLoad.InvoiceNo
            //, ReceiptNo = input.PayLoad.ReceiptNo
            //, PaymentMethod = input.PayLoad.PaymentMethod
            //, RequestStatus = input.PayLoad.RequestStatus
            ,
                Description = input.PayLoad.Description
            ,
                CreatedBy = input.UserId
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = input.UserId
            ,
                UpdateStamp = DateTime.UtcNow

            };
            await _context.ArbitrationServices.AddAsync(data);
            var val = await _context.SaveChangesAsync();
            return new ArbitrationServicePayload(data);
        }

        public async Task<ArbitrationServicePayload> Update(GenericInput<UpdateArbitrationServiceInput> data)
        {
            var tmp = await _context.ArbitrationServices.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Arbitration Service not found");
            //						tmp.AssociationId = data.PayLoad.AssociationId;

            tmp.TIN = data.PayLoad.TIN;
            tmp.FullName = data.PayLoad.FullName;
            tmp.EmailAddress = data.PayLoad.EmailAddress;
            tmp.MobileNo = data.PayLoad.MobileNo;
            //tmp.RequestingBodyId = data.PayLoad.RequestingBodyId;
            tmp.ServiceTypeId = data.PayLoad.ServiceTypeId;
            tmp.RequestDate = data.PayLoad.RequestDate;
            //tmp.InvoiceNo = data.PayLoad.InvoiceNo;
            //tmp.ReceiptNo = data.PayLoad.ReceiptNo;
            //tmp.PaymentMethod = data.PayLoad.PaymentMethod;
            //tmp.RequestStatus = data.PayLoad.RequestStatus;
            tmp.Description = data.PayLoad.Description;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.ArbitrationServices.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationServicePayload(tmp);

        }

        public async Task<ArbitrationServicePayload> UpdatePayment(GenericInput<UpdateArbitrationServicePaymentInput> data)
        {
            var tmp = await _context.ArbitrationServices.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Arbitration Service not found");

            tmp.ReceiptNo = data.PayLoad.ReceiptNo;
            tmp.PaymentMethod = data.PayLoad.PaymentMethod;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.ArbitrationServices.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationServicePayload(tmp);

        }
        public async Task<ArbitrationServicePayload> GenerateInvoice(Guid id, string invoiceNo, Guid userId)
        {
            var tmp = await _context.ArbitrationServices.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Arbitration Service not found");
            var date = DateTime.Now;
            var dateNo = $"{date.Year}{date.Month:00}{date.Day:00}";
            //var lastInvoicNumber = _context.ArbitrationServices.Where(r =>
            //r.InvoiceNo.StartsWith(dateNo))
            //    .Select(r => r.InvoiceNo).OrderByDescending(r => r).FirstOrDefault() ??
            //    $"{dateNo}0000";
            tmp.InvoiceNo = invoiceNo;// $"{Int64.Parse(lastInvoicNumber) + 1}";
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.ArbitrationServices.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationServicePayload(tmp);

        }

        public async Task<ArbitrationServicePayload> ChangeStatus(GenericInput<UpdateArbitrationServiceStatusInput> data)
        {
            var tmp = await _context.ArbitrationServices.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Arbitration Service not found");

            tmp.RequestStatus = data.PayLoad.status;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.ArbitrationServices.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationServicePayload(tmp);
        }

        public async Task Delete(ArbitrationService data)
        {
            var tmp = await _context.ArbitrationServices.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Arbitration Service not found");
            _context.ArbitrationServices.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}