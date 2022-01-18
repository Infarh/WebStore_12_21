using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default);

    Task<Order?> GetOrderByIdAsync(int Id, CancellationToken Cancel = default);

    Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken Cancel = default);
}