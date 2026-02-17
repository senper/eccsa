using System;

namespace ECCSA.BackOffice.Server.Model.PublicSecurity
{

    public class PublicUserProfile
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool Email_verified { get; set; }
        public string FullName { get; set; }
        public string PhoneNo { get; set; }
        public Guid MemberId { get; set; }
        public string TIN { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public bool BusinessAccount { get; set; }
        public string Logo { get; set; }
    }
}
