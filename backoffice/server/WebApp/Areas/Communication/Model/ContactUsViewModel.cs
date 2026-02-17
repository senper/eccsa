using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model
{
    public class ContactUsViewModel : CommunicationViewModel<ContactUs>
    {
        public string Controller => "Contact Us";
        public DateTime Date { get; set; }
    }
}
