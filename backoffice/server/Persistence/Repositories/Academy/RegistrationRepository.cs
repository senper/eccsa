using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Academy
{
    public class RegistrationRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public RegistrationRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Registration> Registrations(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Registrations
.Include(x => x.Attendances)
.Include(x => x.ResourceDeliveries)
.Include(x => x.Assessments)
.Include(x => x.Training)
.Include(x => x.TrainingSponsor)
;
        }

        public IQueryable<Registration> RegistrationsSummary(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Registrations
.Include(x => x.Training).ThenInclude(s => s.Topic).ThenInclude(s => s.Category)
;
        }

        public async Task<Registration> RegistrationById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var reg = await _context.Registrations
.Include(x => x.Attendances)
.Include("ResourceDeliveries.Resource")
.Include(x => x.Assessments)
.Include(x => x.Training)
.Include(x => x.TrainingSponsor).ThenInclude(x => x.Sponsor)
.FirstOrDefaultAsync(i => i.Id == id);
            var training = await _context.Trainings.Include(x => x.Topic).FirstOrDefaultAsync(x => x.Id == reg.TrainingId);
            reg.Training = training;
            return reg;

        }
        public async Task<RegistrationPayload> Create(GenericInput<CreateRegistrationInput> input)
        {
            if(input.PayLoad.RegistrationDate > DateTime.Today)
            {
                throw new Exception("Registration date cannot be greater than today.");
            }
            Registration data = new()
            {
                Id = Guid.NewGuid()
            ,
                TrainingId = input.PayLoad.TrainingId
            ,
                LoginId = input.PayLoad.LoginId
            ,
                TrainingSponsorId = input.PayLoad.TrainingSponsorId
            ,
                FullName = input.PayLoad.FullName
            ,
                EmailAddress = input.PayLoad.EmailAddress
            ,
                MobileNo = input.PayLoad.MobileNo
            ,
                RegistrationDate = input.PayLoad.RegistrationDate
            ,
                RegistrationStatus = RegistrationStatus.Apply // input.PayLoad.RegistrationStatus
            //,
            //    TrainingRating = input.PayLoad.TrainingRating
            //,
            //    TrainingFeedback = input.PayLoad.TrainingFeedback
            //,
            //    CertificateAttachment = input.PayLoad.CertificateAttachment
            //,
            //    InvoiceNumber = input.PayLoad.InvoiceNumber
            ,
                DiscountCategory = input.PayLoad.DiscountCategory
            ,
                CreatedBy = input.UserId
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = input.UserId
            ,
                UpdateStamp = DateTime.UtcNow

            };
            await _context.Registrations.AddAsync(data);
            await _context.SaveChangesAsync();
            return new RegistrationPayload(data);
        }

        public async Task<RegistrationPayload> Update(GenericInput<UpdateRegistrationInput> data)
        {
            if (data.PayLoad.RegistrationDate > DateTime.Today)
            {
                throw new Exception("Registration date cannot be greater than today.");
            }
            var tmp = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Registration not found");
            tmp.TrainingId = data.PayLoad.TrainingId;
            tmp.LoginId = data.PayLoad.LoginId;
            tmp.TrainingSponsorId = data.PayLoad.TrainingSponsorId;
            tmp.FullName = data.PayLoad.FullName;
            tmp.EmailAddress = data.PayLoad.EmailAddress;
            tmp.MobileNo = data.PayLoad.MobileNo;
            tmp.RegistrationDate = data.PayLoad.RegistrationDate;
            //tmp.RegistrationStatus = data.PayLoad.RegistrationStatus;
            //tmp.TrainingRating = data.PayLoad.TrainingRating;
            //tmp.TrainingFeedback = data.PayLoad.TrainingFeedback;
            //tmp.CertificateAttachment = data.PayLoad.CertificateAttachment;
            //tmp.InvoiceNumber = data.PayLoad.InvoiceNumber;
            tmp.DiscountCategory = data.PayLoad.DiscountCategory;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.Registrations.Update(tmp);
            await _context.SaveChangesAsync();
            return new RegistrationPayload(tmp);

        }

        public async Task<RegistrationPayload> Update(GenericInput<UpdateRegistrationFeedbackInput> data)
        {
            var tmp = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Registration not found");
            tmp.TrainingRating = data.PayLoad.TrainingRating;
            tmp.TrainingFeedback = data.PayLoad.TrainingFeedback;
            tmp.CertificateAttachment = data.PayLoad.CertificateAttachment;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.Registrations.Update(tmp);
            await _context.SaveChangesAsync();
            return new RegistrationPayload(tmp);

        }

        public async Task<RegistrationPayload> GenerateInvoice(Guid id, string invoiceNo, YesNoEnum asMember, decimal rate, Guid userId)
        {
            
            var tmp = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Registration not found");
            var date = DateTime.Now;
            var dateNo = $"{date.Year}{date.Month:00}{date.Day:00}";
            tmp.InvoiceNumber = invoiceNo;
            tmp.InvoiceRate = rate;
            tmp.AsMember = asMember;
            tmp.RegistrationStatus = RegistrationStatus.InvoiceGenerated;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.Registrations.Update(tmp);
            await _context.SaveChangesAsync();
            return new RegistrationPayload(tmp);

        }

        public async Task RecordPayment(Guid requestId, string invoiceNo, string receiptNo, PaymentMethodEnum paymentMethod, string remark, Guid userId)
        {
            var tmp = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == requestId);
            if (tmp == null)
                return;
            if (tmp.RegistrationStatus == RegistrationStatus.Apply)
                return;
            if (tmp.InvoiceNumber != invoiceNo)
                return;
            tmp.ReceiptNo = receiptNo;
            tmp.PaymentMethod = paymentMethod;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            tmp.Remark = String.IsNullOrEmpty(remark) ? string.Empty : remark;
            tmp.RegistrationStatus =  RegistrationStatus.PaymentMade;
            _context.Registrations.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<RegistrationPayload> ChangeStatus(Guid id, Guid userId, RegistrationStatus status)
        {
            var tmp = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Registration not found");
            var date = DateTime.Now;
            tmp.RegistrationStatus = status;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.Registrations.Update(tmp);
            await _context.SaveChangesAsync();
            return new RegistrationPayload(tmp);

        }

        public async Task Delete(Registration data)
        {
            var tmp = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Registration not found");
            _context.Registrations.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}