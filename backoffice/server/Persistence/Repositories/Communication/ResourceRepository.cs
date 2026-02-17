using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Communication
{
    public class ResourceRepository
    {
        private readonly EccsaContext _context;
        public ResourceRepository(EccsaContext context)
        {
            _context = context;
        }

        public async Task<List<Resource>> ResourcesForApi()
        {
            var list = await _context.Resources.OrderByDescending(r => r.PublicationDate).ToListAsync();
            foreach(var resource in list)
            {
                if (resource.DigitalCost.HasValue && resource.DigitalCost.Value > 0.01m)
                {
                    resource.Extension = "";
                    resource.Url = "";
                }
            }
            return list;
        }

        public async Task<List<Resource>> ResourcesForApi(Guid typeId)
        {
            var list = await _context.Resources.Where(s => s.TypeId == typeId).ToListAsync();
            foreach (var resource in list)
            {
                if (resource.DigitalCost.HasValue && resource.DigitalCost.Value > 0.01m)
                {
                    resource.Extension = "";
                    resource.Url = "";
                }
            }
            return list;
        }
        public async Task<List<Resource>> Resources(ECCSAModuleFlag module)
        {
            return await _context.Resources.Where(r => r.OwnerModule == module).ToListAsync();
        }
        public async Task<List<ResourceDescription>> ResourceLookups()
        {
            return (await _context.Resources.ToListAsync()).Select(s => new ResourceDescription(s.Id, s.Title, s.Type)).ToList();
        }
        public async Task<Resource> ById(Guid id)
        {
            return await _context.Resources.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Resource> ByIdForApi(Guid id)
        {
            var res = await _context.Resources.FirstOrDefaultAsync(i => i.Id == id);
            if (res != null && res.DigitalCost.HasValue && res.DigitalCost.Value > 0.01m)
            {
                res.Extension = "";
                res.Url = "";
            }
            return res;
        }
        public async Task<Resource> ModuleById(Guid id, ECCSAModuleFlag module)
        {
            return await _context.Resources.FirstOrDefaultAsync(i => i.Id == id && i.OwnerModule == module);
        }
        public async Task Save(Resource data, Guid userId)
        {
            if (data.Id == Guid.Empty)
            {
                data.Id = Guid.NewGuid();
            }
			var tmp = await _context.Resources.FirstOrDefaultAsync(r => r.Id == data.Id);
			var serviceType = await _context.ServiceTypes.FirstOrDefaultAsync(s => s.Id == data.Id);
			data.UpdatedBy = userId;
			data.UpdateStamp = DateTime.UtcNow;
			
			if (string.IsNullOrEmpty(data.Url))
			{
				data.Url = String.Empty;
			}
			if(data.DigitalCost.HasValue && data.DigitalCost.Value > 0.01m && !(string.IsNullOrEmpty(data.Url) && string.IsNullOrEmpty(data.Extension)))
            {
				if(serviceType == null)
                {
                    serviceType = new ServiceType
                    {
                        Id = data.Id,
                        ServiceOwner = data.OwnerModule,
						IsResourceServiceType = true,
						AllowAdditionalAttachments= false,
                        AllowOptionalEntries = false,
						ApprovalAttachmentRequired= false,
						ApprovalRequired= false,
						CertificateType = CertificateTypeEnum.None,
						CreatedBy = userId,
						CreateStamp = DateTime.UtcNow,
						DescriptionEng = data.Description,
						DescriptionLocal = data.DescriptionLocal,
						MemberServiceCharge = data.DigitalCost.Value,
						NonMemberServiceCharge = data.DigitalCost.Value,
						NameEng = data.Title,
						NameLocal = data.TitleLocal,
						RateBasis = RateBasisEnum.Flat,
						Status = StatusEnum.Active,
						UpdatedBy = userId,
						UpdateStamp = DateTime.UtcNow,

                    };
					_context.ServiceTypes.Add(serviceType);
                }else 
                {
					serviceType.NameEng = data.Title;
                    serviceType.NameLocal = data.TitleLocal;
					serviceType.DescriptionEng = data.Description;
                    serviceType.DescriptionLocal = data.DescriptionLocal;
					serviceType.MemberServiceCharge = data.DigitalCost.Value;
					serviceType.NonMemberServiceCharge = data.DigitalCost.Value;
					serviceType.UpdatedBy = userId;
					serviceType.UpdateStamp = DateTime.UtcNow;
					serviceType.Status = StatusEnum.Active;
					_context.ServiceTypes.Update(serviceType);
                }
            }else
            {
				if(serviceType != null)
                {
					serviceType.UpdatedBy = userId;
					serviceType.UpdateStamp = DateTime.UtcNow;
					serviceType.Status = StatusEnum.Active;
					_context.ServiceTypes.Update(serviceType);
				}
            }
            if (string.IsNullOrEmpty(data.PrintUri))
            {
                data.PrintCost = 0;
                data.PrintUri = String.Empty;

			}
			if (string.IsNullOrEmpty(data.PrintUri))
				data.PrintUri = string.Empty;
			if (tmp == null)
			{
				
				data.CreatedBy = userId;
				data.CreateStamp = DateTime.UtcNow;
				_context.Resources.Add(data);
				
			}
			else
			{
				if (string.IsNullOrEmpty(data.Extension))
					data.Extension = tmp.Extension;
				_context.Entry(tmp).CurrentValues.SetValues(data);
				_context.Resources.Update(tmp);
			}
			await _context.SaveChangesAsync();
		}


        public async Task<Resource> UpdateProfile(Guid id, String profile, Guid userId)
        {
            var tmp = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Resource not found");
            tmp.Profile = profile;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Resources.Update(tmp);
            await _context.SaveChangesAsync();
            return tmp;
        }
        public async Task<Resource> UpdateLocalProfile(Guid id, String profile, Guid userId)
        {
            var tmp = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Resource not found");
            tmp.ProfileLocal = profile;
            tmp.UpdatedBy = userId;
            tmp.UpdateStamp = DateTime.UtcNow;
            _context.Resources.Update(tmp);
            await _context.SaveChangesAsync();
            return tmp;
        }
        public async Task Delete(Guid id)
        {
            var tmp = await _context.Resources.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Resource not found");
            _context.Resources.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}