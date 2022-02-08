using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route(WebAPIAddresses.Products)]
public class ProductsApiController : ControllerBase
{
    private readonly IProductData _ProductData;

    public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

    [HttpGet("sections")] // GET -> http://localhost:5001/api/products/sections
    public IActionResult GetSections()
    {
        var sections = _ProductData.GetSections();
        return Ok(sections.ToDTO());
    }

    [HttpGet("sections({Skip}-{Take}")] // GET -> http://localhost:5001/api/products/sections(5-7)
    public IActionResult GetSections(int Skip, int Take)
    {
        var sections = _ProductData.GetSections(Skip, Take);
        return Ok(sections.ToDTO());
    }

    [HttpGet("sections/{Id}")] // GET -> http://localhost:5001/api/products/sections/5
    public IActionResult GetSectionById(int Id)
    {
        var section = _ProductData.GetSectionById(Id);
        if (section is null)
            return NotFound();

        return Ok(section.ToDTO());
    }

    [HttpGet("sections/count")]
    public IActionResult GetSectionsCount()
    {
        var count = _ProductData.GetSectionsCount();
        return Ok(count);
    }

    [HttpGet("brands")] // GET -> http://localhost:5001/api/products/brands
    public IActionResult GetBrands()
    {
        var brands = _ProductData.GetBrands();
        return Ok(brands.ToDTO());
    }

    [HttpGet("brands({Skip}-{Take}")] // GET -> http://localhost:5001/api/products/brands(5-7)
    public IActionResult GetBrands(int Skip, int Take)
    {
        var brands = _ProductData.GetBrands();
        return Ok(brands.ToDTO());
    }

    [HttpGet("brands/{Id}")] // GET -> http://localhost:5001/api/products/brands/5
    public IActionResult GetBrandById(int Id)
    {
        var brand = _ProductData.GetBrandById(Id);
        if (brand is null)
            return NotFound();

        return Ok(brand.ToDTO());
    }

    [HttpGet("brands/count")]
    public IActionResult GetBrandsCount()
    {
        var count = _ProductData.GetBrandsCount();
        return Ok(count);
    }

    [HttpPost]
    public IActionResult GetProducts(ProductFilter? Filter = null)
    {
        var products = _ProductData.GetProducts(Filter);
        return Ok(products.ToDTO());
    }

    [HttpGet("{Id}")]
    public IActionResult GetProductById(int Id)
    {
        var product = _ProductData.GetProductById(Id);
        if (product is null)
            return NotFound();

        return Ok(product.ToDTO());
    }

    [HttpPost("new/{Name}")]
    public IActionResult CreateProduct(CreateProductDTO Model)
{
        var product = _ProductData.CreateProduct(Model.Name, Model.Order, Model.Price, Model.ImageUrl, Model.Section, Model.Brand);
        return CreatedAtAction(nameof(GetProductById), new { product.Id }, product.ToDTO());
    }
}