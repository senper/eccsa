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
    public class ArbitrationDocumentRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ArbitrationDocumentRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<ArbitrationDocument> ArbitrationDocuments(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.ArbitrationDocuments.Include(x => x.ArbitrationService)
;
        }

        public async Task<ArbitrationDocument> ArbitrationDocumentById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.ArbitrationDocuments.Include(x => x.ArbitrationService)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<ArbitrationDocumentPayload> Create(GenericInput<CreateArbitrationDocumentInput> input)
        {
            ArbitrationDocument data = new()
            {
                			Id = Guid.NewGuid()
			, ServiceId = input.PayLoad.ServiceId
			, OrderNo = input.PayLoad.OrderNo
			, DocumentTypeId = input.PayLoad.DocumentTypeId
			, Name = input.PayLoad.Name
			, Content = input.PayLoad.Content
			, DateSubmitted = input.PayLoad.DateSubmitted

            };
            await _context.ArbitrationDocuments.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new ArbitrationDocumentPayload(data);
        }

        public async Task<ArbitrationDocumentPayload> Update(GenericInput<UpdateArbitrationDocumentInput> data)
        {
            var tmp = await  _context.ArbitrationDocuments.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Document not found");
            						tmp.ServiceId = data.PayLoad.ServiceId;
			tmp.OrderNo = data.PayLoad.OrderNo;
			tmp.DocumentTypeId = data.PayLoad.DocumentTypeId;
			tmp.Name = data.PayLoad.Name;
			tmp.Content = data.PayLoad.Content;
			tmp.DateSubmitted = data.PayLoad.DateSubmitted;

            _context.ArbitrationDocuments.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationDocumentPayload(tmp);

        }

        public async Task Delete(ArbitrationDocument data)
        {
            var tmp = await _context.ArbitrationDocuments.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Document not found");
            _context.ArbitrationDocuments.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<ArbitrationDocumentPayload> UpdateReviewStatus(GenericInput<UpdateArbitrationDocumentReviewStatusInput> data)
        {

            var tmp = await _context.ArbitrationDocuments.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Document not found");
            tmp.ReviewedBy = data.UserId;
            tmp.DateReviewed = data.PayLoad.DateReviewed;
            tmp.Remark = data.PayLoad.Remark;
            tmp.ReviewStatus = data.PayLoad.ReviewStatus;

            _context.ArbitrationDocuments.Update(tmp);
            await _context.SaveChangesAsync();
            return new ArbitrationDocumentPayload(tmp);
        }
    }
}