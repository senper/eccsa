using ECCSA.BackOffice.Server.Model.Advocacy;
using FluentValidation;
using System;

namespace ECCSA.BackOffice.Server.WebApp.Validations.Advocacy
{
    public class DialogValidator : AbstractValidator<Dialog>
    {
        public DialogValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required");
        }
    }

    public class ProjectValidator : AbstractValidator<Project>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required");
      
            RuleFor(x => new { x.PlannedStartDate, x.PlannedEndDate })
                .Must(y => PlannedStartDateIsBeforePlannedEndDate(y.PlannedStartDate, y.PlannedEndDate)).WithMessage("planned Start Date should be before planned End Date");

            
            RuleFor(x => new { x.StartDate, x.EndDate })
                .Must(y => StartDateIsBeforeEndDate(y.StartDate, (DateTime)y.EndDate)).WithMessage("start date should be before End Date");
        }

        private bool PlannedStartDateIsBeforePlannedEndDate(DateTime plannedStartDate, DateTime plannedEndDate)
        {
            return plannedStartDate <= plannedEndDate;
        }

        private bool StartDateIsBeforeEndDate(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate;
        }
    }


    public class ProjectFollowupValidator : AbstractValidator<ProjectFollowup>
    {
        public ProjectFollowupValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Title is required");
            RuleFor(x => x.ProjectId).NotEmpty().NotNull().WithMessage("Project ID is required");

            RuleFor(x => new { x.PlannedStartDate, x.PlannedEndDate })
                .Must(y => PlannedStartDateIsBeforePlannedEndDate(y.PlannedStartDate, y.PlannedEndDate)).WithMessage("planned Start Date should be before planned End Date");


            RuleFor(x => new { x.StartDate, x.EndDate })
                .Must(y => StartDateIsBeforeEndDate(y.StartDate, (DateTime)y.EndDate)).WithMessage("start date should be before End Date");
        }

        private bool PlannedStartDateIsBeforePlannedEndDate(DateTime plannedStartDate, DateTime plannedEndDate)
        {
            return plannedStartDate <= plannedEndDate;
        }

        private bool StartDateIsBeforeEndDate(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate;
        }
    }


    public class PublicationValidator : AbstractValidator<Publication>
    {
        public PublicationValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Publication Title is required");
            RuleFor(x => x.Funding).NotEmpty().NotNull().WithMessage("Funding is required");
            RuleFor(x => x.Url).NotEmpty().NotNull().WithMessage("Research findings is required");
        }
    }

    public class ResearchValidator : AbstractValidator<Research>
    {
        public ResearchValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("Research Title is required");
            RuleFor(x => x.ConductedBy).NotEmpty().NotNull().WithMessage("Researcher name is required");
            RuleFor(x => x.Finding).NotEmpty().NotNull().WithMessage("Research findings is required");
        }
    }
}
