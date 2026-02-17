using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Notification;
using ECCSA.BackOffice.Server.Model.Membership;
using ECCSA.BackOffice.Server.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECCSA.BackOffice.Server.WebApp.Areas.Notification.Model
{
    public class SMSViewModel : BaseViewModel
    {
        public SMSViewModel()
        {
            ChangeActive(MenuItemEnum.SMS);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.SMS, "" },
                { MenuItemEnum.EmailMessage, "" },
                { MenuItemEnum.EmailTemplate, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }

        public Guid Id { get; set; }
        public MessageStatus Status { get; set; }
        public List<SMSMessage> TheList { get; set; }

        public SMSMessage TheModel { get; set; }

        public List<RecipientSource> RecipientsSource { get; set; }
        public StakeholderTypeEnum StakeholderType { get; set; } = StakeholderTypeEnum.NotSet;
        public Guid SectorId { get; set; } = Guid.Empty;
        public List<LookupValue> Sectors { get; internal set; }
        public String Message { get; set; }

        public ECCSAModuleFlag Owner { get; set; }


        public string Area => "Notification";

        public string Controller => "SMS";

    }

    public class EmailMessageViewModel : BaseViewModel
    {
        public EmailMessageViewModel()
        {
            ChangeActive(MenuItemEnum.SMS);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.SMS, "" },
                { MenuItemEnum.EmailMessage, "" },
                { MenuItemEnum.EmailTemplate, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }

        public Guid Id { get; set; }
        public MessageStatus Status { get; set; }
        public List<EmailMessage> TheList { get; set; }

        public EmailMessage TheModel { get; set; }
        public List<RecipientSource> RecipientsSource { get; set; }
        public ECCSAModuleFlag Owner { get; set; }

        public String Subject { get; set; }

        public Guid TemplateId { get; set; }

        public String Body { get; set; }

        public List<MessageDelivery> Recipients { get; set; } = new List<MessageDelivery>();
        public List<EmailTemplate> Templates { get; set; }

        public IFormFile Attachment { get; set; }
        public StakeholderTypeEnum StakeholderType { get; set; } = StakeholderTypeEnum.NotSet;
        public Guid SectorId { get; set; } = Guid.Empty;
        public string Area => "Notification";

        public string Controller => "Email";

        public List<LookupValue> Sectors { get; internal set; }
    }
    public class EmailTemplateViewModel : BaseViewModel
    {
        public EmailTemplateViewModel()
        {
            ChangeActive(MenuItemEnum.EmailTemplate);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.SMS, "" },
                { MenuItemEnum.EmailMessage, "" },
                { MenuItemEnum.EmailTemplate, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }

        public Guid Id { get; set; }
        public MessageStatus Status { get; set; }
        public List<EmailTemplate> TheList { get; set; }

        public EmailTemplate TheModel { get; set; }



        public String Message { get; set; }

        public ECCSAModuleFlag Owner { get; set; }


        public string Area => "Notification";

        public string Controller => "EmailTemplate";

    }


    public class RecipientViewModel : BaseViewModel
    {
        public RecipientViewModel()
        {
            ChangeActive(MenuItemEnum.RecipientSource);
        }
        public void ChangeActive(MenuItemEnum menuItem)
        {
            MenuItem = new Dictionary<MenuItemEnum, string>
            {
                { MenuItemEnum.SMS, "" },
                { MenuItemEnum.EmailMessage, "" },
                { MenuItemEnum.EmailTemplate, "" },
                { MenuItemEnum.RecipientSource, "" },
            };
            if (MenuItem.ContainsKey(menuItem))
            {
                MenuItem[menuItem] = MenuItemActive;
            }
        }

        public Guid Id { get; set; }
        public StakeholderTypeEnum StakeholderType { get; set; }
        public List<RecipientSource> TheList { get; set; }
        public List<TempRecipientSource> TempList { get; set; }

        public RecipientSource TheModel { get; set; }

        public String Name { get; set; }

        public String CompanyName { get; set; }

        public String MobileNo { get; set; }

        public String Email { get; set; }

        public Guid SectorId { get; set; }

        public ECCSAModuleFlag Owner { get; set; }


        public string Area => "Notification";

        public string Controller => "Recipient";

        public List<LookupValue> Sectors { get; set; }
        public List<LookupGroup> GroupedSectors
        {
            get
            {
                var list = new List<LookupGroup>();
                if (Sectors != null)
                {
                    foreach (var sector in Sectors)
                    {
                        list.Add(new LookupGroup
                        {
                            Id = sector.Id,
                            Name = sector.Name,
                            Parent = Sectors?.FirstOrDefault(d => d.Id == sector.ParentId)?.Name ?? ""
                        });
                    }
                }
                return list;
            }
        }
    }
    public class LookupGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
    }
}
