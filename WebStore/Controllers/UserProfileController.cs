using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers;

public class UserProfileController : Controller
{
    public IActionResult Index() => View();
}