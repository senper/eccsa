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
    public class TrainingRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public TrainingRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Training> Trainings(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Trainings.Include(x => x.Attendances)
.Include(x => x.Assessments)
.Include(x => x.TrainingSponsors)
.Include(x => x.Registrations)
.Include(x => x.Topic)
;
        }

        public IQueryable<Training> Trainings(Guid userId, TrainingStatus status)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Trainings.Include(x => x.Attendances)
.Include(x => x.Assessments)
.Include(x => x.TrainingSponsors)
.Include(x => x.Registrations)
.Include(x => x.Topic).Where(s => s.Status == status)
;
        }
        public IQueryable<Training> TrainingsSummary(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Trainings
.Include(x => x.Topic).ThenInclude(t => t.Category)
;
        }
        public async Task<List<LookupValue>> GetPrivateSurveySettings()
        {
            return await _context.SurveySettings
                .Where(s => !s.IsPublic && s.Status == StatusEnum.Active && 
                s.ResponsibleUnit.HasFlag(ECCSAModuleFlag.Academy)).Select(x => new LookupValue()
                {
                    Id =x.Id,
                    Name = x.Title
                }).ToListAsync();
        }
        public async Task<Training> TrainingByIdForCompare(Guid id, Guid userId)
        {
            return await _context.Trainings.Include(x => x.Registrations)
                    .FirstOrDefaultAsync(i => i.Id == id);
        }
            public async Task<Training> TrainingById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Trainings.Include(x => x.Attendances)
