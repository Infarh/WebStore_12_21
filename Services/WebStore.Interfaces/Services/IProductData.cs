using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface IProductData
{
    IEnumerable<Section> GetSections(int Skip = 0, int? Take = null);

    int GetSectionsCount();

    Section? GetSectionById(int Id);

    IEnumerable<Brand> GetBrands(int Skip = 0, int? Take = null);

    int GetBrandsCount();

    Brand? GetBrandById(int Id);

    ProductsPage GetProducts(ProductFilter? Filter = null);

    Product? GetProductById(int Id);

    Product CreateProduct(string Name, int Order, decimal Price, string ImageUrl, string Section, string? Brand = null);
}