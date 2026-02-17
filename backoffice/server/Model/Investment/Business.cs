using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Investment
{
    public class Business : Entity
    {
        [NotMapped]
        public bool Selected { get; set; }
        public String Name { get; set; }

        public String Sector { get; set; }

        public String Profile { get; set; }

        public String Country { get; set; }

        public String Address { get; set; }

        public String Email { get; set; }

        public String Telephone { get; set; }

        public String TINo { get; set; }
        public String ContactPerson { get; set; }

        public String Products { get; set; }

        public int YearOfEstablishment { get; set; }

        public int WorkingCapital { get; set; }

        public string ConfirmationLink { get; set; } = " ";

        public DateTime LinkExpiryDate { get; set; }

        public bool EmailConfirmed { get; set; }

        public string SMSCode { get; set; }

        public bool PhoneConfirmed { get; set; }
    }

    
}
