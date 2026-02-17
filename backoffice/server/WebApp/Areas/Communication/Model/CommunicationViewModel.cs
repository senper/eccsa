using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Communication.Model
{
    public class CommunicationViewModel<T>: BaseViewModel
    {
        public CommunicationViewModel()
        {
            ChangeActive(MenuItemEnum.Dashboard);
            CurrentModule = Server.Model.Common.ECCSAModuleFlag.Communication;
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.CommunicationMediaAgency, "" },
                { MenuItemEnum.CommunicationMediaCoverage, "" },
                { MenuItemEnum.CommunicationCampaign, "" },

                { MenuItemEnum.CommunicationLaws, "" },
                { MenuItemEnum.CommunicationTextMedia, "" },
                { MenuItemEnum.CommunicationAdvertisment, "" },
                { MenuItemEnum.CommunicationPhotoAlbum, "" },
                { MenuItemEnum.CommunicationVideoGallery, "" },

                { MenuItemEnum.Resource, "" },
                { MenuItemEnum.News, "" },
                { MenuItemEnum.Newsletter, "" },
                { MenuItemEnum.CommunicationContactUs, "" },

                { MenuItemEnum.WebsiteMenu, "" },
                { MenuItemEnum.WebsiteQuickLink, "" },
                { MenuItemEnum.WebsiteContent, "" },
                { MenuItemEnum.WebsiteTranslation, "" },
                { MenuItemEnum.CommunicationMediaType, "" },
                { MenuItemEnum.FooterMenu, "" },

            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }

     

        public List<T> TheList { get; set; }

        public T TheModel { get; set; }

        public IFormFile Photo { get; set; }
        public IFormFile HomePhoto { get; set; }

        public List<string> ProfileImages = new List<string>();
        public string AttachmentPath { get; set; }
        public Contact Contact { get; set; }

        public string Area => "Communication";
        

    }

    
}
