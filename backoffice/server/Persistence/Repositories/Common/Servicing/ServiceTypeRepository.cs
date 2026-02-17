using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common.Servicing
{
    public class ServiceTypeRepository : BaseRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public ServiceTypeRepository(EccsaContext context, EccsaUserContext userContext) : base(context)
        {
            _context = context;
            _userContext = userContext;
        }
        public IQueryable<ServiceOwner> ServiceOwners()
        {
            return _context.ServiceOwners;
            ;
        }
        public async Task<ServiceOwner> ServiceOwnerById(ECCSAModuleFlag id)
        {
            return await _context.ServiceOwners.FirstOrDefaultAsync(s => s.Id == id);
            ;
        }
        public async Task<List<ServiceOwner>> ActiveServiceOwners()
        {
            var lst = await _context.ServiceOwners.Where(s => s.Status == StatusEnum.Active).ToListAsync();
            var tmpServiceOwners = await _context.ServiceTypes
                .Where(t => t.Status == StatusEnum.Active && !t.IsResourceServiceType &&
                (!t.StartDate.HasValue || t.StartDate.Value <= DateTime.Today) &&
                (!t.EndDate.HasValue || t.EndDate.Value >= DateTime.Today))
                .GroupBy(t => t.ServiceOwner)
                .Select(x => x.Key)
                .ToListAsync();

            var tmp = new List<ServiceOwner>();
            tmp = lst.Where(l => tmpServiceOwners.Any(x => x == l.Id)).OrderBy(x => x.Title).ToList();
            return tmp;
        }

        public async Task<List<ServiceOwner>> ActiveEventsServiceOwners()
        {
            var lst = await _context.ServiceOwners.Where(s => s.Status == StatusEnum.Active).ToListAsync();
            var tmpServiceOwners = await _context.ServiceTypes
                .Where(t => t.Status == StatusEnum.Active && !t.IsResourceServiceType)
                .GroupBy(t => t.ServiceOwner)
                .Select(x => x.Key)
                .ToListAsync();

            var tmp = new List<ServiceOwner>();
            tmp = lst.Where(l => tmpServiceOwners.Any(x => x == l.Id)).ToList();
            return tmp;
        }

        public IQueryable<ServiceType> ServiceTypes(Guid userId)
        {
            return _context.ServiceTypes.Include(x => x.ServiceRequests);
        }

        public List<ServiceType> ServiceTypesWithRequiremnts(StatusEnum status = StatusEnum.Active)
        {
            return _context.ServiceTypes.Include(x => x.ServiceRequirements)
                .Where(s => s.Status == status && !s.IsResourceServiceType &&
                (!s.StartDate.HasValue || s.StartDate.Value <= DateTime.Today) &&
                (!s.EndDate.HasValue || s.EndDate.Value >= DateTime.Today)
                ).ToList();
        }

        public async Task<ServiceType> ServiceTypeWithRequiremnts(Guid id)
        {
            var serviceType = await _context.ServiceTypes.Include(s => s.ServiceRequirements)
                //IncludeFilter(x => x.ServiceRequirements.Where(s => s.Status == StatusEnum.Active))
                .FirstOrDefaultAsync(s => s.Id == id);
            serviceType.ServiceRequirements = serviceType.ServiceRequirements.Where(s => s.Status == StatusEnum.Active).ToList();
            var lookupRequirements = serviceType.ServiceRequirements.
                Where(s => s.InputMethod == ServiceInputTypeEnum.SimpleLookup);
            var tabularRequirements = serviceType.ServiceRequirements.
                Where(s => s.InputMethod == ServiceInputTypeEnum.Tabular);
            //var lookupTypes = await _context.LookupTypes.
            //        IncludeFilter(l => l.LookupValues.Where(v => v.Status == StatusEnum.Active))
            //        .ToListAsync();

            var lookupTypes = await _context.LookupTypes.
                    Include(l => l.LookupValues)
                    .ToListAsync();
            foreach (var lt in lookupTypes)
            {
                lt.LookupValues = lt.LookupValues.Where(v => v.Status == StatusEnum.Active).OrderBy(s => s.Name).ToList();
            }

            if (lookupRequirements.Any())
            {
                serviceType.RequirementLookups =
                    lookupTypes.Where(l => lookupRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
            }
            else
            {
                serviceType.RequirementLookups = new List<LookupType>();
            }
            if (tabularRequirements.Any())
            {
                //serviceType.TabularRequirements = (await _context.TabularDataSources.
                //    IncludeFilter(l => l.Details.Where(v => v.Status == StatusEnum.Active))
                //    .ToListAsync())
                //    .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();

                serviceType.TabularRequirements = (await _context.TabularDataSources.
                     Include(l => l.Details)
                     .ToListAsync())
                     .Where(l => tabularRequirements.Any(r => r.LookupTypeId == l.Id)).ToList();
                foreach (var item in serviceType.TabularRequirements)
                {
                    item.Details = item.Details.Where(d => d.Status == StatusEnum.Active).ToList();
                }

                if (serviceType.TabularRequirements.Any(s => s.Details.Any(d => d.DataType == TabularDataSourceDetailDataType.Lookup)))
                {
                    serviceType.RequirementLookups ??= new List<LookupType>();
                    var list = serviceType.TabularRequirements.Where(s => s.Details.Any(d => d.DataType == TabularDataSourceDetailDataType.Lookup)).ToList();
                    foreach (var tr in list)
                    {
                        var lts = lookupTypes.Where(lt => tr.Details.Any(d => d.LookupId == lt.Id) &&
                        !serviceType.RequirementLookups.Any(rl => rl.Id == lt.Id)).ToList();
                        serviceType.RequirementLookups.AddRange(lts);
                    }
                }
            }
            if (serviceType.AllowOptionalEntries)
            {
                serviceType.OptionalEntryLabels = await _context.LookupValues.
                    Where(s => s.LookupTypeId == LookupType.OptionalRequirements).Select(s => new LookupValue
                    {
                        Id = s.Id,
                        LookupTypeId = s.LookupTypeId,
                        Name = s.Name,
                        Value = s.Value,

                    }).ToListAsync();

            }
            else
            {
                serviceType.OptionalEntryLabels = new List<LookupValue>();
            }
            return serviceType;
        }

        public async Task<List<ServiceType>> ServiceTypes(ECCSAModuleFlag subSystem)
        {

            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var serviceTypes = await _context.ServiceTypes
                .Include(x => x.ServiceRequests)
                .Include(x => x.ServiceRequirements)
                .Where(x => x.ServiceOwner == subSystem).ToListAsync();

            return serviceTypes;
        }

        public async Task<ServiceType> ServiceTypeById(Guid id)
        {
            try
            {
                // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
                var serviceType = await _context.ServiceTypes
                    .Include(x => x.ServiceRequests)
                    .Include(x => x.ServiceTypeReports)
                    .Include(x => x.ServiceRequirements)
                    //.IncludeFilter(x => x.ServiceRequirements.Where(s => s.Status == StatusEnum.Active))
                    .FirstOrDefaultAsync(i => i.Id == id);
                serviceType.ServiceRequirements = serviceType.ServiceRequirements.Where(s => s.Status == StatusEnum.Active).ToList();
                var lookupTypes = await _context.LookupTypes.Where(l => l.Status == StatusEnum.Active).ToDictionaryAsync(l => l.Id);
                foreach (var r in serviceType.ServiceRequirements)
                {
                    r.IsParent = !r.ParentId.HasValue || r.RequirementType == RequirementTypeEnum.Header;
                    r.HasChild = serviceType.ServiceRequirements.Any(sr => sr.ParentId.HasValue && sr.ParentId.Value == r.Id);
                    if (r.InputMethod == ServiceInputTypeEnum.SimpleLookup)
                    {
                        if (r.LookupTypeId.HasValue)
                            r.InputType = lookupTypes[r.LookupTypeId.Value].Name;
                        else
                            r.InputType = "Lookup Not set";

                    }
                    else
                    {
                        r.InputType = Enum.GetName(typeof(ServiceInputTypeEnum), r.InputMethod);
                    }
                }

                serviceType.WorkerList ??= new List<ServiceTypeWorker>();

                var users = await _userContext.Users.Where(u => u.AssociationId == Constants.ECCSA && u.AssignedModules > 0).ToListAsync();
                serviceType.WorkerDetails = new List<ServiceTypeWorkerDetail>();
                foreach (var worker in serviceType.WorkerList)
                {
                    var user = users.FirstOrDefault(u => u.Id == worker.UserId);
                    if (user == null) continue;
                    serviceType.WorkerDetails.Add(new ServiceTypeWorkerDetail
                    {
                        FullName = user.FullName,
                        Email = user.Email,
                        MobileNo = string.IsNullOrEmpty(user.MobileNo) ? user.PhoneNumber : user.MobileNo,
                    });
                }
                return serviceType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LookupType>> LookupTypes(ECCSAModuleFlag subSystem)
        {
            return await _context.LookupTypes
                //.Where(l => l.Owner.HasFlag(subSystem))
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

        public async Task<ServiceTypePayload> Create(GenericInput<CreateServiceTypeInput> input)
        {
            ServiceType data = new()
            {
                Id = Guid.NewGuid()

            };
            await _context.ServiceTypes.AddAsync(data);
            _ = await _context.SaveChangesAsync();
            return new ServiceTypePayload(data);
        }

        public async Task<ServiceTypePayload> Update(GenericInput<UpdateServiceTypeInput> data)
        {
            var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");

            _context.ServiceTypes.Update(tmp);
            await _context.SaveChangesAsync();
            return new ServiceTypePayload(tmp);

        }

        public async Task Delete(ServiceType data)
        {
            var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.ServiceTypes.Remove(tmp);
            await RemoveKeyword(data.Id, data.Id);
            await _context.SaveChangesAsync();
        }


        public async Task SaveEventServiceType(ServiceType data)
        {
            var edetail = await _context.EventDetails.FirstOrDefaultAsync(d => d.DetailId == data.Id);
            if (edetail == null)
            {
                throw new KeyNotFoundException("Event Detail Not found");
            }
            var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (tmp == null)
            {
                _context.ServiceTypes.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            edetail.ServiceTypeId = edetail.DetailId;
            _context.EventDetails.Update(edetail);
            await _context.SaveChangesAsync();
            var serviceType = await _context.ServiceTypes.Include(r => r.ServiceRequirements).FirstOrDefaultAsync(x => x.Id == data.Id);


        }
        public async Task<ServiceType> Save(ServiceType data, Guid userId)
        {
            switch (data.RateBasis)
            {
                case RateBasisEnum.Flat:
                    data.MemberServiceCharge = data.NonMemberServiceCharge;
                    break;
                case RateBasisEnum.Adhoc:
                    data.MemberServiceCharge = 0;
                    data.NonMemberServiceCharge = 0;
                    break;
            }
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
                data.CreatedBy = userId;
                data.CreateStamp = DateTime.UtcNow;
                data.UpdatedBy = userId;
                data.UpdateStamp = DateTime.UtcNow;
                data.WorkerList = new List<ServiceTypeWorker>();
                _context.ServiceTypes.Add(data);
            }
            else
            {
                var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == data.Id);
                if (tmp == null)
                {
                    throw new KeyNotFoundException("Service Type Not found");
                }
                tmp.NameEng = data.NameEng;
                tmp.NameLocal = data.NameLocal;
                tmp.DescriptionEng = data.DescriptionEng;
                tmp.DescriptionLocal = data.DescriptionLocal;

                tmp.RateBasis = data.RateBasis;
                tmp.ApprovalRequired = data.ApprovalRequired;
                tmp.ApprovalAttachmentRequired = data.ApprovalAttachmentRequired;
                tmp.AllowAdditionalAttachments = data.AllowAdditionalAttachments;
                tmp.AllowOptionalEntries = data.AllowOptionalEntries;
                tmp.StartDate = data.StartDate;
                tmp.EndDate = data.EndDate;
                tmp.IsArbitration = data.IsArbitration;
                if (data.RateBasis == RateBasisEnum.Adhoc)
                {
                    tmp.MemberServiceCharge = 0;
                    tmp.NonMemberServiceCharge = 0;
                }
                else
                {
                    tmp.MemberServiceCharge = data.MemberServiceCharge;
                    tmp.NonMemberServiceCharge = data.NonMemberServiceCharge;
                }
                tmp.Status = data.Status;
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;
                _context.ServiceTypes.Update(tmp);
            }
            await GenerateBacklog();
            await GenerateKeyword(data);
            await _context.SaveChangesAsync();
            var serviceType = await _context.ServiceTypes.Include(r => r.ServiceRequirements).FirstOrDefaultAsync(x => x.Id == data.Id);

            return serviceType;
        }
        public async Task Save(ServiceRequirement data, Guid userId)
        {
            var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == data.ServiceTypeId);
            if (string.IsNullOrEmpty(data.DescriptionLocal))
                data.DescriptionLocal = string.Empty;
            if (string.IsNullOrEmpty(data.NameLocal))
                data.NameLocal = string.Empty;
            if (string.IsNullOrEmpty(data.LabelLocal))
                data.LabelLocal = string.Empty;
            data.RequirementType = RequirementTypeEnum.Normal;

            var req = await _context.ServiceRequirements.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (req == null)
            {
                data.Id = Guid.NewGuid();

                _context.ServiceRequirements.Add(data);
            }
            else
            {
                data.Status = req.Status;
                _context.Entry(req).CurrentValues.SetValues(data);
            }
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceTypes.Update(tmp);
            await GenerateKeyword(tmp, data);
            await _context.SaveChangesAsync();
        }


        public async Task SaveServiceRequirementHeader(Guid id, string orderNo, String name, Guid serviceTypeId, Guid userId)
        {
            var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == serviceTypeId);
            var req = await _context.ServiceRequirements.FirstOrDefaultAsync(r => r.Id == id);
            if (req == null)
            {
                req = new ServiceRequirement
                {
                    Id = Guid.NewGuid(),
                    OrderNo = orderNo,
                    NameEng = name,
                    Label = name,
                    ServiceTypeId = serviceTypeId,
                    RequirementType = RequirementTypeEnum.Header,
                    Status = StatusEnum.Active
                };
                _context.ServiceRequirements.Add(req);
            }
            else
            {
                req.OrderNo = orderNo;
                req.NameEng = name;
                req.Label = name;
                _context.ServiceRequirements.Update(req);
            }
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceTypes.Update(tmp);
            await GenerateKeyword(tmp, req);
            await _context.SaveChangesAsync();
        }

        public async Task SaveServiceRequirementInvoice(Guid id, string orderNo, String name, String nameLocal,
            String description, String descriptionLocal, Guid serviceTypeId, bool isForOpponent, Guid userId)
        {
            var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(x => x.Id == serviceTypeId);


            var req = await _context.ServiceRequirements.FirstOrDefaultAsync(r => r.Id == id);
            if (req == null)
            {
                req = new ServiceRequirement
                {
                    Id = Guid.NewGuid(),
                    OrderNo = orderNo,
                    DescriptionEng = description,
                    DescriptionLocal = descriptionLocal,
                    NameEng = name,
                    NameLocal = nameLocal,
                    Label = name,
                    LabelLocal = nameLocal,
                    ServiceTypeId = serviceTypeId,
                    RequirementType = RequirementTypeEnum.Invoice,
                    InputMethod = ServiceInputTypeEnum.Payment,
                    Status = StatusEnum.Active,
                    ApproveOnSubmit = true,
                    IsForOpponent = isForOpponent
                };
                _context.ServiceRequirements.Add(req);
            }
            else
            {
                req.OrderNo = orderNo;
                req.NameEng = name;
                req.Label = name;
                req.LabelLocal = nameLocal;
                req.DescriptionEng = description;
                req.IsForOpponent = isForOpponent;

                _context.ServiceRequirements.Update(req);
            }
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceTypes.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task Save(SimpleLookup data, Guid serviceTypeId, Guid requirementId, Guid userId)
        {
            var tmp = await _context.ServiceTypes
                .Include(x => x.ServiceRequirements)
                .FirstOrDefaultAsync(x => x.Id == serviceTypeId);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Service Type Not found");
            }


            var req = await _context.ServiceRequirements.FirstOrDefaultAsync(r => r.Id == requirementId);
            if (req == null)
            {
                throw new KeyNotFoundException("Requirement Not found");
            }
            if (req.SelectSources == null)
                req.SelectSources = new List<SimpleLookup>();

            if (req.SelectSources.Exists(r => r.Id == data.Id))
            {
                req.SelectSources.Find(r => r.Id == data.Id).Name = data.Name;
            }
            else
            {
                req.SelectSources.Add(data);
            }
            _context.ServiceRequirements.Update(req);
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceTypes.Update(tmp);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteRequirement(Guid id, Guid serviceTypeId, Guid userId)
        {
            if (id == Guid.Empty) return;
            var tmp = await _context.ServiceTypes
                         .Include(x => x.ServiceRequests)
                         .Include(x => x.ServiceRequirements)
                         .FirstOrDefaultAsync(x => x.Id == serviceTypeId);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Service Type Not found");
            }

            var req = await _context.ServiceRequirements.FirstOrDefaultAsync(r => r.Id == id);
            if (req == null)
            {
                throw new KeyNotFoundException("Requirement Not found");
            }
            if (tmp.ServiceRequests.Any())
            {
                req.Status = StatusEnum.Inactive;
                _context.ServiceRequirements.Update(req);
            }
            else
            {
                _context.ServiceRequirements.Remove(req);
            }
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.ServiceTypes.Update(tmp);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteServiceType(Guid id, Guid userId, ECCSAModuleFlag module)
        {
            if (id == Guid.Empty) return;
            var tmp = await _context.ServiceTypes
                         .Include(x => x.ServiceRequirements)
                         .Include(x => x.ServiceRequests)
                         .FirstOrDefaultAsync(x => x.Id == id);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Service Type Not found");
            }
            if (tmp.ServiceRequirements.Count > 0 ||
                tmp.ServiceRequests.Count > 0)
            {
                tmp.Status = StatusEnum.Inactive;
                tmp.UpdatedBy = userId;
                tmp.UpdateStamp = DateTime.UtcNow;
                _context.ServiceTypes.Update(tmp);
            }
            else
            {
                _context.ServiceTypes.Remove(tmp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task SaveWorker(ServiceType serviceType)
        {
            var tmp = await _context.ServiceTypes.FirstOrDefaultAsync(s => s.Id == serviceType.Id);
            if (tmp == null)
            {
                throw new KeyNotFoundException("Service Type Not found");
            }
            tmp.WorkerList = serviceType.WorkerList;
            _context.ServiceTypes.Update(tmp);
            await _context.SaveChangesAsync();
        }


    }
}