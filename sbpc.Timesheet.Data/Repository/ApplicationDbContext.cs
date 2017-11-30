using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using sbpc.Timesheet.Data.Entity;
using System;
using System.IO;

namespace sbpc.Timesheet.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hour> Hours { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Mileage> Mileages { get; set; }
        public DbSet<Method> Methods { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(x => { x.ToTable("Users"); });
            builder.Entity<IdentityRole>(x => { x.ToTable("Roles"); });
            builder.Entity<IdentityRoleClaim<string>>(x => 
            {
                x.HasOne<IdentityRole>().WithMany().HasForeignKey(a => a.RoleId).OnDelete(DeleteBehavior.Cascade);
                x.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserClaim<string>>(x =>
            {
                x.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                x.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(x =>
            {
                x.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                x.ToTable("UserLogins");
            });
            builder.Entity<IdentityUserToken<string>>(x =>
            {
                x.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                x.ToTable("UserTokens");
            });
            builder.Entity<IdentityUserRole<string>>(x =>
            {
                x.HasOne<IdentityRole>().WithMany().HasForeignKey(a => a.RoleId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                x.ToTable("UserRoles");
            });

            builder.Entity<Expense>(x =>
            {
                x.HasOne<Method>().WithMany().HasForeignKey(a => a.MethodId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne<Category>().WithMany().HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne<Job>().WithMany().HasForeignKey(a => a.JobId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Hour>(x =>
            {
                x.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne<Job>().WithMany().HasForeignKey(a => a.JobId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne<Item>().WithMany().HasForeignKey(a => a.ItemId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Mileage>(x =>
            {
                x.HasOne<ApplicationUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne<Job>().WithMany().HasForeignKey(a => a.JobId).OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
