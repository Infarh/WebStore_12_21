using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services;

public class InMemoryProductData : IProductData
{
    public IEnumerable<Section> GetSections() => TestData.Sections;

    public IEnumerable<Brand> GetBrands() => TestData.Brands;

    public IEnumerable<Product> GetProducts(ProductFilter? Filter = null)
    {
        IEnumerable<Product> query = TestData.Products;

        //if (Filter?.SectionId != null)
        //    query = query.Where(p => p.SectionId == Filter.SectionId);

        if (Filter is { SectionId: var section_id })
            query = query.Where(p => p.SectionId == section_id);

        if (Filter is { BrandId: var brand_id })
            query = query.Where(p => p.BrandId == brand_id);

        return query;
    }
}