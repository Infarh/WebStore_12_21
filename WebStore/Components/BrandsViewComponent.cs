using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components;

public class BrandsViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}