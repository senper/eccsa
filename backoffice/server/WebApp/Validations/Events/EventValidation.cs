using ECCSA.BackOffice.Server.Model.Common.EventManagement;
using FluentValidation;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Validations.Events
{
    public class EventValidation: AbstractValidator<Event>
    {
        public EventValidation()
        {
            RuleFor(x => (int)x.EventOwner).NotEqual(0).WithMessage("Owner is required");
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required");
            RuleFor(x => new { x.StartDate, x.EndDate })
                .Must(y => StartDateIsBeforeEndDate(y.StartDate, y.EndDate)).WithMessage("Start Date should be before End Date");
        }

      

        private bool StartDateIsBeforeEndDate(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate;
        }
      
    }

    public class EventDetailValidation : AbstractValidator<EventDetail>
    {
        public EventDetailValidation()
        {
            RuleFor(x => new {x.Event.StartDate
                , x.Event.EndDate
                , DetailStart= x.StartDate
                , DetailEnd= x.EndDate })
                .Must(y => StartDateIsBeforeEndDate(y.StartDate
                , y.EndDate
                , y.DetailStart
                , y.DetailEnd));
        }



        private bool StartDateIsBeforeEndDate(DateTime eventStartDate
            , DateTime eventEndDate
            , DateTime DetailStartDate
            , DateTime DetailEndDate)
        {
            return DetailStartDate >= eventStartDate &&
                DetailEndDate <= eventEndDate &&
                DetailStartDate <= DetailEndDate;
        }

    }

}
