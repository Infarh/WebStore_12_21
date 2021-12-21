using Microsoft.AspNetCore.Mvc;

using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

public class HomeController : Controller
{
    public IActionResult Index([FromServices] IProductData ProductData)
    {
        var products = ProductData.GetProducts()
               .OrderBy(p => p.Order)
               .Take(6)
               .Select(p => new ProductViewModel
                {
                   Id = p.Id,
                   Name = p.Name,
                   Price = p.Price,
                   ImageUrl = p.ImageUrl,
                });
        ViewBag.Products = products;

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