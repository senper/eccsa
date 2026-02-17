using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Communication;
using ECCSA.BackOffice.Server.Model.Trade;
using ECCSA.BackOffice.Server.Model.Website;
using System;
using System.Collections.Generic;
using System.Linq;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.WebApp.Models;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model
{
    public class ResourceViewModel : CommunicationViewModel<Resource>
    {
        public string Controller => "Resource";
        public List<LookupValue> ResourceTypes { get; set; } = new List<LookupValue>();

        public string TypeName
        {
            get
            {
                var tmp = ResourceTypes.First(r => r.Id == TheModel.TypeId);
                return tmp.Name;
            }
        }
    }

    public class LawsViewModel : CommunicationViewModel<Law>
    {
        public string Controller => "Laws";
    }


    public class WebsiteViewModel : CommunicationViewModel<Menu>
    {
        public WebsiteViewModel()
        {
            ChangeActive(MenuItemEnum.WebsiteMenu);
        }

        public List<QuickLink> Links { get; set; }
        public List<QuickLinkHeader> QuickLinkHeaders { get; set; }

        public QuickLink Link { get; set; }

        public string Controller => "Website";
    }


    public class QuickLinkHeaderViewModel : CommunicationViewModel<QuickLinkHeader>
    {
        public QuickLinkHeaderViewModel()
        {
            ChangeActive(MenuItemEnum.WebsiteQuickLink);
        }
        public List<QuickLink> Links { get; set; }
        public List<Menu> Menus { get; set; }
        public QuickLink Link { get; set; }


        public string Controller => "Website";
    }
    public class MediaTypeViewModel : CommunicationViewModel<MediaType>
    {
        public MediaTypeViewModel()
        {
            ChangeActive(MenuItemEnum.CommunicationMediaType);
        }
        public List<QuickLink> Links { get; set; }


        public List<Menu> Menus { get; set; }

        public string Controller => "Website";
    }

    public class WebsiteTranslationViewModel : CommunicationViewModel<Translation>
    {
        public WebsiteTranslationViewModel()
        {
            ChangeActive(MenuItemEnum.WebsiteTranslation);
        }
        public List<QuickLink> Links { get; set; }

        public QuickLink Link { get; set; }
        public string Controller => "Website";
    }
    public class PressViewModel : CommunicationViewModel<OtherMedia>
    {
        public string Controller => "WebsiteContent";
        public List<MediaType> MediaTypes { get; set; }
    }
    public class AdvertViewModel : CommunicationViewModel<Advertizement>
    {
        public string Controller => "Advertizements";
    }
    public class NewsViewModel : CommunicationViewModel<News>
    {
        public string Controller => "News";
    }

    public class NewsletterViewModel : CommunicationViewModel<NewsletterIssue>
    {
        public string Controller => "Newsletter";

        public List<LookupValue> Topics { get; set; }

        public Guid TopicId { get; set; }

    }

    public class SubscribersViewModel : CommunicationViewModel<Subscription>
    {
        public string Controller => "Newsletter";

        public List<LookupValue> Topics { get; set; }

        public Guid TopicId { get; set; }
        public List<User> Users { get; set; } = new List<User>();
    }

    public class SubscriptionViewModel : CommunicationViewModel<Subscription>
    {
        public string Controller => "Subscription";

        public List<LookupValue> Topics { get; set; }

        public Guid TopicId { get; set; }
    }

    public class FooterMenuViewModel : CommunicationViewModel<FooterMenu>
    {
        public FooterMenuViewModel()
        {
            ChangeActive(MenuItemEnum.FooterMenu);
        }
        public string Controller => "FooterMenu";
        public int FindIndex(FooterLink item, bool usefulLink)
        {
            if (TheModel != null && TheModel.UsefulLinks != null && usefulLink)
                return TheModel.UsefulLinks.FindIndex(s => s.Title == item.Title && s.TitleLocal == item.TitleLocal && s.Path == item.Path);
            else if (TheModel != null && TheModel.ImportantLinks != null && !usefulLink)
                return TheModel.ImportantLinks.FindIndex(s => s.Title == item.Title && s.TitleLocal == item.TitleLocal && s.Path == item.Path);
            else return 0;
        }
    }
}
