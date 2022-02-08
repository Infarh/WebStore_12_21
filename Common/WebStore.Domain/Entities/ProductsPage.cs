namespace WebStore.Domain.Entities;

public record ProductsPage(IEnumerable<Product> Products, int TotalCount);