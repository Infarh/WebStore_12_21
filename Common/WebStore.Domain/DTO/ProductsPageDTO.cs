using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO;

public record ProductsPageDTO(IEnumerable<ProductDTO> Products, int TotalCount);

public static class ProductsPageDTOMapper
{
    public static ProductsPageDTO ToDTO(this ProductsPage Page) => new(Page.Products.ToDTO(), Page.TotalCount);

    public static ProductsPage FromDTO(this ProductsPageDTO Page) => new(Page.Products.FromDTO(), Page.TotalCount);
}