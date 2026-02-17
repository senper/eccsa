using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Common;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Common
{
    public class YesNoRepository 
    {
        private readonly EccsaContext _context;

        public YesNoRepository(EccsaContext context)
        {
            _context = context;
        }

        public IQueryable<YesNo> YesNo()
        {

            return _context.YesNo;
        }
        public async Task<GeneralSetting> GetSettingByKey(string key)
        {
            return await _context.GeneralSettings.FirstOrDefaultAsync(i => i.Key == key);
        }
        public async Task<List<GeneralSetting>> GetSettingByKey(List<string> keys)
        {
            return await _context.GeneralSettings.Where(i => keys.Any(k => k == i.Key)).ToListAsync();
        }
        public async Task<YesNo> YesNoById(int id)
        {
            return await _context.YesNo.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<YesNoPayload> Create(CreateYesNoInput input)
        {
            YesNo data = new()
            {
                						 Name = input.Name
			, NameLocal = input.NameLocal

            };
            await _context.YesNo.AddAsync(data);
           await _context.SaveChangesAsync();
           return new YesNoPayload(data);
        }

        public async Task<YesNoPayload> Update(UpdateYesNoInput data)
        {
            var tmp = await  _context.YesNo.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            						tmp.Name = data.Name;
			tmp.NameLocal = data.NameLocal;

            _context.YesNo.Update(tmp);
            await _context.SaveChangesAsync();
            return new YesNoPayload(tmp);

        }

        public async void Delete(YesNo data)
        {
            var tmp = await _context.YesNo.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.YesNo.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task<List<IssueLocation>> IssueLocations(string country)
        {
            var lst = await _context.IssueLocations.Where(c=> c.CountryRegion == country).ToListAsync();
            var locationTypes = await IssueLocationTypes();
            if (lst == null)
                lst = new List<IssueLocation>();
            foreach(var l in lst)
            {
                if (!l.TypeOfLocationId.HasValue) continue;
                var lType = locationTypes.FirstOrDefault(x => x.Id == l.TypeOfLocationId.Value);
                if (lType == null) continue;
                l.LocationType = lType.Name;
            }
            return lst;
        }
        public async Task<List<LookupValue>> IssueLocationTypes()
        {
            var lookupTypeId = Guid.Parse("00000000-0000-0000-0000-00000000002d");
            return await _context.LookupValues.Where(l => l.LookupTypeId == lookupTypeId).ToListAsync();

        }

        public async Task<List<IssueType>> IssueTypes()
        {
           
            return await _context.IssueTypes.Include(i=> i.Issues).ToListAsync();

        }
        public async Task<IssueType> IssueType(Guid id)
        {

            return await _context.IssueTypes.Include(i => i.Issues).FirstOrDefaultAsync(i=> i.Id == id);

        }
        public async Task Save(IssueType data)
        {
            var tmp = await _context.IssueTypes.FirstOrDefaultAsync(i => i.Id == data.Id);
            if(tmp == null)
            {
                if (data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                _context.IssueTypes.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteIssueType(Guid id)
        {
            var tmp = await _context.IssueTypes.Include(i => i.Issues).FirstOrDefaultAsync(i => i.Id == id);
            if (tmp == null) return;
            if(tmp.Issues.Count > 0)
                throw new InvalidOperationException("There are issues logged for this type");
            _context.IssueTypes.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task Save(IssueLocation data)
        {
            var tmp = await _context.IssueLocations.Include(i=> i.Issues).FirstOrDefaultAsync(i => i.Id == data.Id);
            if(tmp == null)
            {
                if(data.Id == Guid.Empty)
                    data.Id = Guid.NewGuid();
                _context.IssueLocations.Add(data);
            }else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteIssueLocation(Guid id)
        {
            var tmp = await _context.IssueLocations.FirstOrDefaultAsync(i => i.Id == id);
            if (tmp == null) return;
            var issue = await _context.Issues.AnyAsync(i => i.LocationId == id);
            if (issue)
                throw new InvalidOperationException("There are issues logged for this location");
            _context.IssueLocations.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        ///
        public async Task<List<LookupType>> LookupTypes()
        {
           // await GenerateEnumLookup();
            return await _context.LookupTypes.ToListAsync();
        }

        public async Task<LookupType> LookupTypeById(Guid id)
        {
            var lst= await _context.LookupTypes.Include(s => s.LookupValues).FirstOrDefaultAsync(i => i.Id == id);
            foreach(var lv in lst.LookupValues)
            {
                if (!lv.ParentId.HasValue) continue;
                lv.ParentName = lst.LookupValues.FirstOrDefault(l => l.Id == lv.ParentId.Value).Name;
            }
            return lst;
        }
        public async Task Delete(Guid id)
        {
            var tmp = await _context.LookupTypes.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Lookup Type  not found");
            if (_context.ServiceRequirements.Any(s => s.InputMethod == ServiceInputTypeEnum.SimpleLookup &&
             s.LookupTypeId == id))
            {
                tmp.Status = StatusEnum.Inactive;
                await _context.SaveChangesAsync();
                throw new InvalidOperationException("Lookup Type is already in use so it is set inactive.");
            }
            else
            {
                _context.LookupTypes.Remove(tmp);
                await _context.SaveChangesAsync();
            }
        }
        public async Task GenerateEnumLookup()
        {
            Type type = typeof(Constants);
            var fields = type.GetFields();
            foreach (var p in fields)
            {
                if (!p.Name.EndsWith("LookupTypeId")) continue;
                
                var v = p.GetValue( null); // static classes cannot be instanced, so use null...
                var enumName = p.Name.Replace("LookupTypeId", "");
                var enumType = GetEnumType(enumName);// "ECCSA.BackOffice.Server.Model.Common";
                if (enumType == null) continue;
                var values = Enum.GetValues(enumType);
                var lookupTypeId = Guid.Parse(v.ToString());
                var luType = await _context.LookupTypes.FirstOrDefaultAsync(l => l.Id == lookupTypeId);
                if(luType == null)
                {
                    _context.LookupTypes.Add(new LookupType {
                    Id = lookupTypeId,
                    Name = enumName.Replace("Enum",""),
                    Owner = (ECCSAModuleFlag)1024,
                    Status = StatusEnum.Active,
                    IsHierarchical = false,
                    SourceType = LookupSourceTypeEnum.Enum
                    });
                }
                var luValues = await _context.LookupValues
                    .Where(l => l.LookupTypeId == lookupTypeId)
                    .ToListAsync();
                if (luValues != null && luValues.Count > 0)
                {
                    _context.LookupValues.RemoveRange(luValues);
                    await _context.SaveChangesAsync();
                }
                foreach (var value in values)
                {
                    var text = Enum.GetName(enumType, value);
                    var key = (int)value;
                    var luId = GetEnumLookupValueId(key, lookupTypeId);
                    _context.LookupValues.Add(new LookupValue
                    {
                        Id = luId,
                        LookupTypeId = lookupTypeId,
                        Value = key.ToString(),
                        Name = text,
                        Status = StatusEnum.Active,
                        Code = string.Empty
                    }) ;
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
            }
        }
        private static Guid GetEnumLookupValueId(int id, Guid lookupTypeId)
        {
            var sv = lookupTypeId.ToString();
            var idPart = sv.Substring(sv.LastIndexOf("-")+1);
            var otherPart = sv.Substring(0, sv.LastIndexOf("-")+1);
            var tmp = idPart.Substring(3);
            tmp = id.ToString() + tmp;
            tmp = $"000{tmp}";
            tmp = tmp.Substring(tmp.Length - idPart.Length);
            var sguid = $"{otherPart}{tmp}";
            return Guid.Parse(sguid);
        }
        private static Type GetEnumType(string enumName)
        {
            
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.FullName.StartsWith("ECCSA.BackOffice.Server.Model")) continue;
                var namespaceName = "ECCSA.BackOffice.Server.Model.Common";
                var  fullName = $"{namespaceName}.{enumName}";
                var type = assembly.GetType(fullName);
                if (type != null && type.IsEnum) return type;
                namespaceName = "ECCSA.BackOffice.Server.Model.Membership";
                fullName = $"{namespaceName}.{enumName}";
                
                type = assembly.GetType(fullName);
                if (type == null)
                    continue;
                if (type.IsEnum)
                    return type;
            }
            return null;
        }
        public async Task Save(LookupType data)
        {
            var tmp = await _context.LookupTypes.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                _context.LookupTypes.Add(data);
            }
            else
            {
                if (tmp.IsHierarchical)
                {
                    throw new InvalidOperationException("Cannot change external lookup type");
                }
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLookupValue(Guid id)
        {
            var tmp = await _context.LookupValues.Include(x=> x.LookupType).FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Lookup Value  not found");
            if (tmp.LookupType.IsHierarchical)
            {
                throw new InvalidOperationException("External data cannot be deleted from here.");
            }
            if (_context.ServiceRequirements.Any(s => s.InputMethod == ServiceInputTypeEnum.SimpleLookup &&
             s.LookupTypeId == tmp.LookupTypeId && s.Responses.Any(r => r.LookupValueId == id)))
            {
                tmp.Status = StatusEnum.Inactive;
                await _context.SaveChangesAsync();
                throw new InvalidOperationException("Lookup value is already in use so it is set inactive.");
            }
            else
            {
                _context.LookupValues.Remove(tmp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Save(LookupValue data)
        {
            if (string.IsNullOrEmpty(data.Value))
                data.Value = data.Id.ToString();
            var tmp = await _context.LookupValues.FirstOrDefaultAsync(x => x.Id == data.Id);
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
               
                _context.LookupValues.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }
        ///
    }
}