using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
	public class RenewalHistoryRepository 
	{
		private readonly EccsaContext _context;
		private readonly EccsaUserContext _userContext;
		public RenewalHistoryRepository(EccsaContext context, EccsaUserContext userContext)
		{
			_context = context;
			_userContext = userContext;
		}

		public IQueryable<RenewalHistory> RenewalHistory(Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return _context.RenewalHistory.Include(x => x.MembershipEnrollment)
;
		}

		public async Task<RenewalHistory> RenewalHistoryById(Guid id, Guid userId)
		{
			// var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
			return await _context.RenewalHistory.Include(x => x.MembershipEnrollment)
.FirstOrDefaultAsync(i => i.Id == id);
		}
		public async Task<RenewalHistoryPayload> Create(GenericInput<CreateRenewalHistoryInput> input)
		{
			RenewalHistory data = new()
			{
							Id = Guid.NewGuid()
			, EnrollmentId = input.PayLoad.EnrollmentId
			, ApplicationDate = DateTime.Today
			, RenewalDate = DateTime.MaxValue.AddYears(-999)
			, Fee = decimal.MaxValue
			, InvoiceNo = ""
			, InvoiceDate = DateTime.MaxValue.AddYears(-999)
			, ReceiptNo = ""
			, PaymentMethod = Model.Common.PaymentMethodEnum.NotSet
			, RenewalStatus = MembershipWorkflowEnum.NotSet

			};
			await _context.RenewalHistory.AddAsync(data);
		    await _context.SaveChangesAsync();
		   return new RenewalHistoryPayload(data);
		}

		public async Task<RenewalHistoryPayload> Update(GenericInput<UpdateRenewalHistoryInput> data)
		{
			var tmp = await  _context.RenewalHistory.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
									tmp.EnrollmentId = data.PayLoad.EnrollmentId;
			tmp.Fee = data.PayLoad.Fee;
			tmp.InvoiceNo = data.PayLoad.InvoiceNo;
			tmp.ReceiptNo = data.PayLoad.ReceiptNo;
			tmp.Remark = data.PayLoad.Remark;

			_context.RenewalHistory.Update(tmp);
			await _context.SaveChangesAsync();
			return new RenewalHistoryPayload(tmp);

		}

		public async Task<RenewalHistoryPayload> GenerateInvoice(GenericInput<UpdateRenewalHistoryInput> data)
		{
			var tmp = await _context.RenewalHistory.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			tmp.EnrollmentId = data.PayLoad.EnrollmentId;
			tmp.Fee = data.PayLoad.Fee;
			tmp.InvoiceNo = data.PayLoad.InvoiceNo;
			tmp.InvoiceDate = DateTime.UtcNow;
			tmp.RenewalStatus = MembershipWorkflowEnum.InvoiceGenerated;

			_context.RenewalHistory.Update(tmp);
			await _context.SaveChangesAsync();
			return new RenewalHistoryPayload(tmp);

		}

		public async Task<RenewalHistoryPayload> MakePayment(GenericInput<UpdateRenewalHistoryInput> data)
		{
			var tmp = await _context.RenewalHistory.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			tmp.PaymentDate = DateTime.UtcNow;
			tmp.PaymentMethod = data.PayLoad.PaymentMethod;
			tmp.ReceiptNo = data.PayLoad.ReceiptNo;

			_context.RenewalHistory.Update(tmp);
			await _context.SaveChangesAsync();
			return new RenewalHistoryPayload(tmp);

		}

		public async Task<RenewalHistoryPayload> ApproveReject(GenericInput<UpdateRenewalHistoryInput> data)
		{
			var tmp = await _context.RenewalHistory.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			if(tmp.RenewalStatus == MembershipWorkflowEnum.Accepted)
			{
				tmp.RenewalDate = DateTime.Today;
			}
			tmp.Remark = data.PayLoad.Remark;

			_context.RenewalHistory.Update(tmp);
			await _context.SaveChangesAsync();
			return new RenewalHistoryPayload(tmp);

		}

		public async Task Delete(RenewalHistory data)
		{
			var tmp = await _context.RenewalHistory.FirstOrDefaultAsync(r => r.Id == data.Id);
			if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
			_context.RenewalHistory.Remove(tmp);
			await _context.SaveChangesAsync();
		}
	}
}