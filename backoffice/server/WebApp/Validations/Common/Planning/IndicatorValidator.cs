using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Common.Planning;
using FluentValidation;

namespace ECCSA.BackOffice.Server.WebApp.Validations.Common.Planning
{
    public class IndicatorValidator : AbstractValidator<Indicator>
    {
        public IndicatorValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("id is required");
        }
    }


    public class PlanValidator : AbstractValidator<Plan>
    {
        public PlanValidator()
        {

        }
    }


    public class ProgressValidator : AbstractValidator<Progress>
    {
        public ProgressValidator()
        {

        }
    }
}
