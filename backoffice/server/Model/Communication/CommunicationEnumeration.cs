using System;
using System.ComponentModel.DataAnnotations;

namespace ECCSA.BackOffice.Server.Model.Communication
{
    public enum MediaAgencyOwnershipType
    {
        [Display(Name = " ")]
        NotSet = 0,
        Private = 1,
        Public = 2,
        Community = 3,
        International = 4
    }

    [Flags]
    public enum MediaTypeFlag
    {
        [Display(Name = " ")]
        NotSet = 0,
        Broadcast = 1,
        Print = 2,
        Digital = 4
    }

    [Flags]
    public enum SocialMediaFlag
    {
        [Display(Name = " ")]
        NotSet = 0,
        Facebook = 1,
        Twitter = 2,
        Instagram = 4,
        LinkedIn = 8
    }

    

    public enum CampaignStatusEnum
    {
        [Display(Name = " ")]
        NotSet = 0,
        Planned = 1,
        Active = 2,
        Closed = 9
    }
}
