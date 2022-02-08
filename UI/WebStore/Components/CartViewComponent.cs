using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;

namespace WebStore.Components;

public class CartViewComponent : ViewComponent
{
    #region Старый вариант
    //private readonly ICartService _CartService;

    //public CartViewComponent(ICartService CartService) => _CartService = CartService;

    //public IViewComponentResult Invoke()
    //{
    //    ViewBag.Count = _CartService.GetViewModel().ItemsCount;
    //    return View();
    //} 
    #endregion

    private readonly ICartStore _CartStore;
    public CartViewComponent(ICartStore CartStore) => _CartStore = CartStore;

    public IViewComponentResult Invoke()
    {
        ViewBag.Count = _CartStore.Cart.ItemsCount;
        return View();
    }
}