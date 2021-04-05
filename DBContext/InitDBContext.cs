using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Accepted.DBContext
{
    static public class InitDBContext
    {
        public static IHost CreateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    //context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    services.GetRequiredService<ILogger<Program>>()?.LogError(ex, "Could not create DB.");
                }
            }
            return host;
        }
    }
}
