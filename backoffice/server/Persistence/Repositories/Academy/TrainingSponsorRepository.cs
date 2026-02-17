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
    public class TrainingSponsorRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public TrainingSponsorRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<TrainingSponsor> TrainingSponsors(Guid userId, Guid trainingId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.TrainingSponsors.Include(x => x.Registrations)
.Include(x => x.Training)
.Include(x => x.Sponsor).Where(s => s.TrainingId == trainingId);
;
        }

        public async Task<TrainingSponsor> TrainingSponsorById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.TrainingSponsors.Include(x => x.Registrations)
.Include(x => x.Training)
.Include(x => x.Sponsor)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<TrainingSponsorPayload> Create(GenericInput<CreateTrainingSponsorInput> input)
        {
            TrainingSponsor data = new()
            {
                			Id = Guid.NewGuid()
			, TrainingId = input.PayLoad.TrainingId
			, SponsorId = input.PayLoad.SponsorId
            , ReferenceNo = input.PayLoad.ReferenceNo
            , Attachment = input.PayLoad.Attachment
			, Status = input.PayLoad.Status

            };
            await _context.TrainingSponsors.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new TrainingSponsorPayload(data);
        }

        public async Task<TrainingSponsorPayload> Update(GenericInput<UpdateTrainingSponsorInput> data)
        {
            var tmp = await  _context.TrainingSponsors.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Training Sponsor not found");
            						tmp.TrainingId = data.PayLoad.TrainingId;
			tmp.SponsorId = data.PayLoad.SponsorId;
			tmp.Status = data.PayLoad.Status;
            tmp.ReferenceNo = data.PayLoad.ReferenceNo;
            tmp.Attachment = data.PayLoad.Attachment;

            _context.TrainingSponsors.Update(tmp);
            await _context.SaveChangesAsync();
            return new TrainingSponsorPayload(tmp);

        }

        public async Task Delete(TrainingSponsor data)
        {
            var tmp = await _context.TrainingSponsors.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Training Sponsor not found");
            _context.TrainingSponsors.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}