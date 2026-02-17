using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Events.Model
{
    public class EventViewModel : BaseViewModel
    {
        public EventViewModel()
        {
            ChangeActive(MenuItemEnum.Events);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.Dashboard, "" },
                { MenuItemEnum.EventType, "" },
                { MenuItemEnum.Events, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }
        
        public Guid Id { get; set; }
        public EventStatus Status { get; set; }
        public List<EventType> EventTypes { get; set; }

        public EventType  EventType { get; set; }
        public List<Event> TheList { get; set; }

        public Event TheModel { get; set; }

        public IFormFile Photo { get; set; }


        public Guid EventTypeId { get; set; }
        public String Title { get; set; }

        public String Description { get; set; }

        public ECCSAModuleFlag EventOwner { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }



        public RateBasisEnum RateBasis { get; set; }
        public decimal MemberServiceCharge { get; set; }
        public decimal NonMemberServiceCharge { get; set; }

        public Guid DetailId { get; set; }
        public Guid EventId { get; set; }




    }


}
