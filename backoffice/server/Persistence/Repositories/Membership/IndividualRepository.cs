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
    public class IndividualRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public IndividualRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<int> ListCount(Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var membershipQL = _context.MembershipEnrollments
                            .Where(m => (m.ParentAssociationId == user.AssociationId || user.AssociationId == Constants.ECCSA)
                            && m.Stakeholder.StakeholderType == StakeholderTypeEnum.Individual);
            var members = await _context.Individuals.Include(m => m.BusinessProfiles).Include(m => m.Location)
                .Where(i => membershipQL.Any(m => m.MemberId == i.Id)).CountAsync(s => s.Stakeholder.Status == StatusEnum.Active);
 
            return members;

        }
        public async Task<List<Individual>> List(Guid userId, Guid associationId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var membershipQL = _context.MembershipEnrollments
                            .Include(s => s.Stakeholder)
                            .Include(s => s.Association)
                            .Where(m => m.ParentAssociationId == user.AssociationId && m.Stakeholder.StakeholderType == StakeholderTypeEnum.Individual);
            var membership = await membershipQL.ToListAsync();
            var members = await _context.Individuals.Include(m => m.LegalDocuments).Include(m => m.BusinessProfiles).Include(m => m.Location)
                .Where(i => membershipQL.Any(m => m.MemberId == i.Id)).ToListAsync();
            foreach (var m in members)
            {
                m.Memberships = membership.Where(v => v.MemberId == m.Id).ToList();
            }
            return members;

        }
        public IQueryable<Individual> Individuals(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Individuals.Include(x => x.Location)
.Include(x => x.BusinessProfiles)
.Include(x => x.LegalDocuments)
;
        }

        public async Task<Individual> IndividualById(Guid id, Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var membership = await _context.MembershipEnrollments
                            .Include(s => s.Stakeholder)
                            .Include(s => s.Association)

                            .Where(m => (m.Association.Id == user.AssociationId ||
                                        m.Association.ParentId == user.AssociationId ||
                                        user.AssociationId == Constants.ECCSA) && m.MemberId == id).ToListAsync();
            var member = await _context.Individuals
                            .Include(m => m.LegalDocuments)
                            .Include(m => m.BusinessProfiles)
                            .Include(m => m.Location)
                            .Include(m => m.BusinessLicenses)
                            .Include(m => m.Services)
                            .Include(m => m.Manufacturings)
                            .Include(m => m.Trades)
                .FirstOrDefaultAsync(i => i.Id == id) ?? new Individual { BusinessLicenses = new List<IndividualBusiness>(),
                Manufacturings = new List<IndividualManufacturing>(), Services = new List<IndividualService>(),
                Trades = new List<IndividualTrade>(), Location = new Location()};

            member.Memberships = membership.Where(v => v.MemberId == id).ToList();
            member.BusinessProfiles = await _context.BusinessProfiles.Include(b => b.ProfileAttribure).Where(b => b.IndividualId == member.Id).ToListAsync();
            member.LegalDocuments = await _context.LegalDocuments.Include(b => b.Requirement).Where(b => b.IndividualId == member.Id).ToListAsync();

            return member;
        }
        public async Task<Individual> GetIndividualByUserId(Guid userId)
        {
            return await _context.Individuals.FirstOrDefaultAsync(i=> i.AssociatedUserId == userId);
        }
        public async Task<MembershipEnrollmentPublicModel> GetEnrollmentById(Guid associationId, Guid individualId)
        {
            var s = await _context.MembershipEnrollments
                                         .FirstOrDefaultAsync(m => m.MemberId == individualId && m.ParentAssociationId == associationId);
            if(s == null) return null;
            return new MembershipEnrollmentPublicModel
            {
                AssociationId = s.Association.Id,
                Code = s.Code,
                MemberSince = s.EnrollmentDate,
                RenewedOn = s.RenewalDate,
                ExpiresOn = s.ExpiryDate.ToString("yyyy-MM-dd"),
                InvoiceNo = s.InvoiceNo,
                ReceiptNo = s.BankReceiptNo,
                Fee = s.ApplicationFee.ToString(),
                Status = s.MembershipStatus.ToString(),
                Association = s.Association.Name,
                AssociationType = (int)s.Association.AssociationType
            };
            
        }
        public async Task<List<MembershipEnrollmentPublicModel>> GetUserMembershipInfo(Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("invalid user");
            }
            var member = await _context.Individuals
                .Include(m => m.Location)

                .FirstOrDefaultAsync(i => i.Id == user.AssociationId);
            List<MembershipEnrollmentPublicModel> lst = new();
            var renewals = _context.Applicants.Where(s => s.IndividualId == user.AssociationId).ToList();
            if (member != null)
            {
                lst = await _context.MembershipEnrollments
                               .Where(m => m.MemberId == user.AssociationId)
                               .Select(s => new MembershipEnrollmentPublicModel
                               {
                                   AssociationId = s.Association.Id,
                                   Code = s.Code,
                                   MemberSince = s.EnrollmentDate,
                                   RenewedOn = s.RenewalDate,
                                   ExpiresOn = s.ExpiryDate.ToString("yyyy-MM-dd"),
                                   InvoiceNo = s.InvoiceNo,
                                   ReceiptNo = s.BankReceiptNo,
                                   Fee = s.ApplicationFee.ToString(),
                                   Status = s.MembershipStatus.ToString(),
                                   Association = s.Association.NameEng,
                                   AssociationLocal = s.Association.NameLocal,
                                   AssociationType = (int)s.Association.AssociationType
                               }).ToListAsync();
                lst.ForEach(s =>
                {
                    s.RenewalStatus = renewals.Any(a => a.AssociationId == s.AssociationId) ?
                    renewals.FirstOrDefault(a => a.AssociationId == s.AssociationId).ApplicationStatus : null;
                });
            }
            return lst.OrderBy(s => s.MemberSince).ToList();
        }
        public async Task<IndividualPayload> Create(GenericInput<CreateIndividualInput> input)
        {
            Individual data = new()
            {
                Id = Guid.NewGuid()
            ,
                NameEng = input.PayLoad.NameEng
            ,
                NameLocal = input.PayLoad.NameLocal
            ,
                MemberSince = input.PayLoad.MemberSince
            ,
                TaxIdNumber = input.PayLoad.TaxIdNumber
            ,
                LocationId = input.PayLoad.LocationId
            ,
                TradeRegistrationNumber = input.PayLoad.TradeRegistrationNumber
            ,
                AssociatedUserId = input.PayLoad.AssociatedUserId
            ,
                CreatedBy = input.UserId
            ,
                CreateStamp = DateTime.UtcNow
            ,
                UpdatedBy = input.UserId
            ,
                UpdateStamp = DateTime.UtcNow

            };
            await _context.Individuals.AddAsync(data);
            var val = await _context.SaveChangesAsync();
            return new IndividualPayload(data);
        }
        public async Task PrepareIdCard(IdCardInfo data)
        {
            var enrollment = await _context.MembershipEnrollments.FirstOrDefaultAsync(a => a.MemberId == data.IndividualId && a.ParentAssociationId == data.AssociationId);
            if (enrollment == null)
                throw new InvalidOperationException("Membership Info not found");
            var individual = await _context.Individuals.Include(i => i.Stakeholder)
                    .FirstOrDefaultAsync(i => i.Id ==data.IndividualId);
            if (enrollment.MembershipStatus == StatusEnum.Active && enrollment.ExpiryDate > DateTime.Today)
            {
                enrollment.IdCard = new IdCardInfo()
                {
                    Code = enrollment.Code,
                    ExpiryDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd"),
                    DateIssued = DateTime.Today.ToString("yyyy-MM-dd"),
                    CompanyName = individual.NameEng,
                    Sector = individual.Sector,
                    FullName = data.FullName,
                    Email = data.Email,
                    HouseNo = data.HouseNo,
                    LicenseNo = data.LicenseNo,
                    MembershipNo = data.MembershipNo,
                    PhoneNumber = data.PhoneNumber,
                    TitleName = data.TitleName,
                    Woreda = data.Woreda,
                    Zone = data.Zone,
                    ReadyForPrint = true
                };
            }
            else
            {
                enrollment.IdCard = new IdCardInfo();
                
            }
            _context.MembershipEnrollments.Update(enrollment);
            await _context.SaveChangesAsync();
        }
        public async Task<IndividualPayload> Update(GenericInput<UpdateIndividualInput> data)
        {
            var tmp = await _context.Individuals.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            tmp.NameEng = data.PayLoad.NameEng;
            tmp.NameLocal = data.PayLoad.NameLocal;
            tmp.MemberSince = data.PayLoad.MemberSince;
            tmp.TaxIdNumber = data.PayLoad.TaxIdNumber;
            tmp.LocationId = data.PayLoad.LocationId;
            tmp.TradeRegistrationNumber = data.PayLoad.TradeRegistrationNumber;
            tmp.AssociatedUserId = data.PayLoad.AssociatedUserId;
            tmp.UpdatedBy = data.UserId;
            tmp.UpdateStamp = DateTime.UtcNow;

            _context.Individuals.Update(tmp);
            await _context.SaveChangesAsync();
            return new IndividualPayload(tmp);

        }



        public async Task Delete(Individual data)
        {
            var tmp = await _context.Individuals.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Individuals.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task<Individual> IndividualByTIN(string TIN, Guid userId)
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var member = await _context.Individuals.Include(m => m.LegalDocuments).Include(m => m.BusinessProfiles).Include(m => m.Location)
                .FirstOrDefaultAsync(i => i.TaxIdNumber == TIN);
            return member;
        }


        public async Task<List<IndividualBusiness>> IndividualBusinesses(Guid individualId)
        {
            return await _context.IndividualBusinesses.Include(x => x.Individual).Where(x => x.IndividualId == individualId).ToListAsync();
        }
        public async Task<IndividualBusiness> IndividualBusinessById(Guid id, Guid individualId)
        {
            return await _context.IndividualBusinesses.Include(x => x.Individual).FirstOrDefaultAsync(x => x.Id == id && x.IndividualId == individualId);
        }

        public async Task<List<IndividualManufacturing>> IndividualManufacturings(Guid individualId)
        {
            return await _context.IndividualManufacturings.Include(x => x.Individual).Where(x => x.IndividualId == individualId).ToListAsync();
        }
        public async Task<IndividualManufacturing> IndividualManufacturingById(Guid id, Guid individualId)
        {
            return await _context.IndividualManufacturings.Include(x => x.Individual).FirstOrDefaultAsync(x => x.Id == id && x.IndividualId == individualId);
        }

        public async Task<List<IndividualService>> IndividualServices(Guid individualId)
        {
            return await _context.IndividualServices.Include(x => x.Individual).Where(x => x.IndividualId == individualId).ToListAsync();
        }
        public async Task<IndividualService> IndividualServiceById(Guid id, Guid individualId)
        {
            return await _context.IndividualServices.Include(x => x.Individual).FirstOrDefaultAsync(x => x.Id == id && x.IndividualId == individualId);
        }

        public async Task<List<IndividualTrade>> IndividualTrades(Guid individualId)
        {
            return await _context.IndividualTrades.Include(x => x.Individual).Where(x => x.IndividualId == individualId).ToListAsync();
        }
        public async Task<IndividualTrade> IndividualTradeById(Guid id, Guid individualId)
        {
            return await _context.IndividualTrades.Include(x => x.Individual).FirstOrDefaultAsync(x => x.Id == id && x.IndividualId == individualId);
        }

        private async Task ValidateDetail(IIndividualDetail data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var individual = await _context.Individuals.FirstOrDefaultAsync(i => i.Id == data.IndividualId);
            if (individual == null) throw new ArgumentNullException(nameof(individual));
        }
        public async Task Save(IndividualBusiness data, Guid userId)
        {
            await ValidateDetail(data);
            data.SerNo = "1";
            if (data.Id == Guid.Empty)
                data.Id = Guid.NewGuid();
            var tmp = await _context.IndividualBusinesses.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                _context.IndividualBusinesses.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(IndividualManufacturing data, Guid userId)
        {
            await ValidateDetail(data);
            data.SerNo = "1";
            if (data.Id == Guid.Empty)
                data.Id = Guid.NewGuid();
            var tmp = await _context.IndividualManufacturings.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                _context.IndividualManufacturings.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }


        public async Task Save(IndividualService data, Guid userId)
        {
            await ValidateDetail(data);
            data.SerNo = "1";
            if (data.Id == Guid.Empty)
                data.Id = Guid.NewGuid();
            var tmp = await _context.IndividualServices.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                _context.IndividualServices.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Save(IndividualTrade data, Guid userId)
        {
            await ValidateDetail(data);
            var trades = await _context.IndividualTrades.Where(t => t.IndividualId == data.IndividualId).ToListAsync();
            if (string.IsNullOrEmpty(data.SerNo))
                data.SerNo = (trades.Count + 1).ToString();
            if (data.Id == Guid.Empty)
                data.Id = Guid.NewGuid();
            var tmp = await _context.IndividualTrades.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                _context.IndividualTrades.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIndividualTrade(Guid id, Guid individualId, Guid userId)
        {
            var tmp = await _context.IndividualTrades.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null) throw new ArgumentNullException(nameof(tmp));
            if (tmp.IndividualId != individualId) throw new InvalidOperationException("Invalid data");
            _context.IndividualTrades.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteIndividualService(Guid id, Guid individualId, Guid userId)
        {
            var tmp = await _context.IndividualServices.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null) throw new ArgumentNullException(nameof(tmp));
            if (tmp.IndividualId != individualId) throw new InvalidOperationException("Invalid data");
            _context.IndividualServices.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIndividualManufacturing(Guid id, Guid individualId, Guid userId)
        {
            var tmp = await _context.IndividualManufacturings.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null) throw new ArgumentNullException(nameof(tmp));
            if (tmp.IndividualId != individualId) throw new InvalidOperationException("Invalid data");
            _context.IndividualManufacturings.Remove(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIndividualLicenses(Guid id, Guid individualId, Guid userId)
        {
            var tmp = await _context.IndividualBusinesses.FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null) throw new ArgumentNullException(nameof(tmp));
            if (tmp.IndividualId != individualId) throw new InvalidOperationException("Invalid data");
            _context.IndividualBusinesses.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}