using Microsoft.AspNet.Mvc;
using Mvc6Service461.Models;

namespace Mvc6Service461.Controllers
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