.Include(x => x.Assessments)
.Include("TrainingSponsors.Sponsor")
.Include("Registrations.ResourceDeliveries")
.Include(x => x.Topic)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Training> TrainingByServiceTypeId(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Trainings.Include(x => x.Attendances)
.Include(x => x.Assessments)
.Include("TrainingSponsors.Sponsor")
.Include("Registrations.ResourceDeliveries")
.Include(x => x.Topic)
.FirstOrDefaultAsync(i => i.ServiceTypeId == id);
        }
        public async Task<TrainingPayload> Create(GenericInput<CreateTrainingInput> input, Guid serviceTypeId)
        {
            if (input.PayLoad.StartDate > input.PayLoad.EndDate)
            {
                throw new Exception("Start Date must be before end date.");
            }
            if (!input.PayLoad.HasEvening && !input.PayLoad.HasMorning && !input.PayLoad.HasAfternoon)
            {
                throw new Exception("Schedule must have at-least morning, afternoon or evening.");
            }
            Training data = new()
            {
                Id = Guid.NewGuid()
            ,
                TopicId = input.PayLoad.TopicId
            ,
                StartDate = input.PayLoad.StartDate
            ,
                EndDate = input.PayLoad.EndDate,
                Schedule = input.PayLoad.Schedule
            ,
                Instructor = input.PayLoad.Instructor
            ,
                InstructionMedia = input.PayLoad.InstructionMedia
            ,
                Status = TrainingStatus.New // input.PayLoad.Status
                ,
                MemberFee = input.PayLoad.MemberFee
                ,
                NonMemberFee = input.PayLoad.NonMemberFee
            ,
                CreatedBy = input.UserId
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = input.UserId
            ,
                UpdateStamp = DateTime.UtcNow,
                RateBasis = input.PayLoad.RateBasis,
                ServiceTypeId = serviceTypeId,
                HasMorning = input.PayLoad.HasMorning,
                HasAfternoon = input.PayLoad.HasAfternoon,
                HasEvening = input.PayLoad.HasEvening,
            };

            switch (data.RateBasis)
            {
                case RateBasisEnum.Flat:
                    data.MemberFee = Math.Max(data.MemberFee, data.NonMemberFee);
                    data.NonMemberFee = Math.Max(data.MemberFee, data.NonMemberFee);
                    break;
                case RateBasisEnum.Adhoc:
                    data.MemberFee = 0;
                    data.NonMemberFee = 0;
                    break;
            }
            await _context.Trainings.AddAsync(data);
            var val = await _context.SaveChangesAsync();

            return new TrainingPayload(data);
        }

        public async Task<TrainingPayload> Update(GenericInput<UpdateTrainingInput> data, Guid serviceTypeId)
        {
            if (data.PayLoad.StartDate > data.PayLoad.EndDate)
            {
                throw new Exception("Start Date must be before end date.");
            }
            if(!data.PayLoad.HasEvening && !data.PayLoad.HasMorning && !data.PayLoad.HasAfternoon)
            {
                throw new Exception("Schedule must have at-least morning, afternoon or evening.");
            }
            var tmp = await _context.Trainings.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Training not found");
            tmp.TopicId = data.PayLoad.TopicId;
            tmp.StartDate = data.PayLoad.StartDate;
            tmp.EndDate = data.PayLoad.EndDate;
            tmp.Instructor = data.PayLoad.Instructor;
            tmp.InstructionMedia = data.PayLoad.InstructionMedia;
            //tmp.Status = data.PayLoad.Status;
            tmp.MemberFee = data.PayLoad.MemberFee;
            tmp.NonMemberFee = data.PayLoad.NonMemberFee;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;
            tmp.RateBasis = data.PayLoad.RateBasis;
            tmp.Schedule = data.PayLoad.Schedule;
            tmp.ServiceTypeId = serviceTypeId;
            tmp.HasMorning = data.PayLoad.HasMorning;
            tmp.HasAfternoon = data.PayLoad.HasAfternoon;
            tmp.HasEvening = data.PayLoad.HasEvening;
            switch (tmp.RateBasis)
            {
                case RateBasisEnum.Flat:
                    tmp.MemberFee = Math.Max(tmp.MemberFee, tmp.NonMemberFee);
                    tmp.NonMemberFee = Math.Max(tmp.MemberFee, tmp.NonMemberFee);
                    break;
                case RateBasisEnum.Adhoc:
                    tmp.MemberFee = 0;
                    tmp.NonMemberFee = 0;
                    break;
            }
            _context.Trainings.Update(tmp);
            if (tmp.ServiceTypeId != null)
            {
                var serviceType = _context.ServiceTypes.FirstOrDefault(s => s.Id == tmp.ServiceTypeId);
                if (serviceType != null)
                {
                    if (tmp.Status == TrainingStatus.OpenForRegistration)
                    {
                        serviceType.Status = StatusEnum.Active;
                        serviceType.StartDate = DateTime.UtcNow;
                    }
                    else
                        serviceType.Status = StatusEnum.Inactive;
                    _context.ServiceTypes.Update(serviceType);
                }
            }
            await _context.SaveChangesAsync();
            return new TrainingPayload(tmp);

        }

        

        public async Task<TrainingPayload> UpdateStatus(GenericInput<UpdateTrainingStatusInput> data)
        {
            var tmp = await _context.Trainings.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Training not found");
            tmp.Status = data.PayLoad.Status;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.Trainings.Update(tmp);
            if (tmp.ServiceTypeId != null)
            {
                var serviceType = _context.ServiceTypes.FirstOrDefault(s => s.Id == tmp.ServiceTypeId);
                if (serviceType != null)
                {
                    if (tmp.Status == TrainingStatus.OpenForRegistration)
                    {
                        serviceType.Status = StatusEnum.Active;
                        serviceType.StartDate = DateTime.UtcNow;
                    } 
                    else if(tmp.Status == TrainingStatus.RegistrationEnded)
                    {
                        serviceType.EndDate = DateTime.UtcNow;
                        serviceType.Status = StatusEnum.Inactive;
                    }
                    else
                        serviceType.Status = StatusEnum.Inactive;

                    _context.ServiceTypes.Update(serviceType);
                }
            }
            await _context.SaveChangesAsync();
            return new TrainingPayload(tmp);

        }
        public async Task Delete(Training data)
        {
            var tmp = await _context.Trainings.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Training not found");
            _context.Trainings.Remove(tmp);
            if (tmp.ServiceTypeId != null)
            {
                var serviceType = _context.ServiceTypes.FirstOrDefault(s => s.Id == tmp.ServiceTypeId);
                if (serviceType != null)
                {
                    _context.ServiceTypes.Remove(serviceType);
                }
            }
            if(tmp.SurveyId != null)
            {
                var survey = await _context.Surveys.FirstOrDefaultAsync(s => s.Id == tmp.SurveyId);
                if(survey != null)
                {
                    _context.Surveys.Remove(survey);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(Training training, Guid userId)
        {
            _context.Trainings.Update(training);
            training.UpdatedBy = userId;
            training.UpdateStamp = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}