using Accepted.FluentValidation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollection_FluentValidation
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection iServiceCollection, Action<FluentValidationOptions> configure)
        {
            var options = new FluentValidationOptions();
            configure.Invoke(options);

            iServiceCollection.AddSingleton(new FluentValidator(options));

            return iServiceCollection;
        }
    }
}
