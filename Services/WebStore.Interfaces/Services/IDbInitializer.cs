namespace WebStore.Interfaces.Services;

public interface IDbInitializer
{
    Task<bool> RemoveAsync(CancellationToken Cancel = default);

    Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default);
}