using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class MembershipEnrollmentRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public MembershipEnrollmentRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }
        public async Task<List<MembershipEnrollment>> List(Guid associationId, Guid userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return new List<MembershipEnrollment>();
            if (user.AssociationId == Constants.ECCSA)
            {
                var lst = await _context.MembershipEnrollments
                    .Include(x => x.Stakeholder).ThenInclude(x => x.Individual)
                    .Include(x => x.Association)
                    .Where(a => a.ParentAssociationId == associationId).ToListAsync();
                return lst;
            }
            var associations = await _context.Associations.Where(a => a.Id == user.AssociationId || a.ParentId == user.AssociationId).ToListAsync();
            if(associations.Count(a => a.Id == associationId || a.ParentId == associationId) == 0)
            {
                throw new InvalidOperationException("Wrong association");
            }
            return await _context.MembershipEnrollments
                    .Include(x => x.Stakeholder).ThenInclude(x => x.Individual)
                    .Include(x => x.Association)
                    .Where(a => a.ParentAssociationId == associationId).ToListAsync();
        }
        public async Task<List<MembershipEnrollment>> ListForExport(Guid associationId, Guid userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return new List<MembershipEnrollment>();
            if (user.AssociationId == Constants.ECCSA)
            {
                var lst = await _context.MembershipEnrollments
                    .Include(x => x.Stakeholder).ThenInclude(x => x.Individual).ThenInclude(x => x.Employees)
                                .Include("Stakeholder.Contacts")
                                .Include("Stakeholder.Representatives")
                                .Include("Stakeholder.Addresses")
                    .Include(x => x.Association)
                    .Where(a => a.ParentAssociationId == associationId).ToListAsync();
                return lst;
            }
            var associations = await _context.Associations.Where(a => a.Id == user.AssociationId || a.ParentId == user.AssociationId).ToListAsync();
            if(associations.Count(a => a.Id == associationId || a.ParentId == associationId) == 0)
            {
                throw new InvalidOperationException("Wrong association");
            }
            return await _context.MembershipEnrollments
                    .Include(x => x.Stakeholder).ThenInclude(x => x.Individual).ThenInclude(x => x.Employees)
                                .Include("Stakeholder.Contacts")
                                .Include("Stakeholder.Representatives")
                                .Include("Stakeholder.Addresses")
                    .Include(x => x.Association)
                    .Where(a => a.ParentAssociationId == associationId).ToListAsync();
        }
        public IQueryable<MembershipEnrollment> MembershipEnrollments(Guid associationId, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.MembershipEnrollments.Include(x => x.RenewalHistory)
                .Include(x => x.Stakeholder).ThenInclude(ind => ind.Individual)
                .Include(x => x.Association)
                .Where(a => a.ParentAssociationId == associationId);
            ;
        }


        public List<ChartTreeAreaModel> AggregateMembershipEnrollments(Guid associationId, Guid userId)
        {
            var enrollments = _context.MembershipEnrollments
                .Where(s => s.MembershipStatus == StatusEnum.Active).ToList();

            var allAssociations = _context.Associations.ToList();
            var associations = _context.Associations.Where(a => a.Id != Constants.ECCSA);
            var ethiopia = allAssociations.FirstOrDefault(a => a.Id == Constants.ECCSA);

            var list = new List<ChartTreeAreaModel>();
            var regions = associations.Where(a => a.ParentId == Constants.ECCSA).ToList();
            foreach(var reg in regions)
            {
                var value = 0;
                var l2Associations = associations.Where(a => a.ParentId == reg.Id).ToList();

                foreach(var l2 in l2Associations)
                {

                    var l3Associations = associations.Where(a => a.ParentId == l2.Id);
                    //foreach (var l3 in l3Associations)
                    //{
                    //    list.Add(new ChartTreeAreaModel
                    //    {
                    //        Key = l3.NameShort + (l3.AssociationType == AssociationTypeEnum.Administrative ? "(A)" : "(S)"),
                    //        Region = reg.NameShort,
                    //        Subregion = l2.NameShort,
                    //        Value = associations.Count(a => a.ParentId == l3.Id) + enrollments.Where(e => e.ParentAssociationId == l3.Id).Count() 
                    //    });
                    //}
                    value = associations.Count(a => a.ParentId == l2.Id) + enrollments.Where(e => e.ParentAssociationId == l2.Id).Count();
                    value = value == 0 ? 1 : value;
                    list.Add(new ChartTreeAreaModel
                    {
                        Key = l2.NameShort + (l2.AssociationType == AssociationTypeEnum.Administrative ? "(A)" : "(S)"),
                        Region = reg.NameShort,
                        Subregion = l2.NameShort,
                        Value = value 
                    });
                }
                value = associations.Count(a => a.ParentId == reg.Id) + enrollments.Where(e => e.ParentAssociationId == reg.Id).Count();
                value = value == 0 ? 1 : value;
                list.Add(new ChartTreeAreaModel
                {
                    Key = reg.NameShort + (reg.AssociationType == AssociationTypeEnum.Administrative? "(A)" : "(S)"),
                    Region = reg.NameShort,
                    Subregion = reg.NameShort,
                    Value = value 
                });
            }
           

            return list;
        }

        public async Task<MembershipEnrollment> MembershipEnrollmentById(Guid id, Guid userId)
        {
             var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var tmp = await _context.MembershipEnrollments.Include(x => x.RenewalHistory)
                                .Include(x => x.Stakeholder)
                                .ThenInclude(x => x.Individual)
                                .Include("Stakeholder.Contacts")
                                .Include("Stakeholder.Representatives")
                                .Include("Stakeholder.Addresses")
                                .Include(x => x.Association)
                                .FirstOrDefaultAsync(i => i.Id == id);
            if(user == null || tmp == null)
            {
                throw new InvalidOperationException("Unknown user or key not found");
            }
            if(user.AssociationId == Constants.ECCSA || 
                tmp.Association.Id == user.AssociationId ||
                tmp.Association.ParentId != user.AssociationId)
            {
                return tmp;
            }
            throw new InvalidOperationException("Unkown association");
            

        }

        public async Task Save(MembershipEnrollment input, Guid userId)
        {
            var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new InvalidOperationException("Unkown user");
            var tmp = await _context.Associations.Where(a => a.Id == user.AssociationId || a.ParentId == user.AssociationId).ToListAsync();
            var tmpCount = tmp.Count(a => a.Id == input.ParentAssociationId || a.ParentId == input.ParentAssociationId);

            if (user.AssociationId != Constants.ECCSA && tmpCount == 0)
            {
                throw new InvalidOperationException("Unkown association");
            }
            Guid id = Guid.NewGuid();
            MembershipEnrollment mm;
            Stakeholder sh;
            if (input.Stakeholder == null || input.Stakeholder.Id == Guid.Empty)
            {
                sh = new Stakeholder
                {
                    Id = id,
                    StakeholderType = StakeholderTypeEnum.Individual,
                    Status = Model.Common.StatusEnum.Active,
                    Individual = new Individual
                    {
                        Id = id,
                        NameEng = input.Member.NameEng,
                        NameLocal = input.Member.NameLocal,
                        TaxIdNumber = input.Member.TaxIdNumber,
                        LocationId = input.Member.LocationId,
                        MemberSince = input.Member.MemberSince,
                        SectorId = input.Member.SectorId,
                        CreatedBy = userId,
                        CreateStamp = DateTime.UtcNow,
                        UpdatedBy = userId,
                        UpdateStamp = DateTime.UtcNow
                    }

                };
                _context.Stakeholders.Add(sh);
                await _context.SaveChangesAsync();
                mm = new MembershipEnrollment
                {
                    Id = id,
                    Code = input.Code,
                    ParentAssociationId = input.ParentAssociationId,
                    MemberId = id,
                    DateOfEnrollment = input.DateOfEnrollment

                };

                // _context.Individuals.Add(ind);
                _context.MembershipEnrollments.Add(mm);
            }
            else
            {
                sh = await _context.Stakeholders.Include(s => s.Individual).FirstOrDefaultAsync(s => s.Id == input.Stakeholder.Id);
                var ind = sh.Individual;
                ind.NameEng = input.Member.NameEng;
                ind.NameLocal = input.Member.NameLocal;
                ind.TaxIdNumber = input.Member.TaxIdNumber;
                ind.LocationId = input.Member.LocationId;
                ind.MemberSince = input.Member.MemberSince;
                ind.SectorId = input.Member.SectorId;
                ind.UpdatedBy = userId;
                ind.UpdateStamp = DateTime.UtcNow;
                _context.Stakeholders.Update(sh);
                if (input.Id == Guid.Empty)
                {
                    mm = new MembershipEnrollment
                    {
                        Id = Guid.NewGuid(),
                        Code = input.Code,
                        ParentAssociationId = input.ParentAssociationId,
                        MemberId = id,
                        DateOfEnrollment = input.DateOfEnrollment
                    };
                    _context.MembershipEnrollments.Add(mm);
                }
                else
                {
                    mm = await _context.MembershipEnrollments.FirstOrDefaultAsync(f => f.Id == input.Id);
                    mm.Code = input.Code;
                    mm.ParentAssociationId = input.ParentAssociationId;
                    mm.DateOfEnrollment = input.DateOfEnrollment;
                    _context.MembershipEnrollments.Update(mm);
                }
            }
            var val = await _context.SaveChangesAsync();
            //return await MembershipEnrollmentById(id, userId);
        }

        public async Task ImportNew(MembershipEnrollment input, Guid userId)
        {
            Guid id = Guid.NewGuid();
            MembershipEnrollment mm;
            Stakeholder sh;
            input.Member.CreatedBy = userId;
            input.Member.CreateStamp = DateTime.UtcNow;
            input.Member.UpdatedBy = userId;
            input.Member.UpdateStamp = DateTime.UtcNow;
            input.Member.Id = input.Stakeholder.Id;
            sh = new Stakeholder
            {
                Id = input.Stakeholder.Id,
                StakeholderType = StakeholderTypeEnum.Individual,
                Status = Model.Common.StatusEnum.Active,
                Individual = input.Member,
                Contacts = input.Stakeholder.Contacts.Where(c => c.Address != null).ToList(),
                Representatives = input.Stakeholder.Representatives.ToList(),
                Addresses = new List<Address>()
            };
            _context.Stakeholders.Add(sh);
            await _context.SaveChangesAsync();
            mm = new MembershipEnrollment
            {
                Id = input.Id,
                Code = input.Code,
                ParentAssociationId = input.ParentAssociationId,
                MemberId = input.Id,
                DateOfEnrollment = DateTime.Today,
                LastRenewalDate = DateTime.Today,
                ExpiryDate = DateTime.Today.AddYears(1),
                RenewalStatus = MembershipWorkflowEnum.Accepted,
                MembershipStatus = StatusEnum.Active

            };

            _context.MembershipEnrollments.Add(mm);

            var val = await _context.SaveChangesAsync();

        }

        public async Task Import(MembershipEnrollment input, Guid userId)
        {
            Guid id = Guid.NewGuid();
            MembershipEnrollment mm;
            Stakeholder sh;
            
                sh = new Stakeholder
                {
                    Id = input.Stakeholder.Id,
                    StakeholderType = StakeholderTypeEnum.Individual,
                    Status = Model.Common.StatusEnum.Active,
                    Individual = new Individual
                    {
                        Id = input.Stakeholder.Id,
                        NameEng = input.Member.NameEng,
                        NameLocal = input.Member.NameLocal,
                        TaxIdNumber = input.Member.TaxIdNumber,
                        LocationId = input.Member.LocationId,
                        MemberSince = input.Member.MemberSince,
                        CreatedBy = userId,
                        CreateStamp = DateTime.UtcNow,
                        UpdatedBy = userId,
                        UpdateStamp = DateTime.UtcNow,
                        SectorId = input.Member.SectorId,
                        Sector = input.Member.Sector
                    }

                };
                _context.Stakeholders.Add(sh);
            _context.Individuals.Add(sh.Individual);
                await _context.SaveChangesAsync();
                mm = new MembershipEnrollment
                {
                    Id = input.Id,
                    Code = input.Code,
                    ParentAssociationId = input.ParentAssociationId,
                    MemberId = input.Id,
                    DateOfEnrollment = DateTime.Today,
                    LastRenewalDate = DateTime.Today,
                    ExpiryDate = DateTime.Today.AddYears(1),
                    RenewalStatus= MembershipWorkflowEnum.Accepted,
                    MembershipStatus = StatusEnum.Active

                };

                // _context.Individuals.Add(ind);
                _context.MembershipEnrollments.Add(mm);
            
            var val = await _context.SaveChangesAsync();
           
        }
        
        public async Task<MembershipEnrollmentPayload> Create(GenericInput<CreateMembershipEnrollmentInput> input)
        {
            MembershipEnrollment data = new()
            {
                Id = Guid.NewGuid()
            ,
                Code = input.PayLoad.Code
            ,
                ParentAssociationId = input.PayLoad.ParentAssociationId
            ,
                MemberId = input.PayLoad.MemberId
            ,
                DateOfEnrollment = input.PayLoad.DateOfEnrollment
            ,
                MembershipStatus = input.PayLoad.MembershipStatus
            ,
                LastRenewalDate = input.PayLoad.LastRenewalDate
            ,
                ExpiryDate = input.PayLoad.ExpiryDate
            ,
                ApplicationFee = input.PayLoad.ApplicationFee
            ,
                AnnualPayment = input.PayLoad.AnnualPayment
            ,
                InvoiceNo = input.PayLoad.InvoiceNo
            ,
                BankReceiptNo = input.PayLoad.BankReceiptNo
            ,
                Remark = input.PayLoad.Remark

            };
            await _context.MembershipEnrollments.AddAsync(data);
            await _context.SaveChangesAsync();
            return new MembershipEnrollmentPayload(data);
        }


        public async Task ApproveRejectRenewal(MembershipEnrollment data, Guid userId)
        {
            var mem = await _context.MembershipEnrollments.FirstOrDefaultAsync(m => m.Id == data.Id);
            if (mem == null) throw new KeyNotFoundException("Enrollment Record  not found");
            var tmp = await _context.RenewalHistory.FirstOrDefaultAsync(r => r.Id == mem.LastRenewalId);
            if (tmp == null) throw new KeyNotFoundException("Renewal History not found");
            if(data.ExpiryDate.Date <= DateTime.Today)
            {
                throw new InvalidOperationException($"Expiry date should be future date");
            }

            tmp.RenewalDate = DateTime.Today;
            tmp.RenewalStatus = data.RenewalStatus;
            tmp.Remark = string.IsNullOrEmpty(data.Remark)?"":data.Remark;
            
            mem.LastRenewalDate = tmp.RenewalDate;
            mem.ExpiryDate = data.ExpiryDate.Date;

            switch (tmp.RenewalStatus)
            {
                case MembershipWorkflowEnum.Accepted:
                    mem.RenewalStatus = MembershipWorkflowEnum.Accepted;
                    mem.MembershipStatus = StatusEnum.Active;
                    break;
                case MembershipWorkflowEnum.Rejected:
                    mem.RenewalStatus = MembershipWorkflowEnum.Rejected;
                    mem.MembershipStatus = StatusEnum.Inactive;
                    break;

            }
            mem.Remark = tmp.Remark;
            _context.RenewalHistory.Update(tmp);
            _context.MembershipEnrollments.Update(mem);
            await _context.SaveChangesAsync();
           

        }

       
        public async Task<MembershipEnrollmentPayload> Update(GenericInput<UpdateMembershipEnrollmentInput> data)
        {
            var tmp = await _context.MembershipEnrollments.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            tmp.Code = data.PayLoad.Code;
            tmp.ParentAssociationId = data.PayLoad.ParentAssociationId;
            tmp.MemberId = data.PayLoad.MemberId;
            tmp.DateOfEnrollment = data.PayLoad.DateOfEnrollment;
            tmp.MembershipStatus = data.PayLoad.MembershipStatus;
            tmp.LastRenewalDate = data.PayLoad.LastRenewalDate;
            tmp.ExpiryDate = data.PayLoad.ExpiryDate;
            tmp.ApplicationFee = data.PayLoad.ApplicationFee;
            tmp.AnnualPayment = data.PayLoad.AnnualPayment;
            tmp.InvoiceNo = data.PayLoad.InvoiceNo;
            tmp.BankReceiptNo = data.PayLoad.BankReceiptNo;
            tmp.Remark = data.PayLoad.Remark;

            _context.MembershipEnrollments.Update(tmp);
            await _context.SaveChangesAsync();
            return new MembershipEnrollmentPayload(tmp);

        }

        public async Task Delete(MembershipEnrollment data)
        {
            var tmp = await _context.MembershipEnrollments.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.MembershipEnrollments.Remove(tmp);
            await _context.SaveChangesAsync();
        }
       
    }
}