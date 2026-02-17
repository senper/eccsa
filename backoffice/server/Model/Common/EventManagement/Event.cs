using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace ECCSA.BackOffice.Server.Model.Common.EventManagement
{
    public class Event: Entity 
    {

        public Guid EventTypeId { get; set; }
        public String Title { get; set; }
        public String TitleLocal { get; set; }

        [NotMapped]
        public String TitleFull => $"{Title} {TitleLocal}";
        public String DescriptionLocal { get; set; }
        public String Description { get; set; }
        
        public ECCSAModuleFlag EventOwner { get; set; }
        [NotMapped]
        public string OwnerStr => EventOwner.ToString();
        public virtual EventType EventType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public string StartDateStr => StartDate.ToString("dd/MM/yyyy");
        [NotMapped]
        public string EndDateStr => EndDate.ToString("dd/MM/yyyy");

        public virtual ICollection<EventDetail> Details { get; set; }
        [NotMapped]
        public virtual List<EventDetail> DetailsSorted => Details.OrderBy(s => s.StartDate).ThenBy(s => s.EndDate).ToList();
        [NotMapped]
        public virtual List<EventScheduleDetail> SortedDetails
        {
            get
            {
                var newList = new List<EventScheduleDetail>();
                if (Details == null) return newList;
                newList.AddRange(Details.Select(s => new EventScheduleDetail
                {
                    Title = s.Title + " start",
                    TitleLocal = s.TitleLocal ,
                    Date = s.StartDate,
                    IsEnded = DateTime.Today >= s.EndDate,
                    IsFuture = DateTime.Today < s.StartDate
                }).ToList());
                newList.AddRange(Details.Select(s => new EventScheduleDetail
                {
                    Title = s.Title + " end",
                    TitleLocal = s.TitleLocal,
                    Date = s.EndDate,
                    IsEndDate = true,
                    IsEnded = DateTime.Today >= s.EndDate,
                    IsFuture = DateTime.Today < s.StartDate
                }).ToList());
                return newList.OrderBy(s => s.Date).ToList();
            }
        }

        public EventStatus Status { get; set; }
        public bool BannerSetted => File.Exists(BannerUrl);
        public string BannerUrl =>
            Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot", EventOwner.ToString(),"documents",
                   Id.ToString()[^15..].Replace("-", "")+".jpg");

        public string BannerUrlRelative =>
            Path.Combine(EventOwner.ToString(), "documents",
                   Id.ToString()[^15..].Replace("-", "") + ".jpg");


    }
    public record EventDescription(Guid Id, string Title, string Type);
}
