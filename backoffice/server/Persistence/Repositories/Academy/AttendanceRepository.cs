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
    public class AttendanceRepository 
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public AttendanceRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public IQueryable<Attendance> Attendances(Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return _context.Attendances.Include(x => x.Training)
.Include(x => x.Registration)
;
        }

        public async Task<Attendance> AttendanceById(Guid id, Guid userId)
        {
            // var user = _userContext.Users.FirstOrDefault(u => u.Id == userId);
            return await _context.Attendances.Include(x => x.Training)
.Include(x => x.Registration)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<AttendancePayload> Create(GenericInput<CreateAttendanceInput> input)
        {
            if (_context.Attendances.Any(r => r.RegistrationId == input.PayLoad.RegistrationId &&
            r.AttendanceDate.Date == input.PayLoad.AttendanceDate.Date))
            {
                throw new Exception($"Attendance already added for the date {input.PayLoad.AttendanceDate.Date.ToString("yyyy-MM-dd")}.");
            }
            Attendance data = new()
            {
                			Id = Guid.NewGuid()
			, TrainingId = input.PayLoad.TrainingId
			, RegistrationId = input.PayLoad.RegistrationId
			, AttendanceDate = input.PayLoad.AttendanceDate
            , Morning = input.PayLoad.Morning
            , Afternoon = input.PayLoad.Afternoon
            , Evening = input.PayLoad.Evening

            };
            await _context.Attendances.AddAsync(data);
           var val = await _context.SaveChangesAsync();
           return new AttendancePayload(data);
        }

        public async Task<AttendancePayload> Update(GenericInput<UpdateAttendanceInput> data)
        {
            if (_context.Attendances.Any(r => r.Id != data.PayLoad.Id && r.RegistrationId == data.PayLoad.RegistrationId &&
            r.AttendanceDate.Date == data.PayLoad.AttendanceDate.Date))
            {
                throw new Exception($"Attendance already added for the date {data.PayLoad.AttendanceDate.Date.ToString("yyyy-MM-dd")}.");
            }
            var tmp = await  _context.Attendances.FirstOrDefaultAsync(r => r.Id == data.PayLoad.Id);
            if (tmp == null) throw new KeyNotFoundException("Attendance not found");
            						tmp.TrainingId = data.PayLoad.TrainingId;
			tmp.RegistrationId = data.PayLoad.RegistrationId;
			tmp.AttendanceDate = data.PayLoad.AttendanceDate;

            tmp.Morning = data.PayLoad.Morning;
            tmp.Afternoon = data.PayLoad.Afternoon;
            tmp.Evening = data.PayLoad.Evening;

            _context.Attendances.Update(tmp);
            await _context.SaveChangesAsync();
            return new AttendancePayload(tmp);

        }

        public async Task Delete(Attendance data)
        {
            var tmp = await _context.Attendances.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Attendance not found");
            _context.Attendances.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}