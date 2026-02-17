using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class FooterMenu
    {
        public int Id { get; set; }
        public List<FooterLink> UsefulLinks { get; set; }
        public List<FooterLink> ImportantLinks { get; set; }
        public FooterContact ContactUs { get; set; }
    }
    public class FooterLink
    {
        public string Title { get; set; }
        public string TitleLocal { get; set; }
        public string Path { get; set; }
    }
    public class FooterContact
    {
        public string Address { get; set; }
        public string AddressLocal { get; set; }
        public string Location { get; set; }
        public string LocationLocal { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
    }
}
