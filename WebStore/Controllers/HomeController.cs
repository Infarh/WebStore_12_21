using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers;

public class HomeController : Controller
{
   public IActionResult Index()
    {
        //ControllerContext.HttpContext.Request.RouteValues

        //return Content("Данные из первого контроллера");
        return View();
    }

    public string ConfiguredAction(string id, string Value1)
    {
        return $"Hello World! {id} - {Value1}";
    }

    public void Throw(string Message) => throw new ApplicationException(Message);
}