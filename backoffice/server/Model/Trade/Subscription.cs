using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Trade
{
    public class Subscription : INotificationAddress
    {
        public Guid Id { get; set; }    
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
        public bool Status { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime? UnsubscriptionDate { get; set; }
        public virtual ICollection<SubscriptionDissimination> Dissiminations { get; set; }

        [NotMapped]
        public String UserName { get; set; }

        [NotMapped]
        public String FullName { get; set; }

        [NotMapped]
        public String Email { get; set; }

        [NotMapped]
        public String CompanyName { get; set; }
        [NotMapped]
        public string MobileNo { get; set; } = "";

        [NotMapped]
        public bool IsStaff { get; set; } = false;
    }

    public class NewsletterIssue : Entity
    {
        
        public Guid TopicId { get; set; }
        public String Title { get; set; }

        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";

        public String Brief { get; set; }

        public String BriefLocal { get; set; }
        public DateTime IssueDate { get; set; }
        public string Extension { get; set; }

        public DissiminationStatusEnum DissiminationStatus { get; set; } = DissiminationStatusEnum.Draft;
        public virtual ICollection<SubscriptionDissimination> Dissiminations { get; set; }

        [NotMapped]
        public int SubscribersCount { get; set; }
        [NotMapped]
        public List<Subscription> Subscribers { get; set; } = new List<Subscription>();

        public string GetStatus(Subscription subscriber)
        {
            return Dissiminations.Any(x => x.SubscriberId == subscriber.Id)? "Sent":"Not Sent";
        }

       
    }
    public class SubscriptionDissimination
    {
        public Guid Id { get; set; }
        public Guid SubscriberId { get; set; }
        public Guid NewsletterIssueId { get; set; }

        public DateTime DisseminationDate { get; set; }
        public Subscription Subscription { get; set; }
        public NewsletterIssue Issue { get; set; }
    }
}
