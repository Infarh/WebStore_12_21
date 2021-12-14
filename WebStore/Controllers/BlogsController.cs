using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    //[Controller]
    public class BlogsController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Blog() => View();
    }
}
