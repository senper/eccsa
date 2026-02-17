using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using ECCSA.BackOffice.Server.Model.Security;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Security
{
    public class UserContactRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public UserContactRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public List<UserContact> UserContacts(Guid userId)
        {
            return _context.UserContacts.Where(r => r.UserId == userId).Distinct().ToList();
            ;
        }
        public async Task<UserContact> UserContact(Guid userId, Guid id)
        {
            var request = await _context.UserContacts
                    .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            return request;
        }

        public async Task Save(Guid userId, UserContact data)
        {
            var tmp = await _context.UserContacts.FirstOrDefaultAsync(x => x.Id == data.Id);
            //data.UpdatedBy = userId;
            //data.UpdateStamp = DateTime.UtcNow;
            data.UserId = userId;
            if (data.Id == Guid.Empty || tmp == null)
            {
                data.Id = data.Id == Guid.Empty ? Guid.NewGuid() : data.Id;
                //data.CreatedBy = userId;
                //data.CreateStamp = DateTime.UtcNow;
                _context.UserContacts.Add(data);
            }
            else
            {
                _context.Entry(tmp).CurrentValues.SetValues(data);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid userId, Guid id)
        {
            var tmp = await _context.UserContacts.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id);
            if (tmp == null) return;
            _context.UserContacts.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}