using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Payment;
using ECCSA.BackOffice.Server.Model.Security;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Membership.Model
{
    public class ApplicantViewModel : MembershipViewModel<Applicant>
    {

        public List<Association> Associations { get; set; } = new List<Association>();

        public Guid AssociationId { get; set; }

        public Guid LocationId { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();


        public List<ApplicantDocument> Documents = new List<ApplicantDocument>();

        public ApplicantDocument Document { get; set; } = new ApplicantDocument();


        public List<LegalDocumentType> DocumentTypes { get; set; } = new List<LegalDocumentType>();

        public RenewalHistory RenewalHistory { get; set; }
        public string AssociationJson { get; internal set; }

        public Invoice Invoice { get; set; }

        public Settlment Payment { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public bool IsCurrentOperator { get; set; }
    }
}
