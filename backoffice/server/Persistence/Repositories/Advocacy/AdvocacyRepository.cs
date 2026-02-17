using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Advocacy;
using System.Linq;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.Advocacy
{
    public class AdvocacyRepository : BaseRepository
    {
        private readonly EccsaContext _context;
        public AdvocacyRepository(EccsaContext context) : base(context)
        {
            _context = context;
        }



        public async Task<List<Dialog>> Dialogs()
        {
            return await _context.Dialogs.ToListAsync();

        }

        public async Task<List<Dialog>> DialogForPublic()
        {
            return await _context.Dialogs.Where(s => s.Status == Model.Common.StatusEnum.Active).ToListAsync();

        }
        public async Task<Dialog> DialogById(Guid id)
        {
            var request = await _context.Dialogs
                .Include(d => d.Documents)
                .Include(d => d.Meetings)
                .Include(d => d.Recommendations)
                .ThenInclude(d => d.Points)
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }

        public async Task<Dialog> DialogByIdForPublic(Guid id)
        {
            var request = await _context.Dialogs
                .Include(d => d.Recommendations)
               // .IncludeFilter(d => d.Documents.Where(d => d.IsPublic))
               .Include(d => d.Documents)
                    .FirstOrDefaultAsync(i => i.Id == id);
            return request;
        }

        public async Task<PPDMeeting> MeetingById(Guid id)
        {
            var request = await _context.PPDMeetings
                .Include(d => d.Documents)
                .Include(d => d.Dialog)
                .ThenInclude(d => d.Recommendations)
                .Include(d => d.Points)
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }
        public async Task Save(Dialog data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.Dialogs.Add(data);
            }
            else
            {
                var tmp = await _context.Dialogs.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Dialog Not found");
                }
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await GenerateKeyword(data);
            await _context.SaveChangesAsync();
        }

        public async Task Save(PPDMeeting data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            var tmp = await _context.PPDMeetings.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.PPDMeetings.Add(data);

            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Save(Recommendation data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            var tmp = await _context.PPDRecommendations.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                var lst = await _context.PPDRecommendations.Where(x => x.DialogId == data.DialogId).ToListAsync();
                if (lst == null || lst.Count == 0)
                    data.No = 1;
                else
                    data.No = lst.Max(x => x.No) + 1;
                _context.PPDRecommendations.Add(data);

            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            var dlg = await _context.Dialogs.FirstOrDefaultAsync(x => x.Id == data.DialogId);
            await GenerateKeyword(dlg, data);
            await _context.SaveChangesAsync();
        }
        public async Task Save(PPDAttachment data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            var tmp = await _context.PPDAttachments.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                _context.PPDAttachments.Add(data);

            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(MeetingPoint data, Guid userId)
        {
            data.ResponsibleBody = String.IsNullOrEmpty(data.ResponsibleBody) ? String.Empty : data.ResponsibleBody;
            data.FollowupPerson = String.IsNullOrEmpty(data.FollowupPerson) ? String.Empty : data.FollowupPerson;
            data.ImplementationText = String.IsNullOrEmpty(data.ImplementationText) ? String.Empty : data.ImplementationText;
            data.Remark = String.IsNullOrEmpty(data.Remark) ? String.Empty : data.Remark;
            var tmp = await _context.PPDMeetingPoints.FirstOrDefaultAsync(x => x.MeetingId == data.MeetingId && x.RecommendationId == data.RecommendationId);
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                _context.PPDMeetingPoints.Add(data);

            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            var tmpRec = await _context.PPDRecommendations.Include(d => d.Dialog).FirstOrDefaultAsync(x => x.Id == data.RecommendationId);

            tmpRec.Status = data.Declaration;
            tmpRec.ResponsibleBody = data.ResponsibleBody;
            tmpRec.FollowupPerson = data.FollowupPerson;
            tmpRec.ImplementationText = data.ImplementationText;
            tmpRec.Remark = data.Remark;
            tmpRec.UpdatedBy = userId;
            tmpRec.UpdateStamp = DateTime.UtcNow;
            _context.PPDRecommendations.Update(tmpRec);
            var dlg = await _context.Dialogs.FirstOrDefaultAsync(x => x.Id == tmpRec.DialogId);
            await GenerateKeyword(dlg, tmpRec);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteDialog(Guid id)
        {
            var tmp = await _context.Dialogs.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Dialog not found");
            _context.Dialogs.Remove(tmp);
            await RemoveKeyword(id, id);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMeeting(Guid id)
        {
            var tmp = await _context.PPDMeetings.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Dialog meeting not found");
            _context.PPDMeetings.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecommendation(Guid id)
        {
            var tmp = await _context.PPDRecommendations.Include(x => x.Dialog).Include(x => x.Points).FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Dialog Recommendation not found");
            if (tmp.Points.Count > 0) throw new KeyNotFoundException("Dialog Recommendation used");
            _context.PPDRecommendations.Remove(tmp);
            await RemoveKeyword(tmp.DialogId, id);
            await _context.SaveChangesAsync();
        }

        public async Task<PPDAttachment> GetAttachment(Guid id)
        {
            var tmp = await _context.PPDAttachments.FirstOrDefaultAsync(r => r.Id == id);
            return tmp;
        }
        public async Task DeleteAttachment(Guid id)
        {
            var tmp = await _context.PPDAttachments.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Dialog attachment not found");
            _context.PPDAttachments.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMeetingPoint(Guid id, Guid userId)
        {
            var tmp = await _context.PPDMeetingPoints.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Dialog Meeting Point not found");
            _context.PPDMeetingPoints.Remove(tmp);

            await _context.SaveChangesAsync();
            var rec = await _context.PPDRecommendations.Include(x => x.Dialog).Include(x => x.Points).FirstOrDefaultAsync(r => r.Id == tmp.RecommendationId);
            if (rec.Points == null || rec.Points.Count == 0)
            {
                rec.Status = Model.Common.PPDRecommendationEnum.Draft;
                rec.ImplementationText = String.Empty;
                rec.ResponsibleBody = String.Empty;
                rec.FollowupPerson = String.Empty;
                rec.UpdateStamp = DateTime.UtcNow;
                rec.UpdatedBy = userId;
            }
            _context.PPDRecommendations.Update(rec);
            await GenerateKeyword(rec.Dialog, rec);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Research>> Researches()
        {
            return await _context.Researches.ToListAsync();

        }
        public async Task<Research> ResearchById(Guid id)
        {
            var request = await _context.Researches
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }


        public async Task Save(Research data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;

                _context.Researches.Add(data);
            }
            else
            {
                var tmp = await _context.Researches.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Dialog Not found");
                }
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResearch(Guid id)
        {
            var tmp = await _context.Researches.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Research not found");
            _context.Researches.Remove(tmp);
            await _context.SaveChangesAsync();
        }



        public async Task<List<Publication>> Publications()
        {
            return await _context.Publications.ToListAsync();

        }
        public async Task<Publication> PublicationById(Guid id)
        {
            var request = await _context.Publications
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }


        public async Task Save(Publication data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;

            if (string.IsNullOrEmpty(data.Url))
            {
                data.DigitalCost = 0;
                data.Url = String.Empty;

            }
            if (string.IsNullOrEmpty(data.PrintUri))
            {
                data.PrintCost = 0;
                data.PrintUri = String.Empty;

            }

            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                if (data.IsPublic)
                {
                    var res = Model.Communication.Resource.FromAdvocacy(data);
                    _context.Resources.Add(res);
                }
                _context.Publications.Add(data);
            }
            else
            {
                var tmp = await _context.Publications.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Publication Not found");
                }
                if (data.IsPublic != tmp.IsPublic)
                {
                    var rData = Model.Communication.Resource.FromAdvocacy(data);
                    var res = await _context.Resources.FirstOrDefaultAsync(r => r.Id == tmp.Id);
                    if (data.IsPublic)
                    {
                        if (res == null)
                        {
                            _context.Resources.Add(rData);
                        }
                        else
                        {
                            _context.Entry(res).CurrentValues.SetValues(rData);
                        }
                    }
                    else
                    {
                        if (res != null)
                        {
                            _context.Resources.Remove(res);
                        }
                    }
                }
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeletePublication(Guid id)
        {
            var tmp = await _context.Publications.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Publication not found");
            var tmpRes = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id);
            if (tmpRes != null)
                _context.Resources.Remove(tmpRes);
            _context.Publications.Remove(tmp);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Project>> Projects()
        {
            return await _context.Projects.ToListAsync();

        }
        public async Task<Project> ProjectById(Guid id)
        {
            var request = await _context.Projects.Include(p => p.Followups)
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }


        public async Task Save(Project data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;

                _context.Projects.Add(data);
            }
            else
            {
                var tmp = await _context.Projects.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Project Not found");
                }
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProject(Guid id)
        {
            var tmp = await _context.Projects.Include(p => p.Followups).FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Project not found");
            if (tmp.Followups.Count > 0) throw new InvalidOperationException("Followup Not empty");
            _context.Projects.Remove(tmp);
            await _context.SaveChangesAsync();
        }


        public async Task<List<ProjectFollowup>> ProjectFollowups()
        {
            return await _context.ProjectFollowups.ToListAsync();

        }
        public async Task<ProjectFollowup> ProjectFollowupById(Guid id)
        {
            var request = await _context.ProjectFollowups.Include(p => p.Project)
                    .FirstOrDefaultAsync(i => i.Id == id);

            return request;
        }


        public async Task Save(ProjectFollowup data, Guid userId)
        {
            data.UpdatedBy = userId;
            data.UpdateStamp = DateTime.UtcNow;
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;

                _context.ProjectFollowups.Add(data);
            }
            else
            {
                var tmp = await _context.ProjectFollowups.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Project Followup Not found");
                }
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectFollowup(Guid id)
        {
            var tmp = await _context.ProjectFollowups.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Project Followup not found");
            _context.ProjectFollowups.Remove(tmp);
            await _context.SaveChangesAsync();
        }

    }
}