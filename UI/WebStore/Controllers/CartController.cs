using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers;

public class CartController : Controller
{
    private readonly ICartService _CartService;

    public CartController(ICartService CartService) => _CartService = CartService;

    public IActionResult Index() => View(new CartOrderViewModel { Cart = _CartService.GetViewModel() });

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

    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
    {
        if (!ModelState.IsValid)
            return View(nameof(Index), new CartOrderViewModel
            {
                Cart = _CartService.GetViewModel(),
                Order = OrderModel
            });

        var order = await OrderService.CreateOrderAsync(
            User.Identity!.Name!,
            _CartService.GetViewModel(),
            OrderModel);

        _CartService.Clear();

        return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
    }

    public IActionResult OrderConfirmed(int Id)
    {
        ViewBag.OrderId = Id;
        return View();
    }
}