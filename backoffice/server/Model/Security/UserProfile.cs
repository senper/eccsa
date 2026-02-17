using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Security
{

    public class UserProfile
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool Email_verified { get; set; }
        public string Name { get; set; }
        public int AssignedModules { get; set; }
        public int DefaultModule { get; set; }
        public Guid AssociationId { get; set; }
        public List<string> Roles { get; set; }
    }
}
