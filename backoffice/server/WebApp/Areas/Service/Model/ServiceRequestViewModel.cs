using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Servicing;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.Model.Security;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class AppointmentViewModel : ServiceViewModel<ServiceFollowup>
    {
        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today.AddDays(1);
    }
    public class ServiceRequestViewModel : ServiceViewModel<ServiceRequest>
    {
        public string SearchKey { get; set; }
        public int? SearchStatus { get; set; }
        public ECCSAModuleFlag Module
        {
            get => CurrentModule;
            set => CurrentModule = value;
        }
        public List<ServiceType> ServiceTypes { get; set; }

        public List<SimpleLookup> NextStatus { get; set; } = new List<SimpleLookup>();

        public List<User> Users { get; set; } = new List<User>();
        public List<User> OpponentUsers { get; set; } = new List<User>();

        public bool IsCurrentOperator { get; set; }

        public Guid ServiceTypeId { get; set; } = Guid.Empty;
            
    }
}
