using ECCSA.BackOffice.Server.Persistence.Context;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Membership
{
    public class RegionalProfileRepository
    {
        private readonly EccsaContext _context;
        private readonly EccsaUserContext _userContext;
        public RegionalProfileRepository(EccsaContext context, EccsaUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        
    }
}