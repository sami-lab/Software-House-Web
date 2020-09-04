using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareHouseWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoftwareHouseWeb.Data.Interfaces;
using SoftwareHouseWeb.Data.Repositories;
using SoftwareHouseWeb.Security.Tokens;
using SoftwareHouseWeb.Services.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using SoftwareHouseWeb.Services.Stripe;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace SoftwareHouseWeb
{
    public class MyIdentityDataInitializer
    {
        public MyIdentityDataInitializer(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void SeedData
    (UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public void SeedUsers
    (UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByEmailAsync(Configuration["Email"]).Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = Configuration["Admin_Username"];
                user.Email = Configuration["Email"];
                user.Name = Configuration["Name"];
                user.PhoneNumber = Configuration["Phone"];
                user.JoiningDate = DateTime.Now;
                user.gender = Gender.Male;
                user.Country = "Pakistan";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync
                (user, Configuration["Admin_Password"]).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "Admin").Wait();
                }
            }

        }

        public void SeedRoles
    (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }
    }
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
            //services.AddMemoryCache();
            //services.AddSession();
            //services.Configure<CookieTempDataProviderOptions>(options =>
            //{
            //    options.Cookie.IsEssential = true;
            //});
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Setting Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                Configuration.GetConnectionString("ConString")));

            //Setting Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 7;
                options.Password.RequiredUniqueChars = 3;
                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
            })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders()
           .AddTokenProvider<CustomEmailConfirmationTokenProvider
            <ApplicationUser>>("CustomEmailConfirmation");

            // Changes token lifespan of all token types
            services.Configure<DataProtectionTokenProviderOptions>(o =>
               o.TokenLifespan = TimeSpan.FromHours(5));
            // Changes token lifespan of just the Email Confirmation Token type
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
                    o.TokenLifespan = TimeSpan.FromDays(3));






            //Setting Up Global Authentication
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                  .RequireAuthenticatedUser()
                                  .Build();

                config.Filters.Add(new AuthorizeFilter(policy));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Login Path
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
            });


            //External Login
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["GoogleClientId"];
                options.ClientSecret = Configuration["GoogleClientSecret"];
            })
            .AddFacebook(options =>
            {
                options.ClientId = Configuration["FacebookClientId"];
                options.ClientSecret = Configuration["FacebookClientSecret"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("abcUser", policy => policy.RequireRole("Admin"));
            });


            //Register DI
            //services.AddHttpContextAccessor();

            //Register Repos
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPackagesRepository, PackagesRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IPromoRepository, PromoRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IServicesRepository, ServicesRepository>();

            //Email And payment Sevice
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<EmailAuthOptions>(Configuration);
            services.Configure<StripeSetting>(Configuration.GetSection("Stripe"));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Register purpose string class with DI container
            services.AddSingleton<DataProctectionPurposeString>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            app.UseHttpsRedirection();
           
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            MyIdentityDataInitializer myIdentityDataInitializer = new MyIdentityDataInitializer(Configuration);
            myIdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
