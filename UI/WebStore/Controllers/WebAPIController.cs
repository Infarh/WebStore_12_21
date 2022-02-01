using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.TestAPI;

namespace WebStore.Controllers;

public class WebAPIController : Controller
{
    private readonly IValuesService _ValuesService;

    public WebAPIController(IValuesService ValuesService) => _ValuesService = ValuesService;

    public IActionResult Index()
    {
        //var values = _ValuesService.GetValues();
        //return View(values);
        return View(Enumerable.Empty<string>());
    }
}