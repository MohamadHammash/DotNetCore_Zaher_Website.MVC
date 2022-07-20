using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Zaher.Core.Entities;
using Zaher.Core.Repositories;
using Zaher.Data.Data;
using Zaher.Data.Repositories;
using Zaher.Ui.AutoMapper;
using Zaher.Ui.Services;

namespace Zaher.Ui
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential
                // cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                // requires using Microsoft.AspNetCore.Http;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {

                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            })
               


            .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddScoped<IUoW, UoW>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IVideosListsRepository, VideosListsRepository>();
            services.AddScoped<IVideosRepository, VideosRepository>();
            services.AddScoped<IPostingCardsRepository, PostingCardsRepository>();
            services.AddScoped<IQAsRepository, QAsRepository>();
            services.AddScoped<IFBooksRepository, FBooksRepository>();
            services.AddScoped<ISectionsRepository, SectionsRepository>();
            services.AddScoped<ISelectVideosListId, SelectVideosListId>();
            services.AddScoped<ISelectServices, SelectServices>();

            services.AddAutoMapper(typeof(MapperProfile));
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(1095);
            });



            //services
            //   .AddFluentEmail("z-alshawa@info.com")
            //   .AddSmtpSender("smtp-relay.sendinblue.com", 587, "mohamad.hammash93@gmail.com", Configuration["SmtpPw"]);
            //services.AddTransient<IEmailSender, EmailSender>();



            services
               .AddFluentEmail("z-alshawa@info.com")
               .AddSmtpSender("smtp-relay.sendinblue.com", 587, "zaher.sh1392@gmail.com", Configuration["SmtpPw "]);
            services.AddTransient<IEmailSender, EmailSender>();


            //services
            //   .AddFluentEmail("z-alshawa@info.com")
            //   .AddSmtpSender("127.0.0.1", 25);
            //services.AddTransient<IEmailSender, EmailSender>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
