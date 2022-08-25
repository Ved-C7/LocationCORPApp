using Microsoft.AspNetCore.Mvc;

namespace LocationCORPApp.ViewComponents
{
    public class DropdownViewComponent : ViewComponent
    {
        public DropdownViewComponent()
        {
        }
        public IViewComponentResult Invoke()
        {
            return View("Index");
        }
    }
}
