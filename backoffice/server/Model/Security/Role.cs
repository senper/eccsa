

using ECCSA.BackOffice.Server.Model.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Security
{
    public class Role : IdentityRole<Guid>
    {
        [NotMapped]
        public RoleLevelEnum RoleLevel { get; set; }

        public ECCSAModuleFlag Module { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public static List<Role> Roles { get; private set; }

        public static List<Role> GetRoles(ECCSAModuleFlag module)
        {
            return Roles.FindAll(r => r.Module == module);
        }
        public Role()
        {

        }
        [NotMapped]
        public static Guid ArbitratorRoleId => Guid.Parse("00000000-0000-0000-0000-000000000018");
        static Role()
        {
            const ECCSAModuleFlag all =

                ECCSAModuleFlag.Academy |
                ECCSAModuleFlag.Administration |
                ECCSAModuleFlag.Arbitration |
                ECCSAModuleFlag.Communication |
                ECCSAModuleFlag.Investment |
                ECCSAModuleFlag.Membership |
                ECCSAModuleFlag.Research |
                ECCSAModuleFlag.MandE |
                ECCSAModuleFlag.TradePromotion;
            Roles = new List<Role>
            {
                new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "Academy Admin",
                    NormalizedName = "ACA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Academy
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Name = "Academy User",
                    NormalizedName = "ACU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Academy
                },
                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Name = "IT Admin",
                    NormalizedName = "ITA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Administration
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Name = "IT User",
                    NormalizedName = "ITU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Administration
                },
                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    Name = "Arbitration Admin",
                    NormalizedName = "ARA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Arbitration
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                    Name = "Arbitration User",
                    NormalizedName = "ARU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Arbitration
                },
                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                    Name = "Investment User",
                    NormalizedName = "INA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Investment
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                    Name = "Investment User",
                    NormalizedName = "INU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Investment
                },
                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                    Name = "Membership Admin",
                    NormalizedName = "MEA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Membership
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-00000000000a"),
                    Name = "Membership User",
                    NormalizedName = "MEU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Membership
                },
                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-00000000000b"),
                    Name = "Communication Admin",
                    NormalizedName = "PRA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Communication
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-00000000000c"),
                    Name = "Communication User",
                    NormalizedName = "PRU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Communication
                },
                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-00000000000d"),
                    Name = "Research Admin",
                    NormalizedName = "REA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Research
                },
                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-00000000000e"),
                    Name = "Research User",
                    NormalizedName = "REU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.Research
                },
                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-00000000000f"),
                    Name = "Trade Admin",
                    NormalizedName = "TRA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.TradePromotion
                },

                 new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                    Name = "Trade User",
                    NormalizedName = "TRU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = ECCSAModuleFlag.TradePromotion
                },

                  new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000011"),
                    Name = "Event Manager",
                    NormalizedName = "EVM",
                    RoleLevel = RoleLevelEnum.Event,
                    Module = 0
                },

                   new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000012"),
                    Name = "Survey Manager",
                    NormalizedName = "SRM",
                    RoleLevel = RoleLevelEnum.Survey,
                    Module = all
                },

                   new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000013"),
                    Name = "Finance",
                    NormalizedName = "FIN",
                    RoleLevel = RoleLevelEnum.Finance,
                    Module = all
                },
                    new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000014"),
                    Name = "MandE Manager",
                    NormalizedName = "MAEM",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.MandE
                },
                        new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000015"),
                    Name = "MandE User",
                    NormalizedName = "MAEU",
                    RoleLevel = RoleLevelEnum.User,
                    Module = all
                },
                    new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000099"),
                    Name = "Web Master",
                    NormalizedName = "WEM",
                    RoleLevel = RoleLevelEnum.Finance,
                    Module = 0
                },
                    new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000016"),
                    Name = "Support Admin",
                    NormalizedName = "SPA",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Support
                },
new Role
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000017"),
                    Name = "BMO",
                    NormalizedName = "BMO",
                    RoleLevel = RoleLevelEnum.Admin,
                    Module = ECCSAModuleFlag.Bmo
                },
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000018"),
    Name = "Arbitrator",
    NormalizedName = "ARBITR",
    RoleLevel = RoleLevelEnum.LimitedUser,
    Module = ECCSAModuleFlag.Arbitration
},
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000019"),
    Name = "Instructor",
    NormalizedName = "INSTR",
    RoleLevel = RoleLevelEnum.LimitedUser,
    Module = ECCSAModuleFlag.Academy
},



new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000020"),
    Name = "Support Viewer",
    NormalizedName = "SPV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.Support
},
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000021"),
    Name = "M&E Viewer",
    NormalizedName = "MEV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.MandE
},
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000022"),
    Name = "Arbitration Viewer",
    NormalizedName = "ARBV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.Arbitration
},
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000023"),
    Name = "Academy Viewer",
    NormalizedName = "CADV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.Academy
},

new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000024"),
    Name = "Communication Viewer",
    NormalizedName = "PRV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.Communication
},
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000025"),
    Name = "Investment Viewer",
    NormalizedName = "INVV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.Investment
},
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000026"),
    Name = "Membership Viewer",
    NormalizedName = "MEMV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.Membership
},

new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000027"),
    Name = "Research Viewer",
    NormalizedName = "RSRV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.Research
},
new Role
{
    Id = Guid.Parse("00000000-0000-0000-0000-000000000028"),
    Name = "TradePromotion Viewer",
    NormalizedName = "TPV",
    RoleLevel = RoleLevelEnum.View,
    Module = ECCSAModuleFlag.TradePromotion
},


            };
        }
    }

}