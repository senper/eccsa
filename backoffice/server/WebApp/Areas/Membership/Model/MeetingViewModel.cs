using ECCSA.BackOffice.Server.Model.Membership;
using Microsoft.AspNetCore.Http;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class MeetingViewModel : MembershipViewModel<Meeting>
    {
        public IFormFile Attachment { get; set; }

        public Guid MeetingId { get; set; }
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public string Controller => "Meeting";

        public Participant TheParticipant { get; set; }
    }
    public class IdCard
    {
        public String Code { get; set; }
        public String FullName { get; set; }
        public String Email { get; set; }
        public String Mobile { get; set; }
        public String ExpiryDate { get; set; }
        public String AssociationName { get; set; }

    }
    public class Badge
    {
        public String No { get; set; }
        public String Code1 { get; set; }
        public String FullName1 { get; set; } 
        public String Title1 { get; set; }
        public String AssociationName1 { get; set; }

        public String Code2 { get; set; } = String.Empty;
        public String FullName2 { get; set; } = String.Empty;
        public String Title2 { get; set; } = String.Empty;
        public String AssociationName2 { get; set; } = String.Empty;

        public String Code3 { get; set; } = String.Empty;
        public String FullName3 { get; set; } = String.Empty;
        public String Title3 { get; set; } = String.Empty;
        public String AssociationName3 { get; set; } = String.Empty;

        public String Code4 { get; set; } = String.Empty;
        public String FullName4 { get; set; } = String.Empty;
        public String Title4 { get; set; } = String.Empty;
        public String AssociationName4 { get; set; } = String.Empty;
    }
}
