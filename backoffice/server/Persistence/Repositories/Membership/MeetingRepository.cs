using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class MeetingRepository
    {
        private readonly EccsaContext _context;
        public MeetingRepository(EccsaContext context)
        {
            _context = context;
        }

        public async Task<List<Meeting>> Meetings()
        {
            return await _context.Meetings
                .Include(i => i.Artifacts)
                .Include(i => i.Attendances)
                .Include(i => i.Nominees)
                .Include(i => i.Participants)
                .ToListAsync();
        }

        public async Task<Meeting> Meeting(Guid id)
        {
            var meeting = await _context.Meetings
                .Include(i => i.Artifacts)
                .Include(i => i.Attendances)
                .Include(i => i.Nominees)
                .Include(i => i.Participants)
                .FirstOrDefaultAsync(i => i.Id == id);
            meeting.Emails = await _context.EmailMessages.Where(e => e.ReferenceId == id
            && e.Owner.HasFlag(ECCSAModuleFlag.Membership)).ToListAsync();
            var associations = await _context.Associations.Where(a => a.ParentId == Constants.ECCSA).ToDictionaryAsync(a => a.Id);
            foreach (var p in meeting.Participants)
            {
                if (associations.Any(s => s.Key == p.AssociationId))
                {
                    p.Association = associations[p.AssociationId].Name;
                }
                else
                {
                    p.Association = "";
                }
            }
            return meeting;
        }
        public async Task<Participant> Participant(Guid id)
        {
            return await _context.MeetingParticipants
                .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<List<Artifact>> Artifacts(Guid? meetingId = null)
        {
            if (meetingId.HasValue)
                return await _context.Artifacts.Include(i => i.Meeting).Where(i => i.MeetingId == meetingId.Value).ToListAsync();
            return await _context.Artifacts.Include(i => i.Meeting).ToListAsync();
        }

        public async Task<Artifact> Artifact(Guid id)
        {
            return await _context.Artifacts.Include(i => i.Meeting).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task Save(Meeting meeting, Guid userId)
        {
            meeting.UpdatedBy = userId;
            meeting.UpdateStamp = DateTime.UtcNow;
            var tmp = await _context.Meetings.FirstOrDefaultAsync(i => i.Id == meeting.Id);
            if (tmp == null)
            {
               
                var associations = await _context.Associations.Where(a => a.ParentId == Constants.ECCSA).ToListAsync();
                meeting.CreatedBy = userId;
                meeting.CreateStamp = DateTime.UtcNow;
                _context.Meetings.Add(meeting);
                var i = 1;
                foreach (var ass in associations)
                {
                    var sid = $"0{i}";
                    
                    var p = new Participant()
                    {
                        Id = Guid.NewGuid(),
                        MeetingId = meeting.Id,
                        AssociationId = ass.Id,
                        Code = $"{meeting.Code}-{sid[^2..]}"
                    };
                    i++;
                    _context.MeetingParticipants.Add(p);
                }
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(meeting);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(Artifact artifact, Guid userId)
        {
            var meeting = await _context.Meetings.Include(m => m.Artifacts).FirstOrDefaultAsync(m => m.Id == artifact.MeetingId);
            if (meeting == null)
            {
                throw new KeyNotFoundException($"Meeting not found {artifact.MeetingId}");
            }
            var tmp = meeting.Artifacts.FirstOrDefault(i => i.Id == artifact.Id);
            if (tmp == null && artifact.Id != Guid.Empty)
            {
                _context.Artifacts.Add(artifact);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(artifact);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArtifact(Guid userId, Guid id)
        {
            var tmp = await _context.Artifacts.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) return;
            _context.Artifacts.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Save(Participant participant, Guid userId)
        {
            var meeting = await _context.Meetings.Include(m => m.Participants).FirstOrDefaultAsync(m => m.Id == participant.MeetingId);
            if (meeting == null)
            {
                throw new KeyNotFoundException($"Meeting not found {participant.MeetingId}");
            }
            var tmp = meeting.Participants.FirstOrDefault(i => i.Id == participant.Id);
            if (tmp == null)
            {
                throw new KeyNotFoundException($"Participant not found {participant.Id}");
            }
            _context.Entry(tmp).CurrentValues.SetValues(participant);

            await _context.SaveChangesAsync();
        }

        public async Task Save(Nominee data, Guid userId)
        {
            var meeting = await _context.Meetings.Include(m => m.Nominees).FirstOrDefaultAsync(m => m.Id == data.MeetingId);
            if (meeting == null)
            {
                throw new KeyNotFoundException($"Meeting not found {data.MeetingId}");
            }
            var tmp = meeting.Nominees.FirstOrDefault(i => i.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                _context.MeetingNominees.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(Guid meetingId, Guest data, Guid id)
        {
            var tmp = await _context.Meetings.FirstOrDefaultAsync(i => i.Id == meetingId);
            if (tmp == null)
                throw new KeyNotFoundException("Meeting Id Not found");
            var code = tmp.Code;
            if (tmp.DistinguishedGuests.Count > 0)
            {
                var maxCode = tmp.DistinguishedGuests.Max(x => x.Code);
                _ = int.TryParse(maxCode, out int i);
                maxCode = $"00{i + 1}"[^3..];
                data.Code = maxCode;
            }
            else
            {
                data.Code = "001";
            }
            tmp.DistinguishedGuests.Add(data);
            _context.Meetings.Update(tmp);
            await _context.SaveChangesAsync();
        }
        
        public async Task Delete(Guid meetingId, Guest data, Guid id)
        {
            var tmp = await _context.Meetings.FirstOrDefaultAsync(i => i.Id == meetingId);
            if (tmp == null)
                throw new KeyNotFoundException("Meeting Id Not found");
          var guest =  tmp.DistinguishedGuests.FirstOrDefault(g=> g.FullName.ToLowerInvariant() == data.FullName.ToLowerInvariant());
            if(guest != null)
            {
                tmp.DistinguishedGuests.Remove(guest);
            }
            _context.Meetings.Update(tmp);
            await _context.SaveChangesAsync();
        }
        
        public async Task Delete(Guid meetingId, MeetingAttendance data, Guid id)
        {
            var tmp = await _context.Meetings.Include(x=> x.Attendances).FirstOrDefaultAsync(i => i.Id == meetingId);
            if (tmp == null)
                throw new KeyNotFoundException("Meeting Id Not found");
            var attendance = tmp.Attendances.FirstOrDefault(g => g.Id == data.Id);
            if (attendance == null)
            {
                return;
            }
            _context.MeetingAttendances.Remove(attendance);
            await _context.SaveChangesAsync();
        }

       
        public async Task DeleteNominee(Guid id,  Guid meetingId, Guid userId)
        {
            var tmp = await _context.MeetingNominees.FirstOrDefaultAsync(i => i.Id == id && i.MeetingId == meetingId);
            if (tmp == null)
                return;
            
            _context.MeetingNominees.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid meetingId, Guid participantId, Guest data, Guid id)
        {
            var tmp = await _context.MeetingParticipants.FirstOrDefaultAsync(i => i.Id == participantId && i.MeetingId == meetingId);
            if (tmp == null)
                throw new KeyNotFoundException("Meeting Id Not found");
            var guest = tmp.OtherParticipants.FirstOrDefault(g => g.FullName.ToLowerInvariant() == data.FullName.ToLowerInvariant());
            if (guest != null)
            {
                tmp.OtherParticipants.Remove(guest);
            }
            _context.MeetingParticipants.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Save(Guid meetingId, Guid participantId, Guest data, Guid id)
        {
            var tmp = await _context.MeetingParticipants.FirstOrDefaultAsync(i => i.Id == participantId && i.MeetingId == meetingId);
            if (tmp == null)
                throw new KeyNotFoundException("Meeting Id Not found");
            var guest = tmp.OtherParticipants.FirstOrDefault(g => g.FullName.ToLowerInvariant() == data.FullName.ToLowerInvariant());
            if (guest != null)
            {
                tmp.OtherParticipants.Remove(guest);
            }
            tmp.OtherParticipants.Add(data);
            _context.MeetingParticipants.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Save(MeetingAttendance data, Guid userId)
        {
            var meeting = await _context.Meetings
                .Include(m => m.Attendances)
                .Include(m=> m.Participants)
                .FirstOrDefaultAsync(m => m.Id == data.MeetingId);
            if (meeting == null)
            {
                throw new KeyNotFoundException($"Meeting not found {data.MeetingId}");
            }
            var tmp = meeting.Attendances.FirstOrDefault(i => i.Id == data.Id);
            
            if (tmp == null)
            {
                tmp = meeting.Attendances.FirstOrDefault(i => i.AttendaceDate == data.AttendaceDate && i.No == data.No && i.ParticipantId == data.ParticipantId);
                if (tmp == null)
                {
                    if (data.Id == Guid.Empty)
                        data.Id = Guid.NewGuid();
                    _context.MeetingAttendances.Add(data);
                }else
                {
                    return;
                }
            }
            else
            {
                tmp.No = data.No;
                tmp.AttendaceDate = data.AttendaceDate;
                _context.MeetingAttendances.Update(tmp);
            }
            await _context.SaveChangesAsync();
        }
    }
}