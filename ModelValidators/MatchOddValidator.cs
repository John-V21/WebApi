using Accepted.Models;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accepted.ModelValidators
{
    public class MatchOddValidator : AbstractValidator<MatchOdd>
    {
        public MatchOddValidator()
        {
            RuleFor(x => x.Specifier).NotNull().MinimumLength(1).MaximumLength(1).WithMessage("One char is required of 1,2 or X");
            RuleFor(x => x.Odd).GreaterThan(0).WithMessage("Odd must be greater than 0");
        }
    }
}
