using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsApiController : Controller
{
    private readonly IProductData _ProductData;

    public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

    [HttpGet("sections")] // GET -> http://localhost:5001/api/products/sections
    public IActionResult GetSections()
    {
        var sections = _ProductData.GetSections();
        return Ok(sections);
    }

    [HttpGet("sections/{Id}")] // GET -> http://localhost:5001/api/products/sections/5
    public IActionResult GetSectionById(int Id)
    {
        var section = _ProductData.GetSectionById(Id);
        if (section is null)
            return NotFound();

        return Ok(section);
    }

    [HttpGet("brands")] // GET -> http://localhost:5001/api/products/brands
    public IActionResult GetBrands()
    {
        var brands = _ProductData.GetBrands();
        return Ok(brands);
    }

    [HttpGet("brands/{Id}")] // GET -> http://localhost:5001/api/products/brands/5
    public IActionResult GetBrandById(int Id)
    {
        var brand = _ProductData.GetBrandById(Id);
        if (brand is null)
            return NotFound();

        return Ok(brand);
    }

    [HttpPost]
    public IActionResult GetProducts(ProductFilter? Filter = null)
    {
        var products = _ProductData.GetProducts(Filter);
        return Ok(products);
    }

    [HttpGet("{Id}")]
    public IActionResult GetProductGyId(int Id)
    {
        var product = _ProductData.GetSectionById(Id);
        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost("new/{Name}")]
    public IActionResult CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand = null)
    {
        var product = _ProductData.CreateProduct(Name, Order, Price, ImageUrl, Section, Brand);
        return CreatedAtAction(nameof(GetProductGyId), new { product.Id }, product);
    }
}