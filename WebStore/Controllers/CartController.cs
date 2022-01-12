using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers;

public class CartController : Controller
{
    private readonly ICartService _CartService;

    public CartController(ICartService CartService) => _CartService = CartService;

    public IActionResult Index() => View(_CartService.GetViewModel());

    public IActionResult Add(int Id)
    {
        _CartService.Add(Id);
        return RedirectToAction("Index", "Cart");
    }

    public IActionResult Decrement(int Id)
    {
        _CartService.Decrement(Id);
        return RedirectToAction("Index", "Cart");
    }

    public IActionResult Remove(int Id)
    {
        _CartService.Remove(Id);
        return RedirectToAction("Index", "Cart");
    }
}