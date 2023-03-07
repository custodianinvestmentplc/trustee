using Microsoft.AspNetCore.Mvc;

namespace TrusteeApp.Components
{
    public class Preloader : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
