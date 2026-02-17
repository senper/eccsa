using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Model.Common;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class AssociationRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public AssociationRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<List<Association>> List(Guid userId)
        {
            var user = await GetUser(userId);
            if (user == null)
                return new List<Association>();
            if (user.AssociationId == Constants.ECCSA)
                return await _context.Associations.Include(x => x.Location).ToListAsync();
            return await _context.Associations.Include(x => x.Location)
                 .Where(a => a.Id == user.AssociationId || a.ParentId == user.AssociationId).ToListAsync();

        }

        public async Task<int> ListCount(Guid userId)
        {
            var user = await GetUser(userId);
            if (user == null)
                return 0;
            if (user.AssociationId == Constants.ECCSA)
                return await _context.Associations.Where(a => a.Id == user.AssociationId || a.ParentId == user.AssociationId).CountAsync(s => s.Stakeholder.Status == StatusEnum.Active);
            return await _context.Associations
                 .Where(a => a.Id == user.AssociationId || a.ParentId == user.AssociationId).CountAsync(s => s.Stakeholder.Status == StatusEnum.Active);

        }
        public async Task<List<Association>> ChildList(Guid associationId, Guid userId)
        {
            var tmp = await List(userId);
            return tmp.Where(a => a.Id == associationId || a.ParentId == associationId).ToList();
        }

        public async Task<List<Association>> GetAssociationsForPublic(Guid associationId)
        {
            return await _context.Associations.Include(x => x.Location).Include(x => x.Stakeholder)
                 .Where(a => a.Id != associationId && a.ParentId == associationId &&
                 a.Stakeholder != null && a.Stakeholder.Status == StatusEnum.Active).ToListAsync();
        }
        public async Task<List<Association>> GetAssociationsForPublic(AssociationTypeEnum type)
        {
            return await _context.Associations
                 .Where(a => a.AssociationType == type).ToListAsync();
        }
        public async Task<List<Association>> GetAssociationsForApply(AssociationTypeEnum type, Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("invalid user");
            }
            return await _context.Associations
                 .Where(a => a.AssociationType == type && a.Settings.Any() &&
                 //
                !_context.Applicants.
                Any(p => p.AssociatedUserId == userId && p.AssociationId == a.Id &&
                p.ApplicationStatus != WorkflowStateEnum.Rejected)
                //
                && !_context.MembershipEnrollments
                               .Any(m => m.MemberId == user.AssociationId && m.ParentAssociationId == a.Id)
                ).ToListAsync();
        }
        private async Task<User> GetUser(Guid id)
        {
            return await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public IQueryable<Association> Associations(Guid userId)
        {


            return _context.Associations.Include(x => x.Location)
;
        }

        public async Task<Association> AssociationById(Guid id, Guid userId)
        {
            var user = await GetUser(userId);
            if (id == Guid.Empty || user == null)
            {
                return null;
            }
            var entity = await _context.Associations.FirstOrDefaultAsync(a => a.Id == id);
            if (user.AssociationId != Constants.ECCSA && (entity.Id != user.AssociationId && entity.ParentId != user.AssociationId))
                return null;
            entity = await _context.Associations.Include(x => x.Location)
                                   .Include(x => x.Settings)
                                   .Include(x => x.MembershipEnrollments)
                                   .Include(x => x.MembershipRequirements)
                                   .ThenInclude(x => x.ApplicantDocuments)
                                   .Include(x => x.AssociationProfiles)
                                   .ThenInclude(x => x.ProfileAttribure)
                                   .FirstOrDefaultAsync(i => i.Id == id);
            entity.Stakeholder = await _context.Stakeholders
                                .Include(x => x.Addresses)
                                .Include(x => x.Contacts)
                                .FirstOrDefaultAsync(s => s.Id == id);
            foreach (var r in entity.MembershipRequirements)
            {
                r.HasApplicants = r.ApplicantDocuments.Any();
            }
            entity.Stakeholder.Representatives = await _context.Representatives.Include(x => x.RepresentativeContacts).Where(x => x.MemberId == id).ToListAsync();
            return entity;
        }
        public async Task<List<LookupType>> LookupTypes(ECCSAModuleFlag subSystem)
        {
            return await _context.LookupTypes
                .Where(l => l.Owner.HasFlag(subSystem))
                .OrderBy(l => l.Name)
                .ToListAsync();
        }

        public async Task<List<LookupValue>> LookupTypeById(Guid id)
        {
            return await _context.LookupValues
                .Where(l => l.LookupTypeId == id)
                .OrderBy(l => l.Name)
                .ToListAsync();
        }
        public async Task<Association> AssociationByIdForPublic(Guid id, Guid userId)
        {
            var entity = await _context.Associations.Include(x => x.Location)
                                    .Include(x => x.Settings)
                                    .Include(x => x.MembershipEnrollments)
                                    .Include(x => x.MembershipRequirements)
                                    .Include(x => x.AssociationProfiles)
                                    .ThenInclude(x => x.ProfileAttribure)
                                    .FirstOrDefaultAsync(i => i.Id == id);
            entity.Stakeholder = await _context.Stakeholders
                                .Include(x => x.Addresses)
                                .Include(x => x.Contacts)
                                .FirstOrDefaultAsync(s => s.Id == id);
            entity.Stakeholder.Representatives = await _context.Representatives.Include(x => x.RepresentativeContacts).Where(x => x.MemberId == id).ToListAsync();
            return entity;
        }

        public async Task<Association> AssociationByIdForPublicApply(Guid id, Guid userId)
        {
            //var entity = await _context.Associations
            //                       .Include(x => x.Settings)
            //     .IncludeFilter(x => x.MembershipRequirements.Where(s => s.Status == StatusEnum.Active))
            //    .FirstOrDefaultAsync(a => a.Id == id);
            var entity = await _context.Associations.AsNoTracking()
                                   .Include(x => x.Settings)
                 .Include(x => x.MembershipRequirements)
                .FirstOrDefaultAsync(a => a.Id == id);
            entity.MembershipRequirements = entity.MembershipRequirements.Where(s => s.Status == StatusEnum.Active).ToList();

            var lookupRequirements = entity.MembershipRequirements.
                            Where(s => s.InputMethod == ServiceInputTypeEnum.SimpleLookup);
            if (lookupRequirements.Any())
            {
                //entity.RequirementLookups = (await _context.LookupTypes.
                //    IncludeFilter(l => l.LookupValues.Where(v => v.Status == StatusEnum.Active))
                //    .ToListAsync())
                //    .Where(l => lookupRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();

                entity.RequirementLookups = (await _context.LookupTypes.
                    Include(l => l.LookupValues)
                    .ToListAsync())
                    .Where(l => lookupRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
                foreach (var item in entity.RequirementLookups)
                {
                    item.LookupValues = item.LookupValues.Where(v => v.Status == StatusEnum.Active).ToList();
                }
            }
            return entity;
        }
        public async Task<AssociationPayload> Create(GenericInput<CreateAssociationInput> input)
        {
            var user = await GetUser(input.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("Unknown user");
            }
            if (user.AssociationId != Constants.ECCSA && user.AssociationId != input.PayLoad.ParentId)
            {
                throw new InvalidOperationException("Cannot create for different association");
            }
            Association data = new()
            {
                Id = Guid.NewGuid()
            ,
                NameEng = input.PayLoad.NameEng
            ,
                NameLocal = input.PayLoad.NameLocal
            ,
                ShortName = input.PayLoad.ShortName
            ,
                ShortName_Local = input.PayLoad.ShortName_Local
            ,
                MemberSince = input.PayLoad.MemberSince
            ,
                AssociationType = input.PayLoad.AssociationType
            ,
                AssociationLevel = input.PayLoad.AssociationLevel
            ,
                ParentId = input.PayLoad.ParentId
            ,
                LocationId = input.PayLoad.LocationId
            ,
                CreatedBy = input.UserId
            ,
                ProposedRegistrationFee = input.PayLoad.RegistrationFee
            ,
                ProposedRenewalFee = input.PayLoad.RenewalFee
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = input.UserId
            ,
                UpdateStamp = DateTime.UtcNow
                ,
                ProposedBy = input.UserId
                ,
                DateProposed = DateTime.UtcNow
                ,
                Profile = string.Empty
                ,
                Profile_Local = string.Empty
                ,
                Logo = input.PayLoad.Logo
                ,
                Extension = input.PayLoad.Extension

            };
            _context.Stakeholders.Add(new Stakeholder()
            {
                Id = data.Id,
                StakeholderType = StakeholderTypeEnum.Association,
                Status = input.PayLoad.Status
            });
            await _context.Associations.AddAsync(data);

            var lookupTypes = await _context.LookupTypes.Include(l => l.LookupValues).FirstOrDefaultAsync(l => l.Id == LookupType.Association);
            var lv = new LookupValue()
            {
                Id = data.Id,
                Status = StatusEnum.Active,
                Name = data.Name,
                LookupTypeId = lookupTypes.Id,
                Value = data.Id.ToString(),
            };
            lookupTypes.LookupValues.Add(lv);
            _context.LookupValues.Add(lv);
            var val = await _context.SaveChangesAsync();
            return new AssociationPayload(data);
        }

        public async Task<AssociationPayload> Update(GenericInput<UpdateAssociationInput> data)
        {
            var tmp = await _context.Associations.Include(a=> a.Stakeholder).FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Association not found");

            var user = await GetUser(data.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("Unknown user");
            }
            if (user.AssociationId != Constants.ECCSA && user.AssociationId != data.PayLoad.ParentId && user.AssociationId != data.PayLoad.Id)
            {
                throw new InvalidOperationException("Cannot update for different association");
            }
            tmp.Stakeholder.Status = data.PayLoad.Status;
            tmp.NameEng = data.PayLoad.NameEng;
            tmp.NameLocal = data.PayLoad.NameLocal;
            tmp.ShortName = data.PayLoad.ShortName;
            tmp.ShortName_Local = data.PayLoad.ShortName_Local;
            tmp.MemberSince = data.PayLoad.MemberSince;
            tmp.AssociationType = data.PayLoad.AssociationType;
            tmp.AssociationLevel = data.PayLoad.AssociationLevel;
            tmp.ParentId = data.PayLoad.ParentId;
            tmp.LocationId = data.PayLoad.LocationId;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;
            if (data.PayLoad.Logo != null && data.PayLoad.Logo.Length > 0)
            {
                tmp.Logo = data.PayLoad.Logo;
                tmp.Extension = data.PayLoad.Extension;
            }
            if (tmp.RegistrationFee != data.PayLoad.RegistrationFee ||
                tmp.RenewalFee != data.PayLoad.RenewalFee)
            {
                tmp.ProposedRegistrationFee = data.PayLoad.RegistrationFee;
                tmp.ProposedRenewalFee = data.PayLoad.RenewalFee;
                tmp.ProposedBy = data.UserId;
                tmp.DateProposed = DateTime.UtcNow;
            }
            _context.Associations.Update(tmp);
            _context.Stakeholders.Update(tmp.Stakeholder);
            var lookupTypes = await _context.LookupTypes.Include(l => l.LookupValues).FirstOrDefaultAsync(l => l.Id == LookupType.Association);
            var lv = lookupTypes.LookupValues.FirstOrDefault(v => v.Value == tmp.Id.ToString());
            if (lv == null)
            {
                lv = new LookupValue()
                {
                    Id = tmp.Id,
                    Status = tmp.Stakeholder.Status,
                    Name = tmp.Name,
                    LookupTypeId = lookupTypes.Id,
                    Value = tmp.Id.ToString(),
                };
                lookupTypes.LookupValues.Add(lv);
                _context.LookupValues.Add(lv);
            }
            else
            {
                lv.Name = tmp.Name;
                lv.Status = tmp.Stakeholder.Status;
                _context.LookupValues.Update(lv);
            }

            await _context.SaveChangesAsync();
            return new AssociationPayload(tmp);

        }


        public async Task<List<Association>> AssociationFeeNeedsApproval(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Associations
                .Where(s => (s.RegistrationFee != s.ProposedRegistrationFee
                || s.RenewalFee != s.ProposedRenewalFee) && s.DateProposed.HasValue
                && s.ProposedBy.HasValue).ToListAsync()
;
        }

        public async Task ApproveAssociationFee(Guid id, Guid userId)
        {
            var tmp = await _context.Associations.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Association not found");
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            if (tmp.ProposedRegistrationFee.HasValue)
                tmp.RegistrationFee = tmp.ProposedRegistrationFee.Value;
            if (tmp.ProposedRenewalFee.HasValue)
                tmp.RenewalFee = tmp.ProposedRenewalFee.Value;
            _context.Associations.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<AssociationPayload> UpdateProfile(Guid id, String profile, Guid userId)
        {
            var tmp = await _context.Associations.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Association not found");
            tmp.Profile = String.IsNullOrEmpty(profile) ? " " : profile;
            tmp.Profile_Local = String.IsNullOrEmpty(profile) ? " " : profile;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Associations.Update(tmp);
            await _context.SaveChangesAsync();
            return new AssociationPayload(tmp);
        }

        public async Task<AssociationPayload> UpdateProfile_Local(Guid id, String profile, Guid userId)
        {
            var tmp = await _context.Associations.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Association not found");
            tmp.Profile_Local = String.IsNullOrEmpty(profile) ? " " : profile;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Associations.Update(tmp);
            await _context.SaveChangesAsync();
            return new AssociationPayload(tmp);
        }

        public async Task Delete(Association data)
        {
            var tmp = await _context.Associations.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Association not found");
            _context.Associations.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Save(MembershipRequirement data, Guid userId)
        {
            var tmp = await _context.MembershipRequirements.FirstOrDefaultAsync(x => x.Id == data.Id);
            var ass = await _context.Associations.FirstOrDefaultAsync(r => r.Id == data.AssociationId);
            if (ass == null) throw new KeyNotFoundException("Association not found");
            if (tmp == null)
            {
                if (data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                _context.MembershipRequirements.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            ass.UpdatedBy = userId;
            ass.UpdateStamp = DateTime.UtcNow;
            _context.Associations.Update(ass);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(MembershipRequirement data, Guid userId)
        {
            var tmp = await _context.MembershipRequirements.Include(x => x.ApplicantDocuments).FirstOrDefaultAsync(x => x.Id == data.Id);
            var ass = await _context.Associations.FirstOrDefaultAsync(r => r.Id == data.AssociationId);
            if (ass == null) throw new KeyNotFoundException("Association not found");
            if (tmp == null)
                return;
            if (tmp.ApplicantDocuments.Any())
            {
                tmp.Status = StatusEnum.Inactive;
                _context.MembershipRequirements.Update(tmp);
            }
            else
            {
                _context.MembershipRequirements.Remove(tmp);
            }
            ass.UpdatedBy = userId;
            ass.UpdateStamp = DateTime.UtcNow;
            _context.Associations.Update(ass);
            await _context.SaveChangesAsync();
        }
    }
}