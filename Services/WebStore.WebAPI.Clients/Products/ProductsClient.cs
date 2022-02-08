using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Products;

public class ProductsClient : BaseClient, IProductData
{
    public ProductsClient(HttpClient Client) : base(Client, WebAPIAddresses.Products)
    {
    }

    public IEnumerable<Section> GetSections()
    {
        var sections = Get<IEnumerable<SectionDTO>>($"{Address}/sections");
        return sections!.FromDTO()!;
    }

    public Section? GetSectionById(int Id)
    {
        var section = Get<SectionDTO>($"{Address}/sections/{Id}");
        return section.FromDTO();
    }

    public IEnumerable<Brand> GetBrands()
    {
        var brands = Get<IEnumerable<BrandDTO>>($"{Address}/brands");
        return brands!.FromDTO()!;
    }

    public Brand? GetBrandById(int Id)
    {
        var brand = Get<BrandDTO>($"{Address}/brands/{Id}");
        return brand.FromDTO();
    }

    public ProductsPage GetProducts(ProductFilter? Filter = null)
    {
        var response = Post(Address, Filter ?? new());
        var products = response.Content.ReadFromJsonAsync<ProductsPageDTO>().Result;
        return products!.FromDTO()!;
    }

    public Product? GetProductById(int Id)
    {
        var product = Get<ProductDTO>($"{Address}/{Id}");
        return product.FromDTO();
    }

    public Product CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand = null)
    {
        var response = Post($"{Address}/new", new CreateProductDTO
        {
            Name = Name,
            Order = Order,
            Price = Price,
            ImageUrl = ImageUrl,
            Section = Section,
            Brand = Brand,
        });

        var product = response.Content.ReadFromJsonAsync<ProductDTO>().Result;
        return product!.FromDTO()!;
    }
}