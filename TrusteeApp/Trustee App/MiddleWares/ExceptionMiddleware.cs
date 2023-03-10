using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrusteeApp.Errors;

namespace TrusteeApp.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                log.Error(DateTime.Now.ToString(), ex);
                _logger.LogError(ex, ex.Message);
                LogWriterController.Write(ex.Message);

                var host = $"{context.Request.Host}";

                var urlString = $@"https://{host}/Home/Error?errorcode={(int)HttpStatusCode.NotFound}&errortype={"notFound"}&message={"Oop! Page you requested... doesn't exist"}&detail={"Sorry, We could not find the page you are looking for"}";

                var url = new Uri(urlString).ToString();

                context.Response.Redirect(url);
                return;
            }
        }
    }
}