using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Services;

[TestClass]
public class CartServiceTests
{
    private Cart _Cart;

    private Mock<IProductData> _ProductDataMock;

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
           .Returns(new[]
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
            });
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
}