using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Context;
using ECCSA.BackOffice.Server.Persistence.DTO.Security;
using Microsoft.EntityFrameworkCore;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Security
{
    public class PublicUserRepository
    {
        private readonly EccsaAdminContext _context;
        private readonly EccsaContext _dataContext;
        public PublicUserRepository(EccsaAdminContext context, EccsaContext dataContext)
        {
            _context = context;
            _dataContext = dataContext;
        }
        public async Task<List<User>> PublicUsers()
        {
            var user = await _context.Users
                .Where(u => u.AssignedModules == 0 && u.BusinessAccount).OrderBy(s => s.FullName).ToListAsync();



           
            return user;
        }
        public async Task<User> GetPublicUser(string emailCode, string mobileCode)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.AssignedModules == 0 && u.BusinessAccount
                && u.EmailConfirmationCode == emailCode /*&& u.MobileConfirmationCode == mobileCode*/);

            return user;
        }
        public async Task<User> GetPublicUser(string passwordResetCode)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.AssignedModules == 0 && u.BusinessAccount
                && u.PasswordResetCode == passwordResetCode);

            return user;
        }
        public async Task<List<string>> GetUserRoles(Guid userId)
        {
            var user = await  _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u=> u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new List<string>();
            }
            var lst = new List<string>();
            var tmp = user.UserRoles.Select(x=> new { x.Role.NormalizedName  }).ToList();
            foreach(var r in tmp)
            {
                lst.Add(r.NormalizedName);
            }
            return lst;
        }

        public async Task<bool> IsUserInRole(User user, string roleKey)
        {
            return await _context.UserRoles
                .Include(x => x.Role)
                .AnyAsync(u => u.UserId == user.Id && u.Role.NormalizedName == roleKey);
        }

        public async Task<List<Plan>> Plans(ECCSAModuleEnum subSystem)
        {
            var setting = await _dataContext.PlanConfigurations.FirstOrDefaultAsync();
            return await _dataContext.Plans
                .Where(p => p.PlanYear == setting.CurrentYear && p.ResponsibleUnit == subSystem)
                .OrderBy(p=> p.Code).ThenBy(p=> p.Name).ToListAsync();
        }
        public async Task<Model.Common.Planning.Configuration> Configuration()
        {
            return await _dataContext.PlanConfigurations.FirstOrDefaultAsync();
        }
        public async Task<List<FinancialPlan>> FinancialPlans()
        {
            var setting = await _dataContext.PlanConfigurations.FirstOrDefaultAsync();
            return await _dataContext.FinancialPlans.Where(p => p.PlanYear == setting.CurrentYear ).ToListAsync();
        }
    }
    public class UserRepository
    {
        private readonly EccsaUserContext _context;
        public UserRepository(EccsaUserContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users()
        {

            return _context.Users.Include(x => x.UserRoles)
.Include(x => x.UserClaims)
.Include(x => x.UserLogins)
;
        }

        public async Task<User> UserById(Guid id)
        {
            return await _context.Users.Include(x => x.UserRoles)
.Include(x => x.UserClaims)
.Include(x => x.UserLogins)
.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<UserPayload> Create(CreateUserInput input)
        {
            User data = new()
            {
                Id = Guid.NewGuid()
            ,
                UserName = input.UserName
            ,
                Email = input.Email
            ,
                PasswordHash = input.PasswordHash
            ,
                SecurityStamp = input.SecurityStamp
            ,
                PhoneNumber = input.PhoneNumber
            ,
                EmailConfirmed = input.EmailConfirmed
            ,
                PhoneNumberConfirmed = input.PhoneNumberConfirmed
            ,
                TwoFactorEnabled = input.TwoFactorEnabled
            ,
                LockoutEnabled = input.LockoutEnabled
            ,
                AccessFailedCount = input.AccessFailedCount
            ,
                LockoutEndDateUtc = input.LockoutEndDateUtc

            };
            await _context.Users.AddAsync(data);
            await _context.SaveChangesAsync();
            return new UserPayload(data);
        }

        public async Task<UserPayload> Update(UpdateUserInput data)
        {
            var tmp = await _context.Users.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            tmp.UserName = data.UserName;
            tmp.Email = data.Email;
            tmp.PasswordHash = data.PasswordHash;
            tmp.SecurityStamp = data.SecurityStamp;
            tmp.PhoneNumber = data.PhoneNumber;
            tmp.EmailConfirmed = data.EmailConfirmed;
            tmp.PhoneNumberConfirmed = data.PhoneNumberConfirmed;
            tmp.TwoFactorEnabled = data.TwoFactorEnabled;
            tmp.LockoutEnabled = data.LockoutEnabled;
            tmp.AccessFailedCount = data.AccessFailedCount;
            tmp.LockoutEndDateUtc = data.LockoutEndDateUtc;

            _context.Users.Update(tmp);
            await _context.SaveChangesAsync();
            return new UserPayload(tmp);

        }

        public async void Delete(User data)
        {
            var tmp = await _context.Users.FirstOrDefaultAsync(r => r.Id == data.Id);
            if (tmp == null) throw new KeyNotFoundException("Exchange Rate not found");
            _context.Users.Remove(tmp);
            await _context.SaveChangesAsync();
        }
    }
}