using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.UserAdministration
{
    public class UserAdministrationRepository
    {

        private readonly EccsaAdminContext _context;
        private readonly EccsaContext _dataContext;
        public UserAdministrationRepository(EccsaAdminContext context, EccsaContext dataContext)
        {
            _context = context;
            _dataContext = dataContext;
        }

        public async Task<bool> Launched()
        {
            var yesNo = await _dataContext.YesNo.FirstOrDefaultAsync();
            if (yesNo == null) return false;
            return true;
        }
        public async Task<bool> Launch(string code)
        {
            if (code.ToLowerInvariant() != "ecds") return false;
            var yesNo = await _dataContext.YesNo.FirstOrDefaultAsync();
            if (yesNo != null)
            {
                yesNo.Name = "Launched";
                yesNo.NameLocal = "Launched";
                _dataContext.YesNo.Update(yesNo);
            }
            else
            {
                _dataContext.YesNo.Add(new YesNo()
                {
                    Id = 1,
                    Name = "Launched",
                    NameLocal = "Launched"
                });
            }
            var result = await _dataContext.SaveChangesAsync();
            return result > 0;
        }
        public async Task<List<string>> GetUserRoles(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new List<string>();
            }
            var lst = new List<string>();
            var tmp = user.UserRoles.Select(x => new { x.Role.NormalizedName }).ToList();
            foreach (var r in tmp)
            {
                lst.Add(r.NormalizedName);
            }
            return lst;
        }
        public async Task<List<Role>> GetRoles()
        {
            var roles = await _context.Roles
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.User)
                .ToListAsync();



            return roles;
        }
        public async Task<Role> GetRole(Guid roleId)
        {
            var role = await _context.Roles
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.User)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            return role;
        }
        public async Task<List<User>> BackOfficeUsers()
        {


            var dbRoles = await _context.Roles.ToListAsync();
            var sysRoles = Role.Roles;
            if (dbRoles.Count != sysRoles.Count)
            {
                foreach (var sysRole in sysRoles)
                {
                    if (dbRoles.Any(r => r.Id == sysRole.Id)) continue;
                    sysRole.ConcurrencyStamp = Guid.NewGuid().ToString();
                    _context.Roles.Add(sysRole);

                }

                await _context.SaveChangesAsync();
            }
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .Where(u => u.AssignedModules > 0 && u.AssociationId == Constants.ECCSA).ToListAsync();

            return user;
        }
        public async Task<List<User>> MemberUsers()
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .Where(u => u.AssignedModules > 0 && u.AssociationId != Guid.Empty && u.AssociationId != Constants.ECCSA).ToListAsync();
            var associations = await _dataContext.Associations.ToListAsync();
            foreach(var user in users)
            {
                var association = associations.FirstOrDefault(a => a.Id == user.AssociationId);
                if (association != null)
                {
                    user.Association = association.Name;
                    user.AssociationParentId = association.ParentId;
                }
            }
            return users;
        }
        public async Task<List<User>> PublicUsers()
        {
            var user = await _context.Users
                .Where(u => u.AssignedModules == 0).ToListAsync();

            return user;
        }

        public async Task<List<Association>> Associations()
        {
            return await _dataContext.Associations
                //.Where(u => u.EnableWebAccess)
                .ToListAsync();
        }

        public async Task<User> BackOfficeUser(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public async Task<User> MemberUser(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var association = await _dataContext.Associations.FirstOrDefaultAsync(a => a.Id == user.AssociationId);
            user.Association = association?.Name;
            return user;
        }

        public async Task DisableUser(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            user.Disabled = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUser(Guid userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task EnableUser(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            user.Disabled = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> PublicUser(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
        public async Task<bool> IsUserInRole(User user, string roleKey)
        {
            return await _context.UserRoles
                .Include(x => x.Role)
                .AnyAsync(u => u.UserId == user.Id && u.Role.NormalizedName == roleKey);
        }
        public async Task UpdateUserModuleAccess(Guid userId, ECCSAModuleFlag assignedModules, ECCSAModuleFlag defaultModule)
        {
            var tmp = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (tmp == null)
            {
                return;
            }
            tmp.DefaultModule = defaultModule;
            tmp.AssignedModules = assignedModules;
            _context.Users.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserRole(Guid userId, Guid roleId)
        {
            var tmp = await _context.UserRoles
                .FirstOrDefaultAsync(u => u.UserId == userId && u.RoleId == roleId);
            if (tmp != null)
            {
                return;
            }

            _context.UserRoles.Add(new UserRole() { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
        }


        public async Task ChangeUserRole(Guid userId, List<Guid> roles, bool Members = false)
        {
            var userRoles = _context.UserRoles.Where(u => u.UserId == userId);
            _context.UserRoles.RemoveRange(userRoles);
            roles.ForEach(r =>
            _context.UserRoles.Add(new UserRole() { UserId = userId, RoleId = r }));
            if(Members)
            {
                var hasAdmin = _context.Roles.Any(r => r.Module == ECCSAModuleFlag.Administration && roles.Contains(r.Id));
                if (hasAdmin)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    if (!user.AssignedModules.HasFlag(ECCSAModuleFlag.Administration))
                    {
                        user.AssignedModules = (ECCSAModuleFlag)((int)user.AssignedModules + (int)ECCSAModuleFlag.Administration);
                    }
                }
                else
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    user.AssignedModules = ECCSAModuleFlag.Membership;
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task ChangeUserSubsystems(Guid userId, List<int> subsystems)
        {
            if (!subsystems.Any())
            {
                throw new InvalidOperationException("Must select atleast one subsytem!");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId); ;
            user.AssignedModules = (ECCSAModuleFlag)subsystems.Sum();
            if (!user.AssignedModules.HasFlag(user.DefaultModule) ||
                user.DefaultModule == 0)
            {
                user.DefaultModule = subsystems.Any() ? (ECCSAModuleFlag)subsystems.FirstOrDefault() : 0;
            }
            await _context.SaveChangesAsync();
        }
        public async Task RemoveUserSubsystem(Guid userId, int subsystem)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.AssignedModules -= subsystem;
            if (user.AssignedModules <= 0)
            {
                throw new InvalidOperationException("User Must has atleast one subsytem!");
            }
            if (!user.AssignedModules.HasFlag(user.DefaultModule))
            {
                user.DefaultModule =
                Enum.GetValues<ECCSAModuleFlag>().Any(s => user.AssignedModules.HasFlag(s)) ?
                    Enum.GetValues<ECCSAModuleFlag>().FirstOrDefault(s => user.AssignedModules.HasFlag(s)) : 0;
            }
            await _context.SaveChangesAsync();
        }
        public async Task SetDefaultSubsystem(Guid userId, int subsystem)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.DefaultModule = (ECCSAModuleFlag)subsystem;
            await _context.SaveChangesAsync();
        }
        public async Task RenameUserName(Guid userId, string userName)
        {
            var tmp = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (tmp != null)
            {
                return;
            }
            tmp.UserName = userName;
            tmp.NormalizedUserName = userName.ToUpperInvariant();
            _context.Users.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProfile(User user)
        {
            var tmp = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (tmp == null)
            {
                return;
            }
            if (tmp.MobileNo != user.MobileNo && tmp.Email != user.Email)
            {
                throw new InvalidOperationException("Cannot change mobile and email at the same time");
            }
            tmp.UserName = user.UserName;
            tmp.NormalizedUserName = user.UserName.ToUpperInvariant();
            tmp.Email = user.Email;
            tmp.NormalizedEmail = user.Email.ToUpperInvariant();
            tmp.MobileNo = user.MobileNo;
            tmp.PhoneNumber = user.MobileNo;
            tmp.FullName = user.FullName;
            tmp.Title = user.Title;
            tmp.DepartmentId = user.DepartmentId;
            _context.Users.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveUserRole(Guid userId, Guid roleId)
        {
            var tmp = await _context.UserRoles
                .FirstOrDefaultAsync(u => u.UserId == userId && u.RoleId == roleId);
            if (tmp == null)
            {
                return;
            }

            _context.UserRoles.Remove(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUser(User user)
        {
            var tmp = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (tmp == null)
            {
                return;
            }
            tmp.FullName = user.FullName;
            tmp.DefaultModule = user.DefaultModule;
            _context.Users.Update(tmp);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserLoginStatus(User user)
        {
            var tmp = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (tmp == null)
            {
                return;
            }
            tmp.Disabled = user.Disabled;
            tmp.PasswordTryCount = user.PasswordTryCount;
            tmp.PasswordDisabledTime = user.PasswordDisabledTime;
            _context.Users.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMemberUser(User user)
        {
            var tmp = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            if (tmp == null)
            {
                return;
            }
            tmp.FullName = user.FullName;
            
            _context.Users.Update(tmp);
            await _context.SaveChangesAsync();
        }
        public async Task SaveUserHistory(UserLoginHistory userLoginHistory)
        {
            _context.UserLoginHistories.Add(userLoginHistory);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserLoginHistory>> UserLoginHistory(Guid userId)
        {
           return await _context.UserLoginHistories.Where(s => s.UserId == userId).OrderByDescending(s => s.Time).ToListAsync();
        }
    }
}
