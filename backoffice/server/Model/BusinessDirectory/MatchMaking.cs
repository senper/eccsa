using ECCSA.BackOffice.Server.Model.Common;
using System;

namespace ECCSA.BackOffice.Server.Model.BusinessDirectory
{
    public class MatchMaking
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public BusinessInterestSource Source { get; set; }
        public Guid UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Remark { get; set; }
        public StatusEnum Status { get; set; }
    }
}
