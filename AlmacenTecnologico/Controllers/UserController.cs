using Microsoft.AspNetCore.Mvc;

namespace AlmacenTecnologico.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
