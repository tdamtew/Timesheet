using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace sbpc.Timesheet.Data.Repository
{
    public static class DbInitializer
    {
        public static void Seed(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!context.Jobs.Any())
            {
                context.Jobs.AddRange(
                    new Entity.Job { Name = "SBP", OverTimeRate = 1, CostPerMile = 0.56 },
                    new Entity.Job { Name = "Motorola Inc.:State of MD", OverTimeRate = 1, CostPerMile = 0.56 }
                    );
            }
            context.SaveChanges();
        }
    }
}
