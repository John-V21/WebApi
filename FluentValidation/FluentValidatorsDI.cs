using Accepted.FluentValidation;
using FluentValidation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollection_FluentValidation
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection iServiceCollection, Action<FluentValidationOptions> configure)
        {
            var options = new FluentValidationOptions();
            configure.Invoke(options);

            // inject validators
            foreach(var validator in options.Validators)
            {
                iServiceCollection.AddSingleton(validator);
                iServiceCollection.AddSingleton(typeof(IValidator), (sp) => sp.GetService(validator));
            }

            // inject IFluentValidatorList && FluentValidator
            iServiceCollection.AddSingleton<IFluentValidatorList, FluentValidatorList>();
            iServiceCollection.AddSingleton<FluentValidator>();
            return iServiceCollection;
        }
    }
}
