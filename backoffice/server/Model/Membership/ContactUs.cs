using System;

namespace ECCSA.BackOffice.Server.Model.Membership
{
    public class ContactUs
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Message { get; set; }
        public DateTime Date { get; set; }
    }
}
