using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrusteeApp.Filters
{
    public class MyAsyncCustomActionFilter : Attribute,IAsyncActionFilter, IAsyncResultFilter,IOrderedFilter
    {
        
        public int Order { get; set; }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            DoLogging("On Action Executing", context.RouteData, context.HttpContext);


            int Hours = Convert.ToInt32(DateTime.Now.ToString("HH"));

            if (Hours < 10)
            {
                await context.HttpContext.Response.WriteAsync("<h1>You cant access this area before 3 PM..</h1>");
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);

            }


            var resultContext = await next();

            DoLogging("On Action Executed", context.RouteData, context.HttpContext);
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            DoLogging("On Result Executing", context.RouteData, context.HttpContext);

            var resultContext = await next();

            DoLogging("On Result Executed", context.RouteData, context.HttpContext);
        }

        public void DoLogging(string FunctionName, RouteData routedata, HttpContext _httpContext)
        {
            string Controller, Action;

            Controller = routedata.Values["controller"].ToString();
            Action = routedata.Values["action"].ToString();
            string str = string.Format("2 - Function Name ={0}, Controller Name={1}, Action={2}", FunctionName, Controller, Action);
            _httpContext.Response.WriteAsync("<br>" + str + "<br>");


        }
    }
}
