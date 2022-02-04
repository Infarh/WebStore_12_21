using Microsoft.AspNetCore.Mvc;

using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components;

public class BreadCrumbsViewComponent : ViewComponent
{
    private readonly IProductData _ProductData;

    public BreadCrumbsViewComponent(IProductData ProductData) => _ProductData = ProductData;

    public IViewComponentResult Invoke()
    {
        var model = new BreadCrumbsViewModel();

        if (int.TryParse(Request.Query["SectionId"], out var section_id))
        {
            model.Section = _ProductData.GetSectionById(section_id);
            if (model.Section?.ParentId is { } parent_section_id && model.Section.Parent is null)
                model.Section.Parent = _ProductData.GetSectionById(parent_section_id)!;
        }

        if (int.TryParse(Request.Query["BrandId"], out var brand_id))
            model.Brand = _ProductData.GetBrandById(brand_id);

        if (int.TryParse(Request.RouteValues["id"]?.ToString(), out var product_id))
            model.Product = _ProductData.GetProductById(product_id)?.Name;

        return View(model);
    }
}