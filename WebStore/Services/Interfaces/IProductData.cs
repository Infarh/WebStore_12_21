using WebStore.Domain;
using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces;

public interface IProductData
{
    IEnumerable<Section> GetSections();

    IEnumerable<Brand> GetBrands();

    IEnumerable<Product> GetProducts(ProductFilter? Filter = null);
}