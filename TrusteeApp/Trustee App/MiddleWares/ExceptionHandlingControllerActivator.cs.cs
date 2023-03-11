using System.Net;
using System.Net.Http;
using System.Reflection;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Email;
using static System.Formats.Asn1.AsnWriter;

namespace TrusteeApp.MiddleWares
{
    public class ExceptionHandlingControllerActivator : IControllerActivator
    {
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly IConfiguration? _configuration;
        private static readonly log4net.ILog? log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly SignInManager<ApplicationUser>? _signInManager;
        private readonly UserManager<ApplicationUser>? _userManager;
        private readonly IWebHostEnvironment? _webHostEnvironment;
        private readonly IEmailSender? _emailSender;

        public ExceptionHandlingControllerActivator(IServiceCollection services)
        {
            var serviceProvider = services?.BuildServiceProvider()!;

            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            _configuration = serviceProvider.GetService<IConfiguration>();
            _webHostEnvironment = serviceProvider.GetService<IWebHostEnvironment>();
            _emailSender = serviceProvider.GetService<IEmailSender>();
            _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            _signInManager = serviceProvider.GetService<SignInManager<ApplicationUser>>();
        }

        public object Create(ControllerContext context)
        {
            try
            {
                var controllerType = context.ActionDescriptor.ControllerTypeInfo.AsType();

                return Activator.CreateInstance(controllerType, _configuration, _httpClientFactory, _userManager, _signInManager, _webHostEnvironment, _emailSender)!;
            }
            catch { throw; }
        }

        public void Release(ControllerContext context, object controller)
        {

        }
    }
}