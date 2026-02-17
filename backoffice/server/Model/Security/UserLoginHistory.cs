using System;

namespace ECCSA.BackOffice.Server.Model.Security
{
    public class UserLoginHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string IPAddress { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
    }
}