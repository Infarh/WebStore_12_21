using WebStore.Domain.ViewModels;

namespace WebStore.Services.Interfaces;

public interface ICartService
{
    void Add(int Id);

    void Decrement(int Id);

    void Remove(int Id);

    void Clear();

    CartViewModel GetViewModel();
}