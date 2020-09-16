using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsApp.DataAccess.Context;
using GoodNewsApp.DataAccess.Entities;
using GoodNewsApp.DataAccess.Interfaces;
using GoodNewsApp.DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using GoodNewsApp.BusinessLogic.Services;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using GoodNewsApp.BusinessLogic.Services.NewsServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GoodNewsApp.BusinessLogic.Helpers;

namespace GoodNewsApp.WEB
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
                

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddSwaggerGen
                (c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GoodNewsWebAPI"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
               var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.LoginPath = "/UserAccount/Login");

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<UnitOfWork>();
                        var userName = context.Principal.Identity.Name;
                        var user = userService.UserRepository.FindBy(u => u.Name == userName).FirstOrDefault();
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<GoodNewsAppContext>(options => options.UseSqlServer(connectionString));


            //services.AddScoped(typeof(Repository<>), typeof(NewsRepository));
            //services.AddScoped(typeof(Repository<>), typeof(UserRepository));
            
           
            //TODO interfaces

            services.AddScoped<NewsRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<UserRoleRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<UnitOfWork>();
            services.AddScoped<NewsService>();

            services.AddSingleton<NewsFromFeedJob>();

            services.AddScoped<DbInitializer>(); //!!!!TODO

            services.AddAutoMapper(typeof(GoodNewsApp.BusinessLogic.Mapping.NewsProfile));

            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString,new SqlServerStorageOptions()
                { CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
                }));

            services.AddHangfireServer();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard();

            app.UseSwagger();

            app.UseSwaggerUI
            (c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        });


            //TODO hangfire: authorization!!!
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard(); 
            });

            //????
            var _dbInitializer = serviceProvider.GetService<DbInitializer>();
            _dbInitializer.InitializeWithUsersAndRoles();

            recurringJobManager.AddOrUpdate("GetAndStoreNews",() => serviceProvider.GetService<NewsFromFeedJob>().CreateAndStoreNewsFromFeed(), Cron.Daily());
        }
    }
}
