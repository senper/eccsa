using System;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class Partner 
    {
        public Guid Id { get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public String CompanyName { get; set; }

        public String PhoneNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
