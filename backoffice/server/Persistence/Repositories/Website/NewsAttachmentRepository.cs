



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Website;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Persistence.DTO.Website;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Website
{
    public class NewsAttachmentRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public NewsAttachmentRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<NewsAttachment> NewsAttachments(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.NewsAttachment.Include(x => x.AttachmentId)
                                           .Include(x => x.NewsAttachmentTypeId)
                                           .Include(x => x.Document)
                                           .Include(x => x.Extension)
;
        }

        public async Task<NewsAttachment> NewsAttachmentById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.NewsAttachment.Include(x => x.AttachmentId)
            .Include(x => x.NewsAttachmentTypeId)
            .Include(x => x.Document)
            .Include(x => x.Extension)
            .FirstOrDefaultAsync(i => i.AttachmentId == id);
        }
        public async Task<NewsAttachmentPayload> Create(GenericInput<CreateNewsAttachmentInput> input)
        {
            NewsAttachment data = new()
            {
                Id = Guid.NewGuid()
            ,
                AttachmentId = input.PayLoad.AttachmentId
            ,
                NewsAttachmentTypeId = input.PayLoad.NewsAttachmentTypeId
            ,
                Document = input.PayLoad.Document
            ,
                Extension = input.PayLoad.Extension

            };
            await _context.NewsAttachment.AddAsync(data);
            await _context.SaveChangesAsync();
            return new NewsAttachmentPayload(data);
        }

        public async Task<NewsAttachmentPayload> Update(GenericInput<UpdateNewsAttachmentInput> data)
        {
            var tmp = await _context.NewsAttachment.FirstOrDefaultAsync(r => r.AttachmentId == data.PayLoad.AttachmentId);
            if (tmp == null) throw new KeyNotFoundException("attachment not found");
            tmp.AttachmentId = data.PayLoad.AttachmentId;
            tmp.NewsAttachmentTypeId = data.PayLoad.NewsAttachmentTypeId;
            tmp.Document = data.PayLoad.Document;
            tmp.Extension = data.PayLoad.Extension;

            if (data.PayLoad.Extension.Length > 1)
            {
                tmp.Extension = data.PayLoad.Extension;
                
            }

            _context.NewsAttachment.Update(tmp);
            await _context.SaveChangesAsync();
            return new NewsAttachmentPayload(tmp);

        }

        

        public async Task Delete(NewsAttachment data)
        {
            var tmp = await _context.NewsAttachment.FirstOrDefaultAsync(r => r.AttachmentId == data.AttachmentId);
            if (tmp == null) throw new KeyNotFoundException("Attachment not found");
            _context.NewsAttachment.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}