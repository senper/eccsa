using System;

namespace ECCSA.BackOffice.Server.Persistence.Repositories.Communication
{
    public enum MediaAgencyOwnershipType
    {
        NotSet = 0,
        Private = 1,
        Public = 2,
        Community = 3
    }

    [Flags]
    public enum MediaTypeFlag
    {

        Broadcast = 1,
        Print = 2,
        Digital = 4
    }

    [Flags]
    public enum SocialMediaFlag
    {
        Facebook = 1,
        Twitter = 2,
        Instagram = 4,
        LinkedIn = 8
    }

    [Flags]
    public enum MediaOutletFlag
    {
        SMS = 1,
        Telegram = 2,
        Email = 4,
        Website = 8
    }

    public enum CampaignStatusEnum
    {
        NotSet = 0,
        Planned = 1,
        Active = 2,
        Closed = 9
    }
}
