using ECCSA.BackOffice.Server.Model.Membership;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Service.Model
{
    public class ReviewResponseModel
    {
        public Guid Id { get; set; }
        public Guid ResponseId { get; set; }

        public DocumentReviewStatusEnum Status { get; set; }

        public String Remark { get; set; }
    }
}
