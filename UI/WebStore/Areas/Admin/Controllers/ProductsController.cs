using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Areas.Admin.ViewModels;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;

namespace WebStore.Areas.Admin.Controllers;

[Area("Admin"), Authorize(Roles = Role.Administrators)]
public class ProductsController : Controller
{
    private readonly IProductData _ProductData;
    private readonly ILogger<ProductsController> _Logger;

    public ProductsController(IProductData ProductData, ILogger<ProductsController> Logger)
    {
        _ProductData = ProductData;
        _Logger = Logger;
    }

    public IActionResult Index()
    {
        var products = _ProductData.GetProducts();
        return View(products.Products);
    }

    public IActionResult Edit(int id)
    {
        var product = _ProductData.GetProductById(id);

        if (product is null)
            return NotFound();

        return View(new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            SectionId = product.SectionId,
            Section = product.Section.Name,
            Brand = product.Brand?.Name,
            BrandId = product.BrandId,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
        });
    }

    [HttpPost]
    public IActionResult Edit(EditProductViewModel Model)
    {
        if (!ModelState.IsValid)
            return View(Model);

        var product = _ProductData.GetProductById(Model.Id);
        if (product is null)
            return NotFound();

        //product.Name = Model.Name;
        //product.Order = Model.Order;
        //product.Price = Model.Price;
        //product.ImageUrl = Model.ImageUrl;

        //var brand = _ProductData.GetBrandById(Model.BrandId ?? -1);
        //var section = _ProductData.GetSectionById(Model.SectionId);

        //product.Brand = brand;
        //product.Section = section;

        //_ProductData.Update(product);

        // отредактировать product используя сервис _ProductData

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var product = _ProductData.GetProductById(id);

        if (product is null)
            return NotFound();

        return View(new EditProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Order = product.Order,
            SectionId = product.SectionId,
            Section = product.Section.Name,
            Brand = product.Brand?.Name,
            BrandId = product.BrandId,
            ImageUrl = product.ImageUrl,
            Price = product.Price,
        });
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int Id)
    {
        var product = _ProductData.GetProductById(Id);

        if (product is null)
            return NotFound();

        //_ProductData.Delete(product);
        // удалить product используя сервис _ProductData

        return RedirectToAction(nameof(Index));
    }
}