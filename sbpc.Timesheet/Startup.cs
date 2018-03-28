using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sbpc.Timesheet.Data;
using sbpc.Timesheet.Services;
using sbpc.Timesheet.Data.Entity;
using sbpc.Timesheet.Data.Repository;
using AutoMapper;
using sbpc.Timesheet.Helpers;
using System;

namespace sbpc.Timesheet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Timesheet")));

            //configure password complexity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredUniqueChars = 2;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            //configure cookie expiration.
            services.AddAuthentication().Services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ITimesheetRepository, TimesheetRepository>();
            services.AddAutoMapper();

            services.AddMvc();

            //configure role based authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRole", policy => policy.Requirements.Add(new AdminRequirement()));
                options.AddPolicy("TimesheetAdminRole", policy => policy.Requirements.Add(new TimesheetAdminRequirement()));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHttpsEnforcement();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Timesheet}/{action=Index}/{id?}");
            });
        }
    }
}
