using Microsoft.AspNetCore.Mvc;
using QRCoder;

using WebStore.Models;

namespace WebStore.Controllers;

public class HomeController : Controller
{
    private static readonly List<Employee> __Employees = new()
    {
        new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 23 },
        new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 27 },
        new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
    };

    public IActionResult Index()
    {
        //return Content("Данные из первого контроллера");
        return View();
    }

    public string ConfiguredAction(string id, string Value1)
    {
        return $"Hello World! {id} - {Value1}";
    }

    public IActionResult Employees()
    {
        return View(__Employees);
    }

    public IActionResult QR(string str)
    {
        var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
        var code = new PngByteQRCode(data);
        var image = code.GetGraphic(20);

        return File(image, "image/png");
    }
}