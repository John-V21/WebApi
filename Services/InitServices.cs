
using Accepted.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollection_Services
    {
        public static IServiceCollection AddServices(this IServiceCollection iServiceCollection)
        {
            iServiceCollection.AddScoped<IMatchesService, MatchesService>();
            iServiceCollection.AddScoped<IMatchOddsService, MatchOddsService>();
            return iServiceCollection;
        }
    }
}