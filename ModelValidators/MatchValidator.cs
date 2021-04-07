using Accepted.Models;
using FluentValidation;
using System;

namespace Accepted.ModelValidators
{
    public class MatchValidator : AbstractValidator<Match>
    {
        public MatchValidator()
        {
            RuleFor(x => x.TeamA).Must((model, field) => model.TeamA != model.TeamB).WithMessage("TeamA and TeamB cannot be equal");
            RuleFor(x => x.MatchTime).InclusiveBetween(TimeSpan.FromHours(0), TimeSpan.FromHours(24)).WithMessage("MatchTime out of range 00-24");
        }
    }
}
