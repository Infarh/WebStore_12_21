namespace WebStore.Domain.ViewModels;

public class CartViewModel
{
    public IEnumerable<(ProductViewModel Product, int Quantity)> Items { get; set; }

    public int ItemsCount => Items.Sum(p => p.Quantity);

    public decimal TotalPrice => Items.Sum(p => p.Product.Price * p.Quantity);
}