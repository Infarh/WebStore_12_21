using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers;

public class CatalogController : Controller
{
    private readonly IProductData _ProductData;
    private readonly IConfiguration _Configuration;

    public CatalogController(IProductData ProductData, IConfiguration Configuration)
    {
        _ProductData = ProductData;
        _Configuration = Configuration;
    }

    public IActionResult Index(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
    {
        var page_size = PageSize
            ?? (int.TryParse(_Configuration["CatalogPageSize"], out var value) ? value : null);

        var filter = new ProductFilter
        {
            BrandId = BrandId,
            SectionId = SectionId,
            Page = Page,
            PageSize = page_size,
        };

        var products = _ProductData.GetProducts(filter);

        var catalog_model = new CatalogViewModel
        {
            BrandId = BrandId,
            SectionId = SectionId,
            Products = products.Products.OrderBy(p => p.Order).ToView(),
        };

        return View(catalog_model);
    }

    public IActionResult Details(int Id)
    {
        var product = _ProductData.GetProductById(Id);
        //CultureInfo.CurrentUICulture = 
        //    CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        if (product is null)
            return NotFound();

        return View(product.ToView());
    }
}