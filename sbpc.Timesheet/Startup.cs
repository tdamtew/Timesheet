﻿using Microsoft.AspNetCore.Builder;
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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ITimesheetRepository, TimesheetRepository>();
            services.AddAutoMapper();
            services.AddMvc();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRole", policy => policy.Requirements.Add(new AdminRequirement(Configuration.GetSection("Data:MasterAdmin").Get<string[]>())));
                options.AddPolicy("TimesheetAdminRole", policy => policy.Requirements.Add(new TimesheetAdminRequirement(Configuration.GetSection("Data:TimesheetAdmin").Get<string[]>())));
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
