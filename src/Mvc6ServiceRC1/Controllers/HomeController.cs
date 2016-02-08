using Microsoft.AspNet.Mvc;
using Mvc6ServiceRC1.Models;

namespace Mvc6ServiceRC1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new HomeModel();
            return View(model);
        }
    }
}
