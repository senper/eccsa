using ECCSA.BackOffice.Server.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECCSA.BackOffice.Server.Model.Website
{
    public class Menu
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";
        public string Path { get; set; }
        public int? ParentId { get; set; }

        [NotMapped]
        public String ParentTitleFull => ParentMenu == null ? "" : $"{ParentMenu.Title} {ParentMenu.TitleLocal}";
        public Menu ParentMenu { get; set; }
        public ICollection<Menu> Children { get; set; }

        public string Order { get; set; }

        public int? HeaderId { get; set; }
        public bool ShowOnMainMenu { get; set; }

        public ICollection<MediaType> MediaTypes { get; set; }

    }
    public class QuickLinkHeader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public String LabelLocal { get; set; }
        [NotMapped]
        public String LabelFull => $"{Label} {LabelLocal}";

        public ICollection<QuickLink> QuickLinks { get; set; }
    }
   
    public class QuickLink
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public string Title { get; set; }
        public String TitleLocal { get; set; }
        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";
        public int Order { get; set; }
        public QuickLinkHeader Header { get; set; }

        [NotMapped]
        public string Path { get; set; }

        [NotMapped] public string HeaderName => Header == null ? string.Empty : Header.Name;
        public int? MenuId { get; set; }

        [NotMapped]
        public Menu Menu { get; set; }

        [NotMapped]
        public String MenuName => Menu == null ? string.Empty : Menu.TitleFull;

        public ICollection<MediaType> MediaTypes { get; set; }
    }

  

    public class MediaType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public String NameLocal { get; set; }

        [NotMapped]
        public String NameFull => $"{Name} {NameLocal}";

        public ECCSAModuleEnum MediaAuthor { get; set; } = ECCSAModuleEnum.Communication;
        public int? MenuId { get; set; }
        public int? QuickLinkId { get; set; }
        public Menu Menu { get; set; }
        [NotMapped]
        public String MenuName => Menu == null ? string.Empty : Menu.TitleFull;
        [NotMapped]
        public String LinkName => QuickLink == null ? string.Empty : QuickLink.TitleFull;
        public QuickLink QuickLink { get; set; }
        
        [NotMapped]
        public List<OtherMedia> Contents { get; set; }
        [NotMapped]
        public QuickLinkHeader QuickLinkHeader { get; set; }

        [NotMapped]  public String HeaderName => QuickLink == null ? string.Empty : QuickLink.HeaderName;
        [NotMapped] public String GroupName => $"{HeaderName}";

    }
}
