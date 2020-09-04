using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SoftwareHouseWeb.Data.Models.Customer;
using SoftwareHouseWeb.Data.Models.CustomQuote;
using SoftwareHouseWeb.Data.Models.Employee;
using SoftwareHouseWeb.Data.Models.Orders;
using SoftwareHouseWeb.Data.Models.Packages;
using SoftwareHouseWeb.Data.Models.Portfolio;
using SoftwareHouseWeb.Data.Models.Promos;
using SoftwareHouseWeb.Data.Models.Review;
using SoftwareHouseWeb.Data.Models.Services;


namespace SoftwareHouseWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public IConfiguration Configuration { get; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);

            //Services Relations
            modelBuilder.Entity<OurServices>()
              .HasMany(a => a.Portfolios)
              .WithOne(b => b.Service_Model)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OurServices>()
             .HasMany(a => a.Packages)
             .WithOne(b => b.Service_Model)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OurServices>()
              .HasMany(a => a.Promos)
              .WithOne(b => b.Services)
              .OnDelete(DeleteBehavior.Cascade);

            //Application User Relations
            modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Review_Model)
            .WithOne(b => b.User)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
           .HasOne(a => a.Customer)
           .WithOne(b => b.User_Model)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
               .HasMany(a => a.OrderTeams)
               .WithOne(b => b.Employee)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.User_Communication)
            .WithOne(b => b.User_Model)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Packages>()
           .HasMany(a => a.OrderPackages)
           .WithOne(b => b.Packages)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
             .HasMany(a => a.Order)
             .WithOne(b => b.Cus_model)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
              .HasMany(a => a.OrderPackages)
              .WithOne(b => b.Order)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
              .HasMany(a => a.OrderTeam)
              .WithOne(b => b.Order)
             .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OurServices>(entity =>
            {
                entity.HasIndex(e => e.ServiceName).IsUnique();
            });
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasIndex(e => e.User_Id).IsUnique();
            });
            modelBuilder.Entity<Promo>(entity =>
            {
                entity.HasIndex(e => new { e.Ser_id , e.PromoCode}).IsUnique();
            });
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });
          
            modelBuilder.Entity<Packages>()
            .Property(b => b.DiscountPercent)
            .HasDefaultValue(0);

            modelBuilder.Entity<ApplicationUser>()
          .Property(b => b.InitialPromoUsed)
          .HasDefaultValue(false);
        }
        public DbSet<OurServices> Services { get; set; }
        public DbSet<Packages> Packages { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderPackages> OrderPackages { get; set; }
        public DbSet<OrderTeam> OrderTeam { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CustomQuote> CustomQuotes { get; set; }
        public DbSet<UsersCommunication> UsersCommunications { get; set; }
        public DbSet<Promo> Promos { get; set; }
    }
}
