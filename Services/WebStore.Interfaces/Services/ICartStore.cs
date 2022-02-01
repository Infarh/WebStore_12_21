using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;

public interface ICartStore
{
    public Cart Cart { get; set; }
}