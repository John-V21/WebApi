using FluentValidation;
using System;
using System.Collections.Generic;

namespace Accepted.FluentValidation
{
    public interface IFluentValidatorList
    {
        Dictionary<Type, List<IValidator>> Validators { get; }
    }
}