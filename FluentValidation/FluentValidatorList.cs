using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Accepted.FluentValidation
{
    public class FluentValidatorList : IFluentValidatorList
    {
        // List of validators per type
        public Dictionary<Type, List<IValidator>> Validators { get; private set; } = new Dictionary<Type, List<IValidator>>();

        public FluentValidatorList(IServiceProvider sp)
        {
            foreach (var entityValidator in sp.GetServices<IValidator>())
            {
                // get typeof T from IValidator<T> 
                var t = entityValidator.GetType().BaseType.GenericTypeArguments.FirstOrDefault()?.UnderlyingSystemType;
                if (t == null)
                    continue;

                if (Validators.ContainsKey(t))
                {
                    Validators[t].Add(entityValidator);
                }
                else
                {
                    Validators.Add(t, new List<IValidator> { entityValidator });
                }
            }
        }
    }
}
