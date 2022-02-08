using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Services;

[TestClass]
public class CartServiceTests
{
    private Cart _Cart;

    private Mock<IProductData> _ProductDataMock;
    private Mock<ICartStore> _CartStoreMock;

    private ICartService _CartService;

    [TestInitialize]
    public void TestInitialize()
    {
        _Cart = new Cart
        {
            Items = new List<CartItem>
            {
                new() { ProductId = 1, Quantity = 1 },
                new() { ProductId = 2, Quantity = 3 },
            }
        };

        _ProductDataMock = new Mock<IProductData>();
        _ProductDataMock
           .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
           .Returns(new ProductsPage(new[]
            {
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Price = 1.1m,
                    Order = 1,
                    ImageUrl = "img_1.png",
                    Brand = new Brand { Id = 1, Name = "Brand 1", Order = 1},
                    SectionId = 1,
                    Section = new Section{ Id = 1, Name = "Section 1", Order = 1 },
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Price = 2.2m,
                    Order = 2,
                    ImageUrl = "img_2.png",
                    Brand = new Brand { Id = 2, Name = "Brand 2", Order = 2},
                    SectionId = 2,
                    Section = new Section{ Id = 2, Name = "Section 2", Order = 2 },
                },
                new Product
                {
                    Id = 3,
                    Name = "Product 3",
                    Price = 3.3m,
                    Order = 3,
                    ImageUrl = "img_3.png",
                    Brand = new Brand { Id = 3, Name = "Brand 3", Order = 3},
                    SectionId = 3,
                    Section = new Section{ Id = 3, Name = "Section 3", Order = 3 },
                },
            }, 3));

        _CartStoreMock = new Mock<ICartStore>();
        _CartStoreMock.Setup(c => c.Cart).Returns(_Cart);

        _CartService = new CartService(_CartStoreMock.Object, _ProductDataMock.Object);
    }

    [TestMethod]
    public void Cart_Class_ItemsCount_returns_Correct_Quantity()
    {
        var cart = _Cart;

        var expected_items_count = cart.Items.Sum(i => i.Quantity);

        var actual_items_count = cart.ItemsCount;

        Assert.Equal(expected_items_count, actual_items_count);
    }

    [TestMethod]
    public void CartViewModel_Returns_Correct_ItemsCount()
    {
        var cart_view_model = new CartViewModel
        {
            Items = new[]
            {
                ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1 ),
                ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 1.5m }, 3 ),
            }
        };

        var expected_items_count = cart_view_model.Items.Sum(i => i.Quantity);

        var actual_items_count = cart_view_model.ItemsCount;

        Assert.Equal(expected_items_count, actual_items_count);
    }

    [TestMethod]
    public void CartViewModel_Returns_Correct_TotalPrice()
    {
        var cart_view_model = new CartViewModel
        {
            Items = new[]
            {
                ( new ProductViewModel { Id = 1, Name = "Product 1", Price = 0.5m }, 1 ),
                ( new ProductViewModel { Id = 2, Name = "Product 2", Price = 1.5m }, 3 ),
            }
        };

        var expected_total_price = cart_view_model.Items.Sum(item => item.Quantity * item.Product.Price);

        var actual_total_price = cart_view_model.TotalPrice;

        Assert.Equal(expected_total_price, actual_total_price);
    }

    [TestMethod]
    public void CartService_Add_WorkCorrect()
    {
        _Cart.Items.Clear();

        const int expected_id = 5;
        const int expected_items_count = 1;

        _CartService.Add(expected_id);

        var actual_items_count = _Cart.ItemsCount;

        Assert.Equal(expected_items_count, actual_items_count);

        Assert.Single(_Cart.Items);

        Assert.Equal(expected_id, _Cart.Items.Single().ProductId);
        //Assert.Equal(expected_id, _Cart.Items.ElementAt(0).ProductId);
    }

    [TestMethod]
    public void CartService_Remove_Correct_Item()
    {
        const int item_id = 1;
        const int expected_product_id = 2;

        _CartService.Remove(item_id);

        Assert.Single(_Cart.Items);

        Assert.Equal(expected_product_id, _Cart.Items.Single().ProductId);
    }

    [TestMethod]
    public void CartService_Clear_ClearCart()
    {
        _CartService.Clear();

        Assert.Empty(_Cart.Items);
    }

    [TestMethod]
    public void CartService_Decrement_Correct()
    {
        const int item_id = 2;

        const int expected_quantity = 2;
        const int expectes_items_count = 3;
        const int expected_products_count = 2;

        _CartService.Decrement(item_id);

        Assert.Equal(expectes_items_count, _Cart.ItemsCount);
        Assert.Equal(expected_products_count, _Cart.Items.Count);

        var items = _Cart.Items.ToArray();
        Assert.Equal(item_id, items[1].ProductId);
        Assert.Equal(expected_quantity, items[1].Quantity);
    }

    [TestMethod]
    public void CartService_Remove_Item_When_Decrement_to_0()
    {
        const int item_id = 1;
        const int expected_items_count = 3;

        _CartService.Decrement(item_id);

        Assert.Equal(expected_items_count, _Cart.ItemsCount);
        Assert.Single(_Cart.Items);
    }

    [TestMethod]
    public void CartService_GetViewModel_WorkCorrect()
    {
        const int expected_items_count = 4;
        const decimal expected_first_product_price = 1.1m;

        var result = _CartService.GetViewModel();

        Assert.Equal(expected_items_count, result.ItemsCount);

        Assert.Equal(expected_first_product_price, result.Items.First().Product.Price);
    }
}