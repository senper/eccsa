using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Academy;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Academy;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Academy
{
    public class LearningResourceRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public LearningResourceRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<LearningResource> LearningResources(Guid userId, Guid topicId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.LearningResources.Include(x => x.ResourceDeliveries)
.Include(x => x.Topic).Where(r => r.TopicId == topicId);
;
        }

        public async Task<LearningResource> LearningResourceById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.LearningResources.Include(x => x.ResourceDeliveries)
.Include(x => x.Topic)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<LearningResourcePayload> Create(GenericInput<CreateLearningResourceInput> input)
        {
            LearningResource data = new()
            {
                			Id = Guid.NewGuid()
			, TopicId = input.PayLoad.TopicId
			, Name = input.PayLoad.Name
			, Description = input.PayLoad.Description
			, ResourceType = input.PayLoad.ResourceType
			, ResourceContent = input.PayLoad.ResourceContent
			, Status = input.PayLoad.Status

            };
            await _context.LearningResources.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new LearningResourcePayload(data);
        }

        public async Task<LearningResourcePayload> Update(GenericInput<UpdateLearningResourceInput> data)
        {
            var tmp = await  _context.LearningResources.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Resource not found");
            						tmp.TopicId = data.PayLoad.TopicId;
			tmp.Name = data.PayLoad.Name;
			tmp.Description = data.PayLoad.Description;
			tmp.ResourceType = data.PayLoad.ResourceType;
			tmp.ResourceContent = data.PayLoad.ResourceContent;
			tmp.Status = data.PayLoad.Status;

            _context.LearningResources.Update(tmp);
            await _context.SaveChangesAsync();
            return new LearningResourcePayload(tmp);

        }

        public async Task Delete(LearningResource data)
        {
            var tmp = await _context.LearningResources.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Resource not found");
            _context.LearningResources.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}