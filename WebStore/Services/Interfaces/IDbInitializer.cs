namespace WebStore.Services.Interfaces;

public interface IDbInitializer
{
    Task<bool> RemoveAsync(CancellationToken Cancel = default);

    Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default);
}