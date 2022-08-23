using Microsoft.AspNetCore.Mvc;

namespace LocationCORPApp.Controllers
{
    public class UserList : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
