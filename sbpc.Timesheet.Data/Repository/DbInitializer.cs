using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace sbpc.Timesheet.Data.Repository
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" },
                        new IdentityRole { Name = "User", NormalizedName = "User" });

                }
                if (!context.Items.Any())
                {
                    context.Items.AddRange(
                        new Entity.Item { Name = "Billable", BillingType = "A" },
                        new Entity.Item { Name = "Non-Billable", BillingType = "A" },
                        new Entity.Item { Name = "Overtime", BillingType = "A" },
                        new Entity.Item { Name = "Travel", BillingType = "A" },
                        new Entity.Item { Name = "Driver", BillingType = "D" },
                        new Entity.Item { Name = "Driver OT", BillingType = "A" }
                        );
                }
                if (!context.Jobs.Any())
                {
                    context.Jobs.AddRange(
                        new Entity.Job { Name = "SBP", OverTimeRate = 1, CostPerMile = 0.56 },
                        new Entity.Job { Name = "Motorola Inc.:State of MD", OverTimeRate = 1, CostPerMile = 0.56 }
                        );
                }
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Entity.Category { Name = "Miscellaneous" },
                        new Entity.Category { Name = "Breakfast" },
                        new Entity.Category { Name = "Lunch" },
                        new Entity.Category { Name = "Dinner" },
                        new Entity.Category { Name = "Snacks" },
                        new Entity.Category { Name = "Flight" },
                        new Entity.Category { Name = "Baggage" },
                        new Entity.Category { Name = "Hotel" },
                        new Entity.Category { Name = "Rental Car" },
                        new Entity.Category { Name = "Tolls" },
                        new Entity.Category { Name = "Fuel" }
                        );
                }
                if (!context.Methods.Any())
                {
                    context.Methods.AddRange(
                        new Entity.Method { Name = "Personal" },
                        new Entity.Method { Name = "Company" }
                        );
                }
                context.SaveChanges();
            }
        }
    }
}
