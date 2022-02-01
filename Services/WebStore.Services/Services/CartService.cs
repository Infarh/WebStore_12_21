using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services;

public class CartService : ICartService
{
    private readonly ICartStore _CartStore;
    private readonly IProductData _ProductData;

    public CartService(ICartStore CartStore, IProductData ProductData)
    {
        _CartStore = CartStore;
        _ProductData = ProductData;
    }

    public void Add(int Id)
    {
        var cart = _CartStore.Cart;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
        if (item is null)
            cart.Items.Add(new CartItem { ProductId = Id, Quantity = 1 });
        else
            item.Quantity += 2;

        _CartStore.Cart = cart;
    }

    public void Decrement(int Id)
    {
        var cart = _CartStore.Cart;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
        if (item is null)
            return;

        if (item.Quantity > 0)
            item.Quantity--;

        if (item.Quantity == 0)
            cart.Items.Remove(item);

        _CartStore.Cart = cart;
    }

    public void Remove(int Id)
    {
        var cart = _CartStore.Cart;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
        if (item is null)
            return;

        cart.Items.Remove(item);

        _CartStore.Cart = cart;
    }

    public void Clear()
    {
        //Cart = new();

        var cart = _CartStore.Cart;
        cart.Items.Clear();
        _CartStore.Cart = cart;
    }

    public CartViewModel GetViewModel()
    {
        var cart = _CartStore.Cart;
        var products = _ProductData.GetProducts(new()
        {
            Ids = cart.Items.Select(i => i.ProductId).ToArray()
        });

        var priducts_views = products.ToView().ToDictionary(p => p!.Id);

        return new()
        {
            Items = cart.Items
               .Where(item => priducts_views.ContainsKey(item.ProductId))
               .Select(item => (priducts_views[item.ProductId], item.Quantity))!
        };
    }
}