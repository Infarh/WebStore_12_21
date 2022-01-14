using Microsoft.AspNetCore.Mvc;

namespace WebStore.Areas.Admin.Controllers;

public class ProductsController : Controller
{
    public IActionResult Index() => View();
}