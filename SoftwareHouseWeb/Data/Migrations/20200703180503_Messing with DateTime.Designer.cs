﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoftwareHouseWeb.Data;

namespace SoftwareHouseWeb.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200703180503_Messing with DateTime")]
    partial class MessingwithDateTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("InitialPromoUsed")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime>("JoiningDate")
                        .HasColumnType("Date");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Photopath");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("gender");

                    b.Property<int?>("user_communication");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("user_communication")
                        .IsUnique()
                        .HasFilter("[user_communication] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.CustomQuote.CustomQuote", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.HasKey("id");

                    b.ToTable("CustomQuotes");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Customer.Customer", b =>
                {
                    b.Property<int>("cus_id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("PhoneNo");

                    b.Property<string>("User_Id");

                    b.Property<string>("cus_name")
                        .IsRequired();

                    b.HasKey("cus_id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("User_Id")
                        .IsUnique()
                        .HasFilter("[User_Id] IS NOT NULL");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Employee.Employee", b =>
                {
                    b.Property<int>("Employee_id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("Date");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Position")
                        .IsRequired();

                    b.Property<double>("Salary");

                    b.Property<string>("Skills");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("Date");

                    b.Property<int>("Time");

                    b.Property<int>("age");

                    b.Property<bool>("is_active");

                    b.HasKey("Employee_id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Orders.Order", b =>
                {
                    b.Property<int>("order_id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChargeID");

                    b.Property<DateTime>("Date")
                        .HasColumnType("Date");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("Date");

                    b.Property<string>("Message");

                    b.Property<int>("OrderStatus");

                    b.Property<int>("PaymentMethod");

                    b.Property<int>("PaymentStatus");

                    b.Property<DateTime>("StartDate");

                    b.Property<double>("TotalAmount");

                    b.Property<int>("cus_id");

                    b.HasKey("order_id");

                    b.HasIndex("cus_id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Orders.OrderPackages", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Quantity");

                    b.Property<int>("order_id");

                    b.Property<int>("pkg_Id");

                    b.Property<double>("price");

                    b.HasKey("id");

                    b.HasIndex("order_id");

                    b.HasIndex("pkg_Id");

                    b.ToTable("OrderPackages");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Orders.OrderTeam", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("emp_id");

                    b.Property<int>("order_id");

                    b.HasKey("id");

                    b.HasIndex("emp_id");

                    b.HasIndex("order_id");

                    b.ToTable("OrderTeam");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Packages.Packages", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Desc");

                    b.Property<double>("DiscountPercent")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0.0);

                    b.Property<string>("Heading");

                    b.Property<DateTime>("LaunchDate")
                        .HasColumnType("Date");

                    b.Property<string>("PhotoPath");

                    b.Property<string>("PkgName")
                        .IsRequired();

                    b.Property<int>("Ser_Id");

                    b.Property<double>("TotalPrice");

                    b.Property<bool>("isActive");

                    b.HasKey("id");

                    b.HasIndex("Ser_Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Portfolio.Portfolio", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Desc");

                    b.Property<string>("Heading");

                    b.Property<string>("PhotoPath");

                    b.Property<int>("Ser_Id");

                    b.Property<DateTime>("created_At");

                    b.HasKey("id");

                    b.HasIndex("Ser_Id");

                    b.ToTable("Portfolio");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Promos.Promo", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PromoCode")
                        .IsRequired();

                    b.Property<double>("PromoDiscount");

                    b.Property<int>("Ser_id");

                    b.HasKey("id");

                    b.HasIndex("Ser_id", "PromoCode")
                        .IsUnique();

                    b.ToTable("Promos");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Review.Review", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Desc")
                        .IsRequired();

                    b.Property<double>("Rating");

                    b.Property<int>("ReviewStaus");

                    b.Property<string>("User_Id")
                        .IsRequired();

                    b.Property<DateTime>("created_At");

                    b.HasKey("id");

                    b.HasIndex("User_Id")
                        .IsUnique();

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Services.OurServices", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IconPath");

                    b.Property<string>("ServiceName");

                    b.Property<bool>("isActive");

                    b.HasKey("id");

                    b.HasIndex("ServiceName")
                        .IsUnique()
                        .HasFilter("[ServiceName] IS NOT NULL");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.UsersCommunication", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<TimeSpan>("FirstPreferedTimeEnd");

                    b.Property<TimeSpan>("FirstPreferedTimeStart");

                    b.Property<string>("FirstPreferences")
                        .IsRequired();

                    b.Property<TimeSpan>("SecondPreferedTimeEnd");

                    b.Property<TimeSpan>("SecondPreferedTimeStart");

                    b.Property<string>("SecondPreferences")
                        .IsRequired();

                    b.HasKey("id");

                    b.ToTable("UsersCommunications");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoftwareHouseWeb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.ApplicationUser", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.UsersCommunication", "User_Communication")
                        .WithOne("User_Model")
                        .HasForeignKey("SoftwareHouseWeb.Data.ApplicationUser", "user_communication")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Customer.Customer", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.ApplicationUser", "User_Model")
                        .WithOne("Customer")
                        .HasForeignKey("SoftwareHouseWeb.Data.Models.Customer.Customer", "User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Orders.Order", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.Models.Customer.Customer", "Cus_model")
                        .WithMany("Order")
                        .HasForeignKey("cus_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Orders.OrderPackages", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.Models.Orders.Order", "Order")
                        .WithMany("OrderPackages")
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoftwareHouseWeb.Data.Models.Packages.Packages", "Packages")
                        .WithMany("OrderPackages")
                        .HasForeignKey("pkg_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Orders.OrderTeam", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.Models.Employee.Employee", "Employee")
                        .WithMany("OrderTeams")
                        .HasForeignKey("emp_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoftwareHouseWeb.Data.Models.Orders.Order", "Order")
                        .WithMany("OrderTeam")
                        .HasForeignKey("order_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Packages.Packages", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.Models.Services.OurServices", "Service_Model")
                        .WithMany("Packages")
                        .HasForeignKey("Ser_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Portfolio.Portfolio", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.Models.Services.OurServices", "Service_Model")
                        .WithMany("Portfolios")
                        .HasForeignKey("Ser_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Promos.Promo", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.Models.Services.OurServices", "Services")
                        .WithMany("Promos")
                        .HasForeignKey("Ser_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SoftwareHouseWeb.Data.Models.Review.Review", b =>
                {
                    b.HasOne("SoftwareHouseWeb.Data.ApplicationUser", "User")
                        .WithOne("Review_Model")
                        .HasForeignKey("SoftwareHouseWeb.Data.Models.Review.Review", "User_Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
