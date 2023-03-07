using TrusteeApp.Domain;
using TrusteeApp.Models;
using TrusteeApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http.Headers;
using TrusteeApp.Repo;
using AutoMapper;
using TrusteeApp.MiddleWares;
using System.Configuration;
using TrusteeApp.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using TrusteeApp.Email;
using Trustee_App.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace TrusteeApp
{
    public class Startup
    {
        private readonly LoginConfig _loginConfig;
        private readonly DatabaseConfig _dbConfig;

        private readonly string? _baseUrl;
        private readonly MediaTypeWithQualityHeaderValue _contentType;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            var config = new LoginConfig();
            var db = new DatabaseConfig();

            string? baseUrl = configuration.GetValue<string>("AppSettings:BaseUrl");
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");

            configuration.GetSection("auth").Bind(config);
            configuration.GetSection("databases").Bind(db);

            _loginConfig = config;
            _dbConfig = db;
            _baseUrl = baseUrl;
            _contentType = contentType;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddScoped<CustomActionFilter>();


            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<IdentityRole>, RoleStore>();

            //services.AddAuthentication(o =>
            //{
            //    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //{
            //    options.Cookie.Name = "RDS.Custodian.Cookie";
            //    options.LoginPath = new PathString("/Account/Login");
            //});

            services.AddHttpClient("client", httpClient =>
            {
                httpClient.BaseAddress = new Uri((_baseUrl ?? ""));
                httpClient.DefaultRequestHeaders.Accept.Add(_contentType);
            });

            //services.AddSingleton(_loginConfig);
            services.AddSingleton(_dbConfig);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });

            //services.AddScoped<ICPCHubServices>(s => new CPCHubServiceFacade(_dbConfig.cpc));
            //services.AddScoped<IAuthProvider, AuthProvider>();

            //services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllersWithViews();

            //services.Configure<UserRegisterDto>(configuration.GetSection("userregister"));

            //services.AddControllers(options =>
            //{
            //    options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();

                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            var cookiePolicyOptions = new CookiePolicyOptions
            {
                //CheckConsentNeeded = x => true,
                MinimumSameSitePolicy = SameSiteMode.Lax,
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = env.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always
            };

            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            //app.UseMiddleware<UserPermissionsMiddleWare>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
