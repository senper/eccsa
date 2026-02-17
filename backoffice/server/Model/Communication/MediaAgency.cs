using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace ECCSA.BackOffice.Server.Model.Communication
{
    public class MediaAgency : Entity, INotificationAddress
    {
        public String Name { get; set; }
        [NotMapped]
        public string FullName { get { return Name; } set { Name = value; } }
        public MediaTypeFlag MediaType { get; set; }
        public int IsLocalAgency { get; set; }
        public MediaAgencyOwnershipType OwnershipType { get; set; }
        public String RepresentativeName { get; set; }
        public String ResponsiblePersonnel { get; set; }
        public String Email { get; set; }
        public String Mobile { get; set; }
        [NotMapped]
        public string MobileNo { get { return Mobile; } set { Mobile = value; } }
        public String Telephone { get; set; }
        public String Url { get; set; }
        public String Description { get; set; }
        public String Remark { get; set; }
        public StatusEnum Status { get; set; }

        //public void MediaType2Bool()
        //{
        //    BroadcastMedia = MediaType.HasFlag(MediaTypeFlag.Broadcast);
        //    PrintMedia = MediaType.HasFlag(MediaTypeFlag.Print);
        //    DigitalMedia = MediaType.HasFlag(MediaTypeFlag.Digital);
        //}
        //public void MediaTypeFromBool()
        //{
        //    MediaType = MediaTypeFlag.NotSet;
        //    MediaType ^= BroadcastMedia ? MediaTypeFlag.Broadcast : MediaType;
        //    MediaType ^= PrintMedia ? MediaTypeFlag.Print : MediaType;
        //    MediaType ^= DigitalMedia ? MediaTypeFlag.Digital : MediaType;
        //}
        [NotMapped]
        public bool BroadcastMedia
        {
            get
            {
                if (MediaType == MediaTypeFlag.NotSet) return false;
                return MediaType.HasFlag(MediaTypeFlag.Broadcast);
            }
            set
            {
                if (value)
                {
                    MediaType ^= MediaTypeFlag.Broadcast;
                }
                else
                {
                    MediaType &= ~MediaTypeFlag.Broadcast;

                }
            }
        }

        [NotMapped]
        public bool PrintMedia
        {
            get
            {
                if (MediaType == MediaTypeFlag.NotSet) return false;
                return MediaType.HasFlag(MediaTypeFlag.Print);
            }
            set
            {
                if (value)
                {
                    MediaType ^= MediaTypeFlag.Print;
                }
                else
                {
                    MediaType &= ~MediaTypeFlag.Print;

                }
            }
        }

        [NotMapped]
        public bool DigitalMedia
        {
            get
            {
                if (MediaType == MediaTypeFlag.NotSet) return false;
                return MediaType.HasFlag(MediaTypeFlag.Digital);
            }
            set
            {
                if (value)
                {
                    MediaType ^= MediaTypeFlag.Digital;
                }
                else
                {
                    MediaType &= ~MediaTypeFlag.Digital;

                }
            }
        }

        public virtual ICollection<MediaCoverage> MediaCoverages { get; set; }
        public virtual ICollection<CampaignPartnerAgency> CampaignPartnerAgencies { get; set; }



    }
}