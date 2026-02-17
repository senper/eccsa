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
    public class SponsorRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public SponsorRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Sponsor> Sponsors(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Sponsors.Include(x => x.TrainingSponsors)
;
        }

        public async Task<Sponsor> SponsorById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Sponsors.Include(x => x.TrainingSponsors)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<SponsorPayload> Create(GenericInput<CreateSponsorInput> input)
        {
            Sponsor data = new()
            {
                			Id = Guid.NewGuid()
			, NameEng = input.PayLoad.NameEng
			, NameLocal = input.PayLoad.NameLocal ?? ""
            , Email = input.PayLoad.Email
            , Mobile = input.PayLoad.Mobile
			, Status = input.PayLoad.Status

            };
            await _context.Sponsors.AddAsync(data);
            _context.LookupValues.Add(new Model.Common.LookupValue()
            {
                Id = data.Id,
                Name = data.NameEng,
                Status = Model.Common.StatusEnum.Active,
                Value = data.Id.ToString(),
                LookupTypeId = Constants.SponsorLookupTypeId

            });
           var val = await _context.SaveChangesAsync();
           return new SponsorPayload(data);
        }

        public async Task<SponsorPayload> Update(GenericInput<UpdateSponsorInput> data)
        {
            var tmp = await  _context.Sponsors.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Sponsor not found");
            						tmp.NameEng = data.PayLoad.NameEng;
            tmp.NameLocal = data.PayLoad.NameLocal ?? "";
            tmp.Email = data.PayLoad.Email;
            tmp.Mobile = data.PayLoad.Mobile;
            tmp.Status = data.PayLoad.Status;

            _context.Sponsors.Update(tmp);
            var lv = await _context.LookupValues.FirstOrDefaultAsync(l => l.Id == tmp.Id);
            if(lv != null)
            {
                lv.Name = tmp.NameEng;
                lv.Status = (Model.Common.StatusEnum)tmp.Status;
                _context.LookupValues.Update(lv);
            }else
            {
                _context.LookupValues.Add(new Model.Common.LookupValue()
                {
                    Id = tmp.Id,
                    Name = tmp.NameEng,
                    Status = (Model.Common.StatusEnum)tmp.Status,
                    LookupTypeId = Constants.SponsorLookupTypeId,
                    Code = string.Empty,
                    Value = tmp.Id.ToString(),

                });
            }
            await _context.SaveChangesAsync();
            return new SponsorPayload(tmp);

        }

        public async Task Delete(Sponsor data)
        {
            var tmp = await _context.Sponsors.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Sponsor not found");
            _context.Sponsors.Remove(tmp);
            var lv = await _context.LookupValues.FirstOrDefaultAsync(l => l.Id == data.Id);
           
            if(lv != null)
            {
                _context.LookupValues.Remove(lv);
            }
            await _context.SaveChangesAsync();
        }
    }
}