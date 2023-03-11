using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrusteeApp.Domain.Dtos;
using TrusteeApp.Errors;
using TrusteeApp.Filters;
using TrusteeApp.Repo;
using TrusteeApp.Services;
using TrusteeApp.ViewModels;

namespace TrusteeApp.Controllers
{
    public class ErrorController : Controller
    {
        public ErrorController()
        {
        }

        [HttpGet]
        [AllowAnonymous]
        [ViewLayout("_LoginLayout")]
        public IActionResult Error([FromQuery] string errorcode, string errortype, string message, string detail)
        {
            TempData["Error"] = $"Error message\r\n {message}.";

            ViewBag.ShowLayout = false;

            return View(new ApiExceptionsResponse(errorcode, errortype, message, detail));
        }
    }
}

