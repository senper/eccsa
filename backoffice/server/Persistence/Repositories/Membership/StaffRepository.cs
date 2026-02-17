using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.DTO;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Membership;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class StaffRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public StaffRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Staff> Employees(Guid associationId, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Employees.Include(x => x.Association)
.Include(x => x.PositionType).Where(s => s.AssociationId == associationId);
            ;
        }

        public async Task<Staff> StaffById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Employees.Include(x => x.Association)
.Include(x => x.PositionType)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<StaffPayload> Create(GenericInput<CreateStaffInput> input)
        {
            if (!Validation.IsEmail(input.PayLoad.Email) || !Validation.IsPhoneNumber(input.PayLoad.Mobile))
            {
                throw new InvalidOperationException("Validation error");
            }
            Staff data = new()
            {
                Id = Guid.NewGuid()
            ,
                Title = input.PayLoad.Title
            ,
                NameEng = input.PayLoad.NameEng
            ,
                NameLocal = input.PayLoad.NameLocal
            ,
                PositionTypeId = input.PayLoad.PositionTypeId
            ,
                AssociationId = input.PayLoad.AssociationId
            ,
                StartDate = input.PayLoad.StartDate
            ,
                EndDate = input.PayLoad.EndDate
            ,
                Email = input.PayLoad.Email
            ,
                Mobile = input.PayLoad.Mobile
            ,
                Status = input.PayLoad.Status
            ,
                Photo = input.PayLoad.Photo
            ,
                Extension = input.PayLoad.Extension
            };
            await _context.Employees.AddAsync(data);
            var val = await _context.SaveChangesAsync();
            return new StaffPayload(data);
        }

        public async Task<StaffPayload> Update(GenericInput<UpdateStaffInput> data)
        {
            var tmp = await _context.Employees.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            if (!Validation.IsEmail(data.PayLoad.Email) || !Validation.IsPhoneNumber(data.PayLoad.Mobile))
            {
                throw new InvalidOperationException("Validation error");
            }
            tmp.Title = data.PayLoad.Title;
            tmp.NameEng = data.PayLoad.NameEng;
            tmp.NameLocal = data.PayLoad.NameLocal;
            tmp.PositionTypeId = data.PayLoad.PositionTypeId;
            tmp.AssociationId = data.PayLoad.AssociationId;
            tmp.StartDate = data.PayLoad.StartDate;
            tmp.EndDate = data.PayLoad.EndDate;
            tmp.Status = data.PayLoad.Status;
            tmp.Email = data.PayLoad.Email;
            tmp.Mobile = data.PayLoad.Mobile;
            tmp.Photo = data.PayLoad.Photo;
            tmp.Extension = data.PayLoad.Extension;
            _context.Employees.Update(tmp);
            await _context.SaveChangesAsync();
            return new StaffPayload(tmp);

        }

        public async Task Delete(Guid id)
        {
            var tmp = await _context.Employees.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Employees.Remove(tmp);
            await _context.SaveChangesAsync();
        }


        public List<IndividualStaff> IndividualEmployees(Guid individualId, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var list = _context.IndividualEmployees.Include(x => x.Individual)
                .Where(s => s.IndividualId == individualId).ToList();
            return list;
        }

        public async Task<IndividualStaff> IndividualStaffById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            var staff = await _context.IndividualEmployees.Include(x => x.Individual)
.FirstOrDefaultAsync(i => i.Id == id);
            if (staff != null)
            {
                staff.PositionType = await _context.PositionTypes.FirstOrDefaultAsync(p => p.Id == staff.PositionTypeId);
            }
            return staff;
        }
        public async Task<IndividualStaffPayload> CreateIndividualStaff(GenericInput<CreateIndividualStaffInput> input)
        {
            if (!Validation.IsEmail(input.PayLoad.Email) || !Validation.IsPhoneNumber(input.PayLoad.Mobile))
            {
                throw new InvalidOperationException("Validation error");
            }
            IndividualStaff data = new()
            {
                Id = Guid.NewGuid()
            ,
                Title = input.PayLoad.Title
            ,
                NameEng = input.PayLoad.NameEng
            ,
                NameLocal = input.PayLoad.NameLocal
            ,
                PositionTypeId = input.PayLoad.PositionTypeId
            ,
                IndividualId = input.PayLoad.IndividualId
            ,
                StartDate = input.PayLoad.StartDate
            ,
                EndDate = input.PayLoad.EndDate
            ,
                Email = input.PayLoad.Email
            ,
                Mobile = input.PayLoad.Mobile
            ,
                Status = input.PayLoad.Status
            ,
                Photo = input.PayLoad.Photo
            ,
                Extension = input.PayLoad.Extension
            };
            await _context.IndividualEmployees.AddAsync(data);
            var val = await _context.SaveChangesAsync();
            return new IndividualStaffPayload(data);
        }

        public async Task<IndividualStaffPayload> UpdateIndividualStaff(GenericInput<UpdateIndividualStaffInput> data)
        {
            var tmp = await _context.IndividualEmployees.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            if (!Validation.IsEmail(data.PayLoad.Email) || !Validation.IsPhoneNumber(data.PayLoad.Mobile))
            {
                throw new InvalidOperationException("Validation error");
            }
            tmp.Title = data.PayLoad.Title;
            tmp.NameEng = data.PayLoad.NameEng;
            tmp.NameLocal = data.PayLoad.NameLocal;
            tmp.PositionTypeId = data.PayLoad.PositionTypeId;
            tmp.IndividualId = data.PayLoad.IndividualId;
            tmp.StartDate = data.PayLoad.StartDate;
            tmp.EndDate = data.PayLoad.EndDate;
            tmp.Status = data.PayLoad.Status;
            tmp.Email = data.PayLoad.Email;
            tmp.Mobile = data.PayLoad.Mobile;
            tmp.Photo = data.PayLoad.Photo;
            tmp.Extension = data.PayLoad.Extension;
            _context.IndividualEmployees.Update(tmp);
            await _context.SaveChangesAsync();
            return new IndividualStaffPayload(tmp);

        }

        public async Task DeleteIndividualStaff(Guid id)
        {
            var tmp = await _context.IndividualEmployees.FirstOrDefaultAsync(r => r.Id == id);
            if (tmp == null) throw new KeyNotFoundException("Staf not found");
            _context.IndividualEmployees.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}