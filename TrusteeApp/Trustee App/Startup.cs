using System.Net.Http.Headers;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Trustee_App.Services;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Email;
using TrusteeApp.MiddleWares;
using TrusteeApp.Models;

namespace TrusteeApp
{
    public class Startup
    {
        //private readonly LoginConfig _loginConfig;
        //private readonly DatabaseConfig _dbConfig;

        private readonly string? _baseUrl;
        private readonly MediaTypeWithQualityHeaderValue _contentType;
        private readonly IConfiguration configuration;

        private IApplicationBuilder? _app;

        public Startup(IConfiguration configuration)
        {
            var config = new LoginConfig();
            var db = new DatabaseConfig();

            string? baseUrl = configuration.GetValue<string>("AppSettings:BaseUrl");
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");

            configuration.GetSection("auth").Bind(config);
            configuration.GetSection("databases").Bind(db);

            //_loginConfig = config;
            //_dbConfig = db;
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

            //services.AddSingleton(_dbConfig);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession(Options =>
            {
                Options.IdleTimeout = TimeSpan.FromMinutes(10);
                Options.Cookie.HttpOnly = true;
                Options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddSingleton<IControllerActivator>(new ExceptionHandlingControllerActivator(services));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //    if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            //    else app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.Map("/errorresponse", errorresponse => {

                errorresponse.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Internal server error occured while creating API controller.");
                });
            });

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
